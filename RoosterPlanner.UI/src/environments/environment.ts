export const environment = {
  production: false,

  auth: {
    clientId: '3cf2da13-f764-40f1-8f24-d6a3f81ff064',
    authority:
      'https://roosterplannerdev.b2clogin.com/roosterplannerdev.onmicrosoft.com/b2c_1_susi',
    redirectUri: 'http://localhost:4200/',
    postLogoutRedirectUri: 'http://localhost:4200/',
    navigateToLoginRequestUrl: true,
    validateAuthority: false,
    knownAuthorities: ['https://roosterplannerdev.b2clogin.com'],
  },
  protectedResourceMap: [
    [
      'https://roosterplanner-api-tst.azurewebsites.net/api/',
      [
        'https://roosterplannerdev.onmicrosoft.com/24e46ae1-6114-444b-87cc-788d5380f819/Read',
      ],
    ],
  ],
  scopes: [
    'https://roosterplannerdev.onmicrosoft.com/24e46ae1-6114-444b-87cc-788d5380f819/Read',
  ],
  authorities: {
    signUpSignIn: {
      authority:
        'https://roosterplannerdev.b2clogin.com/roosterplannerdev.onmicrosoft.com/b2c_1_susi',
    },
    resetPassword: {
      authority:
        'https://roosterplannerdev.b2clogin.com/roosterplannerdev.onmicrosoft.com/b2c_1_reset_pwd',
    },
    editProfile: {
      authority:
        'https://roosterplannerdev.b2clogin.com/roosterplannerdev.onmicrosoft.com/b2c_1_edit',
    },
  },
  backendUrl: 'https://roosterplanner-api-tst.azurewebsites.net/',

  appInsights: {
    instrumentationKey: '9001510e-6341-4bf0-b9c4-dbed80189dd8',
  },
};
