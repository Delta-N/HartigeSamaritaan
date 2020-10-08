import {b2cPolicies} from "../app/app-config";

export const environment = {
  production: true,
  auth: {
    clientId: "a6ca557c-9d83-4867-a02d-99c5fbd159a5",
    authority: b2cPolicies.authorities.signUpSignIn.authority,
    redirectUri: "https://roosterplanner-web-dev.azurewebsites.net",
    postLogoutRedirectUri: "https://roosterplanner-web-dev.azurewebsites.net",
    navigateToLoginRequestUrl: true,
    validateAuthority: false,
  },
};
