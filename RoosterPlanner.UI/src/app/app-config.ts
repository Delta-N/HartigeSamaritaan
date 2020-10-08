import { Configuration } from 'msal';
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
    cacheLocation: "localStorage",
    storeAuthStateInCookie: isIE, // Set this to "true" to save cache in cookies to address trusted zones limitations in IE
  },}

export const loginRequest: {scopes: string[]} = {
  scopes: [
    'openid',
    'profile',
    'https://deltanhackaton.b2clogin.com/DeltanHackaton.onmicrosoft.com/v2.0/.well-known/openid-configuration?p=B2C_1_edit'],
};

export const msalAngularConfig: MsalAngularConfiguration = {
  popUp: false,
  consentScopes: [
    ...loginRequest.scopes,
    ...tokenRequest.scopes,
  ],
  unprotectedResources: [], // API calls to these coordinates will NOT activate MSALGuard
  protectedResourceMap,     // API calls to these coordinates will activate MSALGuard
  extraQueryParameters: {}
}









