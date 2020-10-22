using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RoosterPlanner.Common;
using RoosterPlanner.Common.Config;
using RoosterPlanner.Service.DataModels;
using RoosterPlanner.Service.DataModels.B2C;
using RoosterPlanner.Service.Helpers;

namespace RoosterPlanner.Service
{
    public interface IAzureB2CService
    {
        Task<TaskResult<AppUser>> CreateUserAsync(B2cUser b2cUser);

        Task<User> GetUserAsync(Guid userId);

        Task<TaskResult<IEnumerable<User>>> GetAllUsersAsync();

        Task<TaskResult<List<AppUser>>> GetUsersAsync(string displayName, int offset, int pageSize);

        Task<TaskResult<AppUser>> UpdateUserAsync(AppUser user);

        Task<TaskResult> UpdatePasswordAsync(Guid userId, string userToken, string oldPassword, string newPassword);

        Task<TaskResult> DeleteUserAsync(Guid userId);

        Task<string> GetAzureADTokenAsync(AzureAuthenticationConfig azureB2cConfig);
    }

    public class AzureB2CService : IAzureB2CService
    {
        #region Fields

        private readonly AzureAuthenticationConfig azureB2cConfig = null;
        private GraphServiceClient graphServiceClient = null;
        private DateTime graphServiceClientTimestamp = new DateTime();

        private const string graphSelectList =
            "id,identities,accountEnabled,creationType,createdDateTime,displayName,givenName,surname,mail,otherMails,mailNickname,userPrincipalName,mobilePhone,usageLocation,userType,streetAddress,postalCode,city,country,preferredLanguage,refreshTokensValidFromDateTime,extensions,JobTitle,BusinessPhones,Department,OfficeLocation, DeletedDateTime,AdditionalData";

        #endregion

        //Registered B2C ApplicationId in Azure AD
        private string AzureB2cApplicationId
        {
            get { return this.azureB2cConfig.GraphB2cApplicationId.Replace("-", ""); }
        }

        //Constructor
        public AzureB2CService(IOptions<AzureAuthenticationConfig> azureB2cConfig)
        {
            this.azureB2cConfig = azureB2cConfig.Value;
        }

        public async Task<TaskResult<AppUser>> CreateUserAsync(B2cUser b2cUser)
        {
            if (b2cUser == null)
                throw new ArgumentNullException("b2cUser");

            TaskResult<AppUser> result = new TaskResult<AppUser>
                {StatusCode = HttpStatusCode.NoContent, Succeeded = false};

            try
            {
                //Get token for access to Microsoft Graph as this application
                string accessToken = await GetAzureADTokenAsync(this.azureB2cConfig);
                if (String.IsNullOrEmpty(accessToken))
                    return new TaskResult<AppUser>
                        {StatusCode = HttpStatusCode.Unauthorized, Message = "Failed to get authentication token."};

                string userData =
                    JsonConvert.SerializeObject(b2cUser, JsonSerializerSettingsExtensions.DefaultSettings());

                string resourcePath =
                    $"{this.azureB2cConfig.ResourcePathUsers}?api-version={this.azureB2cConfig.AzureADApiVersion}";
                Uri requestUri =
                    new Uri(this.azureB2cConfig.AzureADBaseUrl).AddPath(this.azureB2cConfig.TenantId, resourcePath);

                HttpResponseMessage response =
                    await SendRequestAsync(HttpMethod.Post, requestUri, accessToken, userData);
                result.StatusCode = response.StatusCode;
                result.Succeeded = response.IsSuccessStatusCode;
                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    result.Data = JsonConvert.DeserializeObject<AppUser>(data);
                    result.Data.CreatedDateTime = DateTime.UtcNow;
                    result.Data.Email = result.Data.SignInName;
                }
                else
                {
                    result.Error = new HttpRequestException(await response.Content.ReadAsStringAsync());
                }
            }
            catch (Exception ex)
            {
                b2cUser.PasswordProfile.Password = null; //Set to null for security reasons!
                result.StatusCode = HttpStatusCode.InternalServerError;
                result.Message = ex.Message;
                result.Error = ex;
            }

