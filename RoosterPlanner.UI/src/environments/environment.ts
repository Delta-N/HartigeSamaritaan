// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.


export const environment = {
  production: false,

  auth: {
    clientId: "71002406-7b5a-4f76-bcd7-101bcc156941",
    authority: "https://DeltanHackaton.b2clogin.com/DeltanHackaton.onmicrosoft.com/b2c_1_susi",
    redirectUri: "http://localhost:4200/",
    postLogoutRedirectUri: "http://localhost:4200/",
    navigateToLoginRequestUrl: true,
    validateAuthority: false,
    knownAuthorities: ["https://DeltanHackaton.b2clogin.com"],
  },
  protectedResourceMap: [
    ['https://localhost:5001/api/', ['https://DeltanHackaton.onmicrosoft.com/0f68eccd-b4a7-4747-b90e-ff88685173a3/Read']],
    ['https://roosterplanner-api-dev.azurewebsites.net/api/', ['https://DeltanHackaton.onmicrosoft.com/0f68eccd-b4a7-4747-b90e-ff88685173a3/Read']]
  ],
  scopes: [
    'https://DeltanHackaton.onmicrosoft.com/0f68eccd-b4a7-4747-b90e-ff88685173a3/Read'
  ],
  authorities: {
    signUpSignIn: {
      authority: "https://DeltanHackaton.b2clogin.com/DeltanHackaton.onmicrosoft.com/b2c_1_susi"
    },
    resetPassword: {
      authority: "https://DeltanHackaton.b2clogin.com/DeltanHackaton.onmicrosoft.com/b2c_1_reset_pwd"
    },
    editProfile: {
      authority: "https://DeltanHackaton.b2clogin.com/DeltanHackaton.onmicrosoft.com/b2c_1_edit"
    }
  },
  backendUrl: "https://localhost:5001/",

  appInsights:{
    instrumentationKey: 'a1c408ad-5c7c-4485-b037-f78fea63e71b'
  },

};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
