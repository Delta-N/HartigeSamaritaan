using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using RoosterPlanner.Common.Config;
using RoosterPlanner.Data.Common;
using RoosterPlanner.Models.FilterModels;
using RoosterPlanner.Service.DataModels;


namespace RoosterPlanner.Service
{
    public interface IAzureB2CService
    {
        Task<User> GetUserAsync(Guid userId);

        Task<TaskResult<IEnumerable<User>>> GetAllUsersAsync(PersonFilter filter);

        Task<TaskResult<User>> UpdateUserAsync(User user);
    }

    public class AzureB2CService : IAzureB2CService
    {
        #region Fields

        private readonly AzureAuthenticationConfig azureB2CConfig;
        private GraphServiceClient graphServiceClient;
        private DateTime graphServiceClientTimestamp;

        private const string GraphSelectList =
            "id,identities,accountEnabled,creationType,createdDateTime,displayName,givenName,surname,mail,otherMails,mailNickname,userPrincipalName,mobilePhone,usageLocation,userType,streetAddress,postalCode,city,country,preferredLanguage,refreshTokensValidFromDateTime,extensions,JobTitle,BusinessPhones,Department,OfficeLocation, DeletedDateTime,AdditionalData";

        #endregion

        //Constructor
        public AzureB2CService(IOptions<AzureAuthenticationConfig> azureB2CConfig, IUnitOfWork unitOfWork,
            ILogger<AzureB2CService> logger)
        {
            this.azureB2CConfig = azureB2CConfig.Value;
        }

        public async Task<User> GetUserAsync(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentNullException(nameof(userId));

            var graphService = GetGraphServiceClient();
            var userRole = $"extension_{azureB2CConfig.B2CExtentionApplicationId.Replace("-", "")}_UserRole";
            var dateOfBirth = $"extension_{azureB2CConfig.B2CExtentionApplicationId.Replace("-", "")}_DateOfBirth";
            var phoneNumber = $"extension_{azureB2CConfig.B2CExtentionApplicationId.Replace("-", "")}_PhoneNumber";
            var nationality = $"extension_{azureB2CConfig.B2CExtentionApplicationId.Replace("-", "")}_Nationality";
            var nativeLanguage = $"extension_{azureB2CConfig.B2CExtentionApplicationId.Replace("-", "")}_NativeLanguage";
            var dutchProficiency = $"extension_{azureB2CConfig.B2CExtentionApplicationId.Replace("-", "")}_DutchProficiency";
            var termsofuseconsented = $"extension_{azureB2CConfig.B2CExtentionApplicationId.Replace("-", "")}_TermsOfUseConsented";

            var user = await graphService.Users[userId.ToString()].Request()
                .Select($"{GraphSelectList},{userRole},{dateOfBirth},{phoneNumber},{nationality},{termsofuseconsented},{nativeLanguage},{dutchProficiency}").GetAsync();
            
            return user;
        }

