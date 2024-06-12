// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
	production: false,

	auth: {
		clientId: '23cbcba3-683e-4fea-bf57-f25d3dc4f0fc',
		authority:
			'https://roosterplanneridp.b2clogin.com/roosterplanneridp.onmicrosoft.com/b2c_1_susi',
		redirectUri: 'http://localhost:4200/',
		postLogoutRedirectUri: 'http://localhost:4200/',
		navigateToLoginRequestUrl: true,
		validateAuthority: false,
		knownAuthorities: ['https://roosterplanneridp.b2clogin.com'],
	},
	protectedResourceMap: [
		[
			'https://localhost:5001/api/',
			[
				'https://roosterplanneridp.onmicrosoft.com/4edc10b5-3274-4594-8116-ecd6860a2272/Read',
			],
		],
		[
			'https://roosterplanner-api-dev.azurewebsites.net/api/',
			[
				'https://roosterplanneridp.onmicrosoft.com/4edc10b5-3274-4594-8116-ecd6860a2272/Read',
			],
		],
		[
			'https://roosterplanner-api-prd.azurewebsites.net/api/',
			[
				'https://roosterplanneridp.onmicrosoft.com/4edc10b5-3274-4594-8116-ecd6860a2272/Read',
			],
		],
		[
			'https://roosterplanner-api-tst.azurewebsites.net/api/',
			[
				'https://roosterplanneridp.onmicrosoft.com/4edc10b5-3274-4594-8116-ecd6860a2272/Read',
			],
		],
		[
			'https://rooster-api.hartigesamaritaan.nl/api/',
			[
				'https://roosterplanneridp.onmicrosoft.com/4edc10b5-3274-4594-8116-ecd6860a2272/Read',
			],
		],
	],
	scopes: [
		'https://roosterplanneridp.onmicrosoft.com/4edc10b5-3274-4594-8116-ecd6860a2272/Read',
	],
	authorities: {
		signUpSignIn: {
			authority:
				'https://roosterplanneridp.b2clogin.com/roosterplanneridp.onmicrosoft.com/b2c_1_susi',
		},
		resetPassword: {
			authority:
				'https://roosterplanneridp.b2clogin.com/roosterplanneridp.onmicrosoft.com/b2c_1_reset_pwd',
		},
		editProfile: {
			authority:
				'https://roosterplanneridp.b2clogin.com/roosterplanneridp.onmicrosoft.com/b2c_1_edit',
		},
	},
	backendUrl: 'https://localhost:5001/',

	appInsights: {
		instrumentationKey: '54c0fc49-0057-453d-bae6-e384d5f00ce4', //Dit is afhankelijk van de deploy omgeving
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
