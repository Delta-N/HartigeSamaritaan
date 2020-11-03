import {CacheLocation, Configuration} from 'msal';
import { MsalAngularConfiguration } from '@azure/msal-angular';
import {environment} from "../environments/environment";

export const isIE = window.navigator.userAgent.indexOf('MSIE ') > -1 || window.navigator.userAgent.indexOf('Trident/') > -1;

export const b2cPolicies = {
  names: {
    signUpSignIn: "b2c_1_susi",
    resetPassword: "b2c_1_reset",
    editProfile: "b2c_1_edit_profile"
  },
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
  }
}

// # api region
export const apiConfig: {b2cScopes: string[], webApi: string} = {
  b2cScopes: [''],
  webApi: ''
};

export const tokenRequest: {scopes: string[]} = {
  scopes: apiConfig.b2cScopes
};

export const protectedResourceMap: [string, string[]][] = [
  [apiConfig.webApi, apiConfig.b2cScopes]
];
//

export const msalConfig: Configuration = {
  auth: environment.auth,
  cache: {
    cacheLocation: 'sessionStorage' as CacheLocation,
    storeAuthStateInCookie: isIE, // Set this to "true" to save cache in cookies to address trusted zones limitations in IE
  },}

export const loginRequest: {scopes: string[]} = {
  scopes: [
    'openid',
    'profile',

    'offline_access',
    'https://DeltanHackaton.onmicrosoft.com/0f68eccd-b4a7-4747-b90e-ff88685173a3/Read'
    ],
};

export const msalAngularConfig: MsalAngularConfiguration = {
  popUp: false,
  consentScopes: [
    ...loginRequest.scopes,
    ...tokenRequest.scopes,
  ],
  unprotectedResources: [], // API calls to these coordinates will NOT activate MSALGuard
  protectedResourceMap: [
    ['https://localhost:5001/api/', ['https://DeltanHackaton.onmicrosoft.com/0f68eccd-b4a7-4747-b90e-ff88685173a3/Read']],
    ['https://roosterplanner-api-dev.azurewebsites.net/api/', ['https://DeltanHackaton.onmicrosoft.com/0f68eccd-b4a7-4747-b90e-ff88685173a3/Read']]
  ],     // API calls to these coordinates will activate MSALGuard
  extraQueryParameters: {}
}