            return result;
        }

        public async Task<User> GetUserAsync(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentNullException("userId");

            User user = null;

            try
            {
                B2cCustomAttributeHelper helper = new B2cCustomAttributeHelper(azureB2cConfig.B2CExtentionApplicationId);
                GraphServiceClient graphService = this.GetGraphServiceClient(this.azureB2cConfig);
                string userRole = helper.GetCompleteAttributeName("UserRole");
                string dateOfBirth = helper.GetCompleteAttributeName("DateOfBirth");
                string phoneNumber = helper.GetCompleteAttributeName("PhoneNumber");

                user = await graphService.Users[userId.ToString()].Request()
                    .Select($"{graphSelectList},{userRole},{dateOfBirth},{phoneNumber}").GetAsync();
            }


            catch (ServiceException graphEx)
            {
                throw graphEx;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return user;
        }
        //todo pagination inbouwen
        public async Task<TaskResult<IEnumerable<User>>> GetAllUsersAsync()
        {
            TaskResult<IEnumerable<User>> result = new TaskResult<IEnumerable<User>> 
                {StatusCode = HttpStatusCode.NoContent, Succeeded = false, Data = new List<User>()};;
            IEnumerable<User> users = null;

            try
            {
                if (string.IsNullOrEmpty(azureB2cConfig.B2CExtentionApplicationId))
                {
                    throw new ArgumentException(("B2CExtentionApplicationId is null"));
                }
                B2cCustomAttributeHelper helper = new B2cCustomAttributeHelper(azureB2cConfig.B2CExtentionApplicationId);
                GraphServiceClient graphService = this.GetGraphServiceClient(this.azureB2cConfig);
                string userRole = helper.GetCompleteAttributeName("UserRole");
                string dateOfBirth = helper.GetCompleteAttributeName("DateOfBirth");
                string phoneNumber = helper.GetCompleteAttributeName("PhoneNumber");

                users = await graphService.Users
                    .Request()
                    .Select($"{graphSelectList},{userRole},{dateOfBirth},{phoneNumber}")
                    .GetAsync();
               
                if (users == null)
                {
                    throw new NullReferenceException("No users found");
                }

                result.StatusCode = HttpStatusCode.OK;
                result.Succeeded = true;
                result.Data = users;
                return result;
            }


            catch (ServiceException graphEx)
            {
                result.Succeeded = false;
                result.StatusCode = graphEx.StatusCode;
                result.Message = graphEx.Message;
                throw graphEx;
            }
            catch (Exception ex)
            {
                result.Succeeded = false;
                result.StatusCode = HttpStatusCode.InternalServerError;
                result.Message = ex.Message;
                throw ex;
            }

            return result;
        }

        public async Task<TaskResult<List<AppUser>>> GetUsersAsync(string search, int offset, int pageSize)
        {
            int blockSize = new int[2] {50, (offset + pageSize)}.Min();
            if (offset < 0)
                offset = 0;

            TaskResult<List<AppUser>> result = new TaskResult<List<AppUser>>
                {StatusCode = HttpStatusCode.NoContent, Succeeded = false, Data = new List<AppUser>()};

            try
            {
                //Get token for access to Microsoft Graph as this application
                string accessToken = await GetAzureADTokenAsync(this.azureB2cConfig);
                if (String.IsNullOrEmpty(accessToken))
                    return new TaskResult<List<AppUser>>
                    {
                        StatusCode = HttpStatusCode.Unauthorized, Message = "Failed to get authentication token.",
                        Data = new List<AppUser>()
                    };

                UriBuilder builder = new UriBuilder(this.azureB2cConfig.AzureADBaseUrl);
                builder.Path =
                    UriExtensions.CombinePath(this.azureB2cConfig.TenantId, this.azureB2cConfig.ResourcePathUsers);
                builder.Query = $"api-version={this.azureB2cConfig.AzureADApiVersion}&$top={blockSize}";

                HttpResponseMessage responseMessage =
                    await SendRequestAsync(HttpMethod.Get, builder.Uri, accessToken, null);
                result.Succeeded = responseMessage.IsSuccessStatusCode;
                result.StatusCode = responseMessage.StatusCode;
                if (responseMessage.IsSuccessStatusCode)
                {
                    string data = await responseMessage.Content.ReadAsStringAsync();
                    GraphUserData userData = JsonConvert.DeserializeObject<GraphUserData>(data);

                    ValueTuple<List<AppUser>, int> blockData =
                        GetPartToAddFromBlock(userData.Value, result.Data.Count, search, offset, pageSize);
                    if (blockData.Item1.Count != 0)
                        result.Data.AddRange(blockData.Item1);
                    offset = blockData.Item2;

                    //Retrieve more data?
                    while (result.Data.Count < pageSize && !String.IsNullOrEmpty(userData.odataNextLink))
                    {
                        UriBuilder nextLinkBuilder = new UriBuilder(userData.odataNextLink);
                        builder.Query = $"{nextLinkBuilder.Query}&api-version={this.azureB2cConfig.AzureADApiVersion}";

                        responseMessage = await SendRequestAsync(HttpMethod.Get, builder.Uri, accessToken, null);
                        result.Succeeded = responseMessage.IsSuccessStatusCode;
                        result.StatusCode = responseMessage.StatusCode;
                        if (responseMessage.IsSuccessStatusCode)
                        {
                            data = await responseMessage.Content.ReadAsStringAsync();
                            userData = JsonConvert.DeserializeObject<GraphUserData>(data);

                            blockData = GetPartToAddFromBlock(userData.Value, result.Data.Count, search, offset,
                                pageSize);
                            if (blockData.Item1.Count != 0)
                                result.Data.AddRange(blockData.Item1);
                            offset = blockData.Item2;
                        }
                    }

                    //string roleKey = $"extension_{AzureB2cApplicationId}_UserRole";
                    //string company = $"extension_{AzureB2cApplicationId}_UserDepartment";

                    Parallel.ForEach<AppUser>(result.Data, usr =>
                    {
                        if (usr.OtherMails != null && usr.OtherMails.Length != 0)
                            usr.Email = usr.OtherMails[0];
                        else
                            usr.Email = usr.SignInName;
                        //if (usr.AdditionalData != null && usr.AdditionalData.Count != 0)
                        //{
                        //    usr.UserRole = usr.AdditionalData.ContainsKey(roleKey) ? usr.AdditionalData[roleKey].ToString() : null;
                        //    usr.UserDepartment = usr.AdditionalData.ContainsKey(company) ? usr.AdditionalData[company].ToString() : null;
                        //}
                    });
                }
                else
                {
                    result.Message = await responseMessage.Content.ReadAsStringAsync();
                }
            }
            catch (ServiceException graphEx)
            {
                result.Succeeded = false;
                if (graphEx.IsMatch("Request_ResourceNotFound"))
                    result.StatusCode = HttpStatusCode.NotFound;
                else
                    result.StatusCode = graphEx.StatusCode;

                result.Message = graphEx.Error?.Message ?? graphEx.Message;
                result.Error = graphEx;
            }
            catch (Exception ex)
            {
                result.StatusCode = HttpStatusCode.InternalServerError;
                result.Message = ex.Message;
                result.Error = ex;
            }

            return result;
        }

        public async Task<TaskResult<AppUser>> UpdateUserAsync(AppUser user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            TaskResult<AppUser> result = new TaskResult<AppUser>
                {StatusCode = HttpStatusCode.NoContent, Succeeded = false};

            try
            {
                //Get token for access to Microsoft Graph as this application
                string accessToken = await GetAzureADTokenAsync(this.azureB2cConfig);
                if (String.IsNullOrEmpty(accessToken))
                    return new TaskResult<AppUser>
                        {StatusCode = HttpStatusCode.Unauthorized, Message = "Failed to get authentication token."};

                UriBuilder builder = new UriBuilder(this.azureB2cConfig.AzureADBaseUrl);
                builder.Path = UriExtensions.CombinePath(this.azureB2cConfig.TenantId,
                    this.azureB2cConfig.ResourcePathUsers, user.Id.ToString());
                builder.Query = $"api-version={this.azureB2cConfig.AzureADApiVersion}";

                JObject updateData = new JObject();
                updateData.Add("displayName", new JValue(user.DisplayName));
                updateData.Add("givenName",
                    new JValue(user.AdditionalData.ContainsKey("givenName") && user.AdditionalData["givenName"] != null
                        ? user.AdditionalData["givenName"].ToString()
                        : string.Empty));
                updateData.Add("surName",
                    new JValue(user.AdditionalData.ContainsKey("surname") && user.AdditionalData["surname"] != null
                        ? user.AdditionalData["surname"].ToString()
                        : string.Empty));
                updateData.Add("accountEnabled", new JValue(true));
                updateData.Add("mobile", new JValue(user.PhoneNumber));
                //if (!String.IsNullOrEmpty(user.Email) && !user.Email.Equals(user.SignInName, StringComparison.InvariantCultureIgnoreCase))
                //{
                //    updateData.Add("otherMails", JToken.FromObject(new string[1] { user.Email }));
                //    updateData.Add("mailNickname", new JValue(user.Email.Split("@".ToCharArray()).FirstOrDefault()));
                //}

                //updateData.Add($"extension_{AzureB2cApplicationId}_UserRole", new JValue(user.UserRole));
                //updateData.Add($"extension_{AzureB2cApplicationId}_UserDepartment", new JValue(user.UserDepartment));

                HttpResponseMessage responseMessage = await SendRequestAsync(new HttpMethod("PATCH"), builder.Uri,
                    accessToken, updateData.ToString());
                result.Succeeded = responseMessage.IsSuccessStatusCode;
                result.StatusCode = responseMessage.StatusCode;
                if (responseMessage.IsSuccessStatusCode)
                {
                    result.Data = user;
                    result.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    result.Message = await responseMessage.Content.ReadAsStringAsync();
                }
            }
            catch (ServiceException graphEx)
            {
                result.Succeeded = false;
                if (graphEx.IsMatch("Request_ResourceNotFound"))
                    result.StatusCode = HttpStatusCode.NotFound;
                else
                    result.StatusCode = graphEx.StatusCode;

                result.Message = graphEx.Error?.Message ?? graphEx.Message;
                result.Error = graphEx;
            }
            catch (Exception ex)
            {
                result.StatusCode = HttpStatusCode.InternalServerError;
                result.Message = ex.Message;
                result.Error = ex;
            }

            return result;
        }

        /// <summary>
        /// Changes the password, if minimal length is 8 characters, for the given user id.
        /// </summary>
        /// <param name="userId">The users object id.</param>
        /// <param name="password">The new password for the user, with a minimal length of 8.</param>
        /// <returns>Succeeded as true and a StatusCode of NoContent.</returns>
        public async Task<TaskResult> UpdatePasswordAsync(Guid userId, string userToken, string oldPassword,
            string newPassword)
        {
            if (userId == Guid.Empty)
                throw new ArgumentNullException("userId");
            if (String.IsNullOrEmpty(oldPassword) || oldPassword.Length < 4)
                return new TaskResult
                {
                    Succeeded = false, StatusCode = HttpStatusCode.BadRequest,
                    Message = "The old password does not meet the minimal length of 4 charachters."
                };
            if (String.IsNullOrEmpty(newPassword) || newPassword.Length < 8)
                return new TaskResult
                {
                    Succeeded = false, StatusCode = HttpStatusCode.BadRequest,
                    Message = "The old password does not meet the minimal length of 8 charachters."
                };

            TaskResult result = new TaskResult {StatusCode = HttpStatusCode.NoContent, Succeeded = false};

            try
            {
                //Get Graph API Client for access to Microsoft Graph API v2.0 as this application
                GraphServiceClient graphClient = new GraphServiceClient(
                    this.azureB2cConfig.GraphApiBaseUrl,
                    new DelegateAuthenticationProvider(async (requestMessage) =>
                    {
                        AuthenticationResult authResult =
                            await this.GetOnBehalfOfUserTokenAsync(this.azureB2cConfig, userToken);
                        if (authResult != null)
                            requestMessage.Headers.Authorization =
                                new AuthenticationHeaderValue("bearer", authResult.AccessToken);
                    }));

                await graphClient.Users[userId.ToString()].ChangePassword(oldPassword, newPassword).Request()
                    .PostAsync();
                result.Succeeded = true;
                result.StatusCode = HttpStatusCode.OK;

                //Get token for access to Microsoft Graph as this application
                //string accessToken = await GetAzureADTokenAsync(this.azureB2cConfig);
                //if (String.IsNullOrEmpty(accessToken))
                //    return new TaskResult<AppUser> { StatusCode = HttpStatusCode.Unauthorized, Message = "Failed to get authentication token." };

                //UriBuilder builder = new UriBuilder(this.azureB2cConfig.AzureADBaseUrl);
                //builder.Path = Common.Extensions.UriExtensions.CombinePath(this.azureB2cConfig.TenantId, this.azureB2cConfig.ResourcePathUsers, $"{userId}/changePassword");
                //builder.Query = $"api-version={this.azureB2cConfig.AzureADApiVersion}";

                //JObject updateData = new JObject();
                //updateData.Add("currentPassword", new JValue(oldPassword));
                //updateData.Add("newPassword", new JValue(newPassword));

                //HttpResponseMessage responseMessage = await SendRequestAsync(HttpMethod.Post, builder.Uri, accessToken, updateData.ToString());
                //result.Succeeded = responseMessage.IsSuccessStatusCode;
                //result.StatusCode = responseMessage.StatusCode;
                //if (responseMessage.IsSuccessStatusCode)
                //    result.StatusCode = HttpStatusCode.NoContent;
                //else
                //    result.Message = await responseMessage.Content.ReadAsStringAsync();
            }
            catch (ServiceException graphEx)
            {
                result.Succeeded = false;
                if (graphEx.IsMatch("Request_ResourceNotFound"))
                    result.StatusCode = HttpStatusCode.NotFound;
                else
                    result.StatusCode = graphEx.StatusCode;

                result.Message = graphEx.Error?.Message ?? graphEx.Message;
                result.Error = graphEx;
            }
            catch (Exception ex)
            {
                result.StatusCode = HttpStatusCode.InternalServerError;
                result.Message = ex.Message;
                result.Error = ex;
            }

            return result;
        }

        public async Task<TaskResult> DeleteUserAsync(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentNullException("userId");

            TaskResult result = new TaskResult {StatusCode = HttpStatusCode.NoContent, Succeeded = false};

            try
            {
                //Get Graph API Client for access to Microsoft Graph API v2.0 as this application
                GraphServiceClient graphClient = new GraphServiceClient(
                    this.azureB2cConfig.GraphApiBaseUrl,
                    new DelegateAuthenticationProvider(async (requestMessage) =>
                    {
                        AuthenticationResult authResult = await GetB2CTokenAsync(this.azureB2cConfig);
                        if (authResult != null)
                            requestMessage.Headers.Authorization =
                                new AuthenticationHeaderValue("bearer", authResult.AccessToken);
                    }));

                await graphClient.Users[userId.ToString()].Request().DeleteAsync();
                result.Succeeded = true;
                result.StatusCode = HttpStatusCode.OK;
            }
            catch (ServiceException graphEx)
            {
                result.Succeeded = false;
                if (graphEx.IsMatch("Request_ResourceNotFound"))
                    result.StatusCode = HttpStatusCode.NotFound;
                else
                    result.StatusCode = graphEx.StatusCode;

                result.Message = graphEx.Error?.Message ?? graphEx.Message;
                result.Error = graphEx;
            }
            catch (Exception ex)
            {
                result.Succeeded = false;
                result.StatusCode = HttpStatusCode.InternalServerError;
                result.Message = ex.Message;
                result.Error = ex;
            }

            return result;
        }

        /// <summary>
        /// Token for use with 'https://graph.windows.net', Azure AD v1.0 endpoint.
        /// </summary>
        /// <param name="azureAuthConfig"></param>
        /// <returns>An access token.</returns>
        public async Task<string> GetAzureADTokenAsync(AzureAuthenticationConfig azureB2cConfig)
        {
            if (azureB2cConfig == null)
                throw new ArgumentNullException("azureB2cConfig");

            string token = null;
            Dictionary<string, string> loginValues = new Dictionary<string, string>
            {
                {"grant_type", "client_credentials"},
                {"client_id", azureB2cConfig.ClientId},
                {"client_secret", azureB2cConfig.ClientSecret},
                {"scope", azureB2cConfig.GraphApiScopes}
            };

            string azureADUrl = String.Format(azureB2cConfig.AzureADTokenUrl, azureB2cConfig.TenantId);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, azureADUrl)
            {
                Content = new FormUrlEncodedContent(loginValues)
            };

            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage response = await httpClient.SendAsync(request);
                string jsonContent = await response.Content.ReadAsStringAsync();
                JObject dataObject = JObject.Parse(jsonContent);
                if (response.IsSuccessStatusCode)
                {
                    token = dataObject["access_token"]?.ToString();
                }
                else
                {
                    throw new HttpRequestException($"Failed to get access token: {dataObject["error_description"]}");
                }
            }

