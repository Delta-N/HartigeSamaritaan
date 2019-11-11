export const environment = {
  production: true,
  baseUrl: 'https://roosterplan-api-dev.azurewebsites.net/api',
  options: {
    redirectUri: 'https://hackatonstoragedev.z6.web.core.windows.net/',
    protectedResourceMap: [],
    appId: '2eb090db-afb8-4deb-b7b4-03e649e15ca5',
    authority:
      'https://login.microsoftonline.com/tfp/DeltanHackaton.onmicrosoft.com/B2C_1_susi/',
    passwordResetPolicy: 'B2C_1_reset_pwd',
    consentScopes: [
      'openid',
      'https://deltanhackaton.b2clogin.com/DeltanHackaton.onmicrosoft.com/v2.0/.well-known/openid-configuration?p=B2C_1_edit'
    ]
  }
};
