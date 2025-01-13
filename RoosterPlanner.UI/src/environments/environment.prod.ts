const hostName = '#{hostName}#';
const environmentName = '#{environmentName}#';
const feClientId = '#{feClientId}#';
const apiClientId = '#{tenantId}#';
const appInsightsKey = '#{appInsightsKey}#';

export const environment = {
  production: true,
  auth: {
    clientId: `${feClientId}`,
    authority: `https://${hostName}.b2clogin.com/${hostName}.onmicrosoft.com/b2c_1_susi`,
    redirectUri: `https://roosterplanner-${environmentName}-web.azurewebsites.net/`,
    postLogoutRedirectUri: `https://roosterplanner-${environmentName}-web.azurewebsites.net/`,
    navigateToLoginRequestUrl: true,
    validateAuthority: false,
    knownAuthorities: [`https://${hostName}.b2clogin.com`],
  },
  protectedResourceMap: [
    [
      `https://roosterplanner-${environmentName}-api.azurewebsites.net/api/`,
      [`https://${hostName}.onmicrosoft.com/${apiClientId}/Read`],
    ],
  ],
  scopes: [`https://${hostName}.onmicrosoft.com/${apiClientId}/Read`],
  authorities: {
    signUpSignIn: {
      authority: `https://${hostName}.b2clogin.com/${hostName}.onmicrosoft.com/b2c_1_susi`,
    },
    resetPassword: {
      authority: `https://${hostName}.b2clogin.com/${hostName}.onmicrosoft.com/b2c_1_reset_pwd`,
    },
    editProfile: {
      authority: `https://${hostName}.b2clogin.com/${hostName}.onmicrosoft.com/b2c_1_edit`,
    },
  },
  backendUrl: `https://roosterplanner-${environmentName}-api.azurewebsites.net/`,

  appInsights: {
    instrumentationKey: `${appInsightsKey}`,
  },
};