            return token;
        }

        #region Private Methods

        /// <summary>
        /// Token for use with 'https://graph.microsoft.com/v1.0/', Azure AD v2.0 endpoint.
        /// </summary>
        /// <param name="azureB2cConfig"></param>
        /// <returns></returns>
        private async Task<AuthenticationResult> GetB2CTokenAsync(AzureAuthenticationConfig azureB2cConfig)
        {
            if (azureB2cConfig == null)
                throw new ArgumentNullException("azureB2cConfig");

            string authority = $"https://login.microsoftonline.com/{azureB2cConfig.TenantId}/oauth2/v2.0/token";
            string[] scopes = {"https://graph.microsoft.com/.default"};

            IPublicClientApplication app;
            app = PublicClientApplicationBuilder.Create(azureB2cConfig.ClientId)
                .WithB2CAuthority(authority)
                .Build();
            return await app.AcquireTokenInteractive(scopes).ExecuteAsync();
        }

        /// <summary>
        /// On behalf of token for use with 'https://graph.microsoft.com/v1.0/', Azure AD v2.0 endpoint.
        /// </summary>
        /// <param name="azureB2cConfig"></param>
        /// <param name="userToken"></param>
        /// <returns></returns>
        private async Task<AuthenticationResult> GetOnBehalfOfUserTokenAsync(AzureAuthenticationConfig azureB2cConfig,
            string userToken)
        {
            if (azureB2cConfig == null)
                throw new ArgumentNullException("azureB2cConfig");

            string authority = $"https://login.microsoftonline.com/{azureB2cConfig.TenantId}/oauth2/v2.0/token";
            string[] scopes = {"https://graph.microsoft.com/.default"};

            IConfidentialClientApplication app;
            app = ConfidentialClientApplicationBuilder.Create(azureB2cConfig.ClientId)
                .WithClientSecret(azureB2cConfig.ClientSecret)
                .WithB2CAuthority(authority)
                .Build();

            return await app.AcquireTokenOnBehalfOf(scopes, new UserAssertion(userToken)).ExecuteAsync();
        }

        private async Task<HttpResponseMessage> SendRequestAsync(HttpMethod method, Uri apiUrl, string accessToken,
            string data)
        {
            HttpResponseMessage response = null;

            if (String.IsNullOrEmpty(accessToken))
                throw new ArgumentNullException("accessToken");

            using (HttpClient httpClient = new HttpClient())
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage(method, apiUrl);
                // Pass the Bearer token as part of request headers.
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (!method.Equals(HttpMethod.Get) && !method.Equals(HttpMethod.Delete))
                    requestMessage.Content = new StringContent(data, Encoding.UTF8, "application/json");
                response = await httpClient.SendAsync(requestMessage);
            }

            return response;
        }

        private (List<AppUser> users, int offset) GetPartToAddFromBlock(List<AppUser> users, int itemCount,
            string search, int offset, int pageSize)
        {
            if (users == null)
                return ValueTuple.Create<List<AppUser>, int>(new List<AppUser>(), offset);

            List<AppUser> userList = new List<AppUser>();
            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToUpper();
                users = users.Where(x =>
                    x.DisplayName.ToUpper().Contains(search) ||
                    (x.AdditionalData.ContainsKey("givenName") &&
                     (x.AdditionalData["givenName"]?.ToString().ToUpper().Contains(search) ?? false)) ||
                    (x.AdditionalData.ContainsKey("surname") &&
                     (x.AdditionalData["surname"]?.ToString().ToUpper().Contains(search) ?? false))).ToList();
            }

            if (offset < users.Count)
            {
                if ((itemCount + users.Count) > (offset + pageSize))
                    userList = users.Skip(offset).Take(pageSize - itemCount).ToList();
                else
                    userList = users.Skip(offset).ToList();
            }

            offset -= users.Count;
            if (offset < 0)
                offset = 0;

            return ValueTuple.Create<List<AppUser>, int>(userList, offset);
        }

        #endregion

        public class GraphUserData
        {
            [JsonProperty("odata.metadata")] public string odataMetadata { get; set; }

            [JsonProperty("odata.nextLink")] public string odataNextLink { get; set; }

            [JsonProperty("value")] public List<AppUser> Value { get; set; }
        }


        private GraphServiceClient GetGraphServiceClient(AzureAuthenticationConfig azureB2cConfig)
        {
            if (graphServiceClient == null)
            {
                graphServiceClient = this.CreateGraphServiceClient(azureB2cConfig);
                graphServiceClientTimestamp = DateTime.UtcNow;
            }
            else if (DateTime.UtcNow.Subtract(graphServiceClientTimestamp).TotalMinutes > 30)
            {
                graphServiceClient = this.CreateGraphServiceClient(azureB2cConfig);
                graphServiceClientTimestamp = DateTime.UtcNow;
            }

            return graphServiceClient;
        }


        /// <summary>
        /// Builds a GraphServiceClient object with bearer token added as Authorization header.
        /// </summary>
        /// <param name="identityManagerConfig"></param>
        /// <returns>GraphServiceClient object.</returns>
        private GraphServiceClient CreateGraphServiceClient(AzureAuthenticationConfig azureB2cConfig)
        {
            IConfidentialClientApplication confidentialClientApplication = ConfidentialClientApplicationBuilder
                .Create(this.azureB2cConfig.ClientId)
                .WithTenantId(this.azureB2cConfig.TenantId)
                .WithClientSecret(this.azureB2cConfig.ClientSecret)
                .Build();

            string[] scopes =
                this.azureB2cConfig.GraphApiScopes.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            GraphServiceClient graphService = new GraphServiceClient(new DelegateAuthenticationProvider(
                async (requestMessage) =>
                {
                    // Retrieve an access token for Microsoft Graph (gets a fresh token if needed).
                    AuthenticationResult authResult =
                        await confidentialClientApplication.AcquireTokenForClient(scopes).ExecuteAsync();

                    // Add the access token in the Authorization header of the API request.
                    requestMessage.Headers.Authorization =
                        new AuthenticationHeaderValue("Bearer", authResult.AccessToken);
                }));

            return graphService;
        }
    }
}