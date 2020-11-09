export const environment = {
  production: true,
  auth: {
    clientId: "71002406-7b5a-4f76-bcd7-101bcc156941",
    authority: "https://DeltanHackaton.b2clogin.com/DeltanHackaton.onmicrosoft.com/b2c_1_susi",
    redirectUri: "https://roosterplanner-web-dev.azurewebsites.net",
    postLogoutRedirectUri: "https://roosterplanner-web-dev.azurewebsites.net",
    navigateToLoginRequestUrl: true,
    validateAuthority: false,
    knownAuthorities: ["https://DeltanHackaton.b2clogin.com"],
  },
  protectedResourceMap: [
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
  backendUrl: "https://roosterplanner-api-dev.azurewebsites.net/"
};
