export const environment = {
  production: true,
  auth: {
    clientId: "23cbcba3-683e-4fea-bf57-f25d3dc4f0fc",
    authority: "https://roosterplanneridp.b2clogin.com/roosterplanneridp.onmicrosoft.com/b2c_1_susi",
    redirectUri: "https://roosterplanner-web-prd.azurewebsites.net", //aanpassen in prod omgeving naar "https://rooster.hartigesamaritaan.nl"
    postLogoutRedirectUri: "https://roosterplanner-web-prd.azurewebsites.net", //aanpassen in prod omgeving naar "https://rooster.hartigesamaritaan.nl"
    navigateToLoginRequestUrl: true,
    validateAuthority: false,
    knownAuthorities: ["https://roosterplanneridp.b2clogin.com"],
  },
  protectedResourceMap: [
    ['https://localhost:5001/api/', ['https://roosterplanneridp.onmicrosoft.com/4edc10b5-3274-4594-8116-ecd6860a2272/Read']],
    ['https://roosterplanner-api-dev.azurewebsites.net/api/', ['https://roosterplanneridp.onmicrosoft.com/4edc10b5-3274-4594-8116-ecd6860a2272/Read']],
    ['https://roosterplanner-api-prd.azurewebsites.net/api/', ['https://roosterplanneridp.onmicrosoft.com/4edc10b5-3274-4594-8116-ecd6860a2272/Read']],
    ['https://roosterplanner-api-tst.azurewebsites.net/api/', ['https://roosterplanneridp.onmicrosoft.com/4edc10b5-3274-4594-8116-ecd6860a2272/Read']],
    ['https://rooster-api.hartigesamaritaan.nl/api/', ['https://roosterplanneridp.onmicrosoft.com/4edc10b5-3274-4594-8116-ecd6860a2272/Read']],

  ],
  scopes: [
    'https://roosterplanneridp.onmicrosoft.com/4edc10b5-3274-4594-8116-ecd6860a2272/Read'
  ],
  authorities: {
    signUpSignIn: {
      authority: "https://roosterplanneridp.b2clogin.com/roosterplanneridp.onmicrosoft.com/b2c_1_susi"
    },
    resetPassword: {
      authority: "https://roosterplanneridp.b2clogin.com/roosterplanneridp.onmicrosoft.com/b2c_1_reset_pwd"
    },
    editProfile: {
      authority: "https://roosterplanneridp.b2clogin.com/roosterplanneridp.onmicrosoft.com/b2c_1_edit"
    }
  },
  backendUrl: "https://roosterplanner-api-prd.azurewebsites.net/", //Dit is afhankelijk van de deploy omgeving

  appInsights: {
    instrumentationKey: '54c0fc49-0057-453d-bae6-e384d5f00ce4' //Dit is afhankelijk van de deploy omgeving
  },

};
