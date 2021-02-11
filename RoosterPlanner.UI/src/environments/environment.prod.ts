export const environment = {
  production: true,
  auth: {
    clientId: "23cbcba3-683e-4fea-bf57-f25d3dc4f0fc",
    authority: "https://roosterplanneridp.b2clogin.com/roosterplanneridp.onmicrosoft.com/b2c_1_susi",
    redirectUri: "https://roosterplanner-web-dev.azurewebsites.net", //aanpassen in prod omgeving naar "https://rooster.hartigesamaritaan.nl"
    postLogoutRedirectUri: "https://roosterplanner-web-dev.azurewebsites.net", //aanpassen in prod omgeving naar "https://rooster.hartigesamaritaan.nl"
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
  backendUrl: "https://roosterplanner-api-dev.azurewebsites.net/", //Dit is afhankelijk van de deploy omgeving

  appInsights: {
    instrumentationKey: 'd9b2031e-7045-4f6d-ab36-b7141dd18db7' //Dit is afhankelijk van de deploy omgeving
  },

};