        public async Task<TaskResult<IEnumerable<User>>> GetAllUsersAsync(PersonFilter filter)
        {
            var result = new TaskResult<IEnumerable<User>>
                {StatusCode = HttpStatusCode.NoContent, Succeeded = false, Data = new List<User>()};

            try
            {
                if (string.IsNullOrEmpty(azureB2CConfig.B2CExtentionApplicationId))
                    throw new ArgumentException("B2CExtensionApplicationId is null");

                var graphService = GetGraphServiceClient();
                var userRole = $"extension_{azureB2CConfig.B2CExtentionApplicationId.Replace("-", "")}_UserRole";
                var dateOfBirth = $"extension_{azureB2CConfig.B2CExtentionApplicationId.Replace("-", "")}_DateOfBirth";
                var phoneNumber = $"extension_{azureB2CConfig.B2CExtentionApplicationId.Replace("-", "")}_PhoneNumber";
                var nationality = $"extension_{azureB2CConfig.B2CExtentionApplicationId.Replace("-", "")}_Nationality";
                var nativeLanguage =
                    $"extension_{azureB2CConfig.B2CExtentionApplicationId.Replace("-", "")}_NativeLanguage";
                var dutchProficiency =
                    $"extension_{azureB2CConfig.B2CExtentionApplicationId.Replace("-", "")}_DutchProficiency";
                var termsofuseconsented =
                    $"extension_{azureB2CConfig.B2CExtentionApplicationId.Replace("-", "")}_TermsOfUseConsented";
                var tenant = azureB2CConfig.AzureTenantName;

                var filterString = "";

                if (!string.IsNullOrEmpty(filter.Email))
                {
                    filterString +=
                        $" or identities/any(c:c/issuerAssignedId eq '{filter.Email}' and c/issuer eq '{tenant}')";
                }
                else
                {
                    if (!string.IsNullOrEmpty(filter.FirstName))
                        filterString +=
                            $" or startswith(displayName, '{filter.FirstName}') or startswith(givenName,'{filter.FirstName}')";

                    if (!string.IsNullOrEmpty(filter.LastName))
                        filterString +=
                            $" or startswith(surname, '{filter.LastName}')";

                    if (!string.IsNullOrEmpty(filter.UserRole))
                        filterString +=
                            $" or {userRole} eq {filter.UserRole}";

                    if (!string.IsNullOrEmpty(filter.City))
                        filterString +=
                            $" or startswith(city, '{filter.City}')";
                }

                if (filterString.StartsWith(" or ")) filterString = filterString.Substring(4);
                var users = new List<User>();
                var currentUsers = await graphService.Users
                    .Request()
                    .Filter(filterString)
                    .Select($"{GraphSelectList},{userRole},{dateOfBirth},{phoneNumber},{nationality},{termsofuseconsented},{nativeLanguage},{dutchProficiency}")
                    .GetAsync();
                while (currentUsers.Count > 0)
                {
                    users.AddRange(currentUsers);
                    if (currentUsers.NextPageRequest != null)
                        currentUsers = await currentUsers.NextPageRequest.GetAsync();
                    else
                        break;
                }

                filter.TotalItemCount = users.Count;

                if (users.Count == 0)
                    throw new NullReferenceException("No users found");
     
                result.StatusCode = HttpStatusCode.OK;
                result.Succeeded = true;
                result.Data = users.OrderBy(u => u.DisplayName).ToList();
                return result;
            }

            catch (ServiceException graphEx)
            {
                result.StatusCode = graphEx.StatusCode;
                result.Message = graphEx.Message;
                throw;
            }
            catch (Exception ex)
            {
                result.StatusCode = HttpStatusCode.InternalServerError;
                result.Message = ex.Message;
                throw;
            }
        }

        public async Task<TaskResult<User>> UpdateUserAsync(User user)
        {
            if (user?.Id == null || Guid.Parse(user.Id) == Guid.Empty)
                throw new ArgumentNullException(nameof(user));

            var updatedUser = new TaskResult<User>();
            try
            {
                var graphService = GetGraphServiceClient();
                updatedUser.Data = await graphService.Users[user.Id].Request().UpdateAsync(user);
                if (updatedUser.Data == null)
                {
                    //UpdateAsync has a bug so manully get user
                    updatedUser.Data = GetUserAsync(Guid.Parse(user.Id)).Result;
                    updatedUser.Succeeded = true;
                }
            }
            catch (ServiceException)
            {
                updatedUser.Succeeded = false;
                updatedUser.Message = "Error during patching of user";
                throw;
            }
            catch (Exception)
            {
                updatedUser.Succeeded = false;
                updatedUser.Message = "Error during patching of user";
                throw;
            }
            
            return updatedUser;
        }

        private GraphServiceClient GetGraphServiceClient()
        {
            if (graphServiceClient == null)
            {
                graphServiceClient = CreateGraphServiceClient();
                graphServiceClientTimestamp = DateTime.UtcNow;
            }
            else if (DateTime.UtcNow.Subtract(graphServiceClientTimestamp).TotalMinutes > 30)
            {
                graphServiceClient = CreateGraphServiceClient();
                graphServiceClientTimestamp = DateTime.UtcNow;
            }

            return graphServiceClient;
        }

        /// <summary>
        ///     Builds a GraphServiceClient object with bearer token added as Authorization header.
        /// </summary>
        /// <returns>GraphServiceClient object.</returns>
        private GraphServiceClient CreateGraphServiceClient()
        {
            var confidentialClientApplication = ConfidentialClientApplicationBuilder
                .Create(azureB2CConfig.ClientId)
                .WithTenantId(azureB2CConfig.TenantId)
                .WithClientSecret(azureB2CConfig.ClientSecret)
                .Build();

            var scopes =
                azureB2CConfig.GraphApiScopes.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            var graphService = new GraphServiceClient(new DelegateAuthenticationProvider(
                async requestMessage =>
                {
                    // Retrieve an access token for Microsoft Graph (gets a fresh token if needed).
                    var authResult =
                        await confidentialClientApplication.AcquireTokenForClient(scopes).ExecuteAsync();

                    // Add the access token in the Authorization header of the API request.
                    requestMessage.Headers.Authorization =
                        new AuthenticationHeaderValue("Bearer", authResult.AccessToken);
                }));

            return graphService;
        }
        
        public class GraphUserData
        {
            [JsonProperty("odata.metadata")] public string OdataMetadata { get; set; }

            [JsonProperty("odata.nextLink")] public string OdataNextLink { get; set; }

            [JsonProperty("value")] public List<User> Value { get; set; }
        }
    }
}