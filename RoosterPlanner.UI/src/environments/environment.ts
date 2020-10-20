// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.


export const environment = {
  production: false,

  auth: {
    clientId: "a6ca557c-9d83-4867-a02d-99c5fbd159a5",
    authority: "https://DeltanHackaton.b2clogin.com/DeltanHackaton.onmicrosoft.com/b2c_1_susi",
    redirectUri: "http://localhost:4200/",
    postLogoutRedirectUri: "http://localhost:4200/",
    navigateToLoginRequestUrl: true,
    validateAuthority: false,
  },

  backendUrl: "https://localhost:5001/"
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
