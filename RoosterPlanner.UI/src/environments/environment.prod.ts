export const environment = {
  production: true,

  protectedResourceMap: [
    ['https://localhost:5001/api/', ['https://roosterplanneridp.onmicrosoft.com/4edc10b5-3274-4594-8116-ecd6860a2272/Read']],
    ['https://roosterplanner-api-dev.azurewebsites.net/api/', ['https://roosterplanneridp.onmicrosoft.com/4edc10b5-3274-4594-8116-ecd6860a2272/Read']],
    ['https://roosterplanner-api-prd.azurewebsites.net/api/', ['https://roosterplanneridp.onmicrosoft.com/4edc10b5-3274-4594-8116-ecd6860a2272/Read']],
    ['https://roosterplanner-api-tst.azurewebsites.net/api/', ['https://roosterplanneridp.onmicrosoft.com/4edc10b5-3274-4594-8116-ecd6860a2272/Read']],
    ['https://rooster-api.hartigesamaritaan.nl/api/', ['https://roosterplanneridp.onmicrosoft.com/4edc10b5-3274-4594-8116-ecd6860a2272/Read']],

  ],

  b2cPolicies: {
    names: {
      signUpSignIn: "b2c_1_susi",
      resetPassword: "b2c_1_reset_pwd",
      editProfile: "b2c_1_edit"
    },
    authorities: {
      signUpSignIn: {
        authority: 'https://roosterplanneridp.b2clogin.com/roosterplanneridp.onmicrosoft.com/b2c_1_susi'
      },
      resetPassword: {
        authority: 'https://roosterplanneridp.b2clogin.com/roosterplanneridp.onmicrosoft.com/b2c_1_reset_pwd'
      },
      editProfile: {
        authority: "https://roosterplanneridp.b2clogin.com/roosterplanneridp.onmicrosoft.com/b2c_1_edit"
      }
    },
    authorityDomain: "fabrikamb2c.b2clogin.com"
  },
  apiUrl: "https://roosterplanner-api-prd.azurewebsites.net/", //Dit is afhankelijk van de deploy omgeving
  msalConfig: {
    auth: {
      clientId: '23cbcba3-683e-4fea-bf57-f25d3dc4f0fc',
      authority:
        'https://roosterplanneridp.b2clogin.com/roosterplanneridp.onmicrosoft.com/b2c_1_susi',
      validateAuthority: false,
      navigateToLoginRequestUrl: false,
      redirectUri: 'https://rooster.hartigesamaritaan.nl',
      postLogoutRedirectUri: 'https://rooster.hartigesamaritaan.nl',
      authorityDomain: 'https://roosterplanneridp.b2clogin.com',
    },
    cache: {
      cacheLocation: 'sessionStorage',
    },
    consentScopes: [
      'https://roosterplanneridp.onmicrosoft.com/4edc10b5-3274-4594-8116-ecd6860a2272/Read',
    ],
  },
  appInsights: {
    instrumentationKey: '54c0fc49-0057-453d-bae6-e384d5f00ce4' //Dit is afhankelijk van de deploy omgeving
  }


}

