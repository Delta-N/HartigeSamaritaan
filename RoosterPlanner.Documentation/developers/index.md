# New deployment enviroment

The following steps currently need to be be done manually when setting up a new deployment environment:

## Domain

Add a custom domain name in Azure web-appservice.

## SSL-certificate

Connect the Azure Extension 'Azure Let's Encrypt' to the Azure web-appservice.

## Create a B2C environment, register this to the resourcegroup and extract necessary data into Key Vault.

The B2C environment contains:

- Two app-registrations (WEB & API),
- Custom user attributes:
    - DateOfBirth : string,
    - DutchProficiency : string,
    - Nationality : string,
    - NativeLanguage : string,
    - PhoneNumber : string,
    - TermsOfUseConsented : string,
    - UserRole : int
- 3 user flows:
    - b2c_1_susi
    - b2c_1_reset
    - b2c_1_edit
- Custom company branding (logo and banner)

The WEB-appregistration is dedicated to an SPA, contains the necessary redirect URI's (custom domain + localhost:4200),
provides **Access tokens**  (used for implicit flows) and does allow *public client flows*.

The API-appregistration does not have any redirect URI's, provides **ID tokens** (used for implicit and hybrid flows) but
does NOT allow *public client flows*.   
This registration also has a secret registered which need be added to the key
vault (AzureAuthentication--ClientSecret).

## Add secrets to key vault

The following secrets need to be added manually to the key vault:

Secret name | Location
--- | --- 
| AzureAuthentication--AzureTenantName  | *Name of b2c + onmicrosoft.com*
| AzureAuthentication--B2CExtentionApplicationId  | *B2C=>app registrations=> all => b2c-extentions-app: client id*
| AzureAuthentication--ClientId | *B2C=>app registrations=>all => API app: client id*
| AzureAuthentication--ClientSecret |*B2C=>app registrations=> all => API app: client secret: GraphAPISecret*
| AzureAuthentication--GraphApiScopes |*https://graph.microsoft.com/.default*
| AzureAuthentication--Instance |*https://(nameofb2c).b2clogin.com*
| AzureAuthentication--SignUpSignInPolicyId| *b2c_1_susi*
| AzureAuthentication--TenantId| *B2C=>app registrations => any app=> overview=> tenentID*
| Email--Emailadres| *Email address that is used to send emails*
| Email--Password| *The password for the email address*
| Email--Port| *The port that is used to send emails* (587)
| Email--SMTPadres| *The SMTP-server address*
| WebUrl--Url| *custom domain URI* 

## Overwriting values in the front-end environment.prod file
Currently, the values environment.prod file are added and changed manually depending in which environment the application is deployed.  
It would be ideal to overwrite these values during the deployment process.  

The following values need to be customized: 

Value name | Location |
--- | ---
clientId|*B2C=>app registrations=> all => WEB app: client id* 
authority | *https://(nameofb2c).b2clogin.com/(nameofb2c).onmicrosoft.com/b2c_1_susi* 
redirectUri| *custom domain URI* 
postLogoutRedirectUri| *custom domain URI*
knownAuthorities| *["https://(nameofb2c).b2clogin.com"]*
protectedResourceMap| *[  ['CUSTOM DOMAIN URI', ['URL TO API EXPOSE PERMISSION']],]*
scopes| *URL TO API-appregistration =>EXPOSE PERMISSION*
signUpSignIn:|   { authority:=> https://(nameofb2c).b2clogin.com/(nameofb2c).onmicrosoft.com/b2c_1_susi }
resetPassword:| { authority: => https://(nameofb2c).b2clogin.com/(nameofb2c).onmicrosoft.com/b2c_1_reset_pwd }
editProfile:| { authority: => https://(nameofb2c).b2clogin.com/(nameofb2c).onmicrosoft.com/b2c_1_edit }


