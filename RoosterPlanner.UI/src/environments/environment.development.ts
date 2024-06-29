// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
	production: true,

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
			'https://localhost:5001/api/',
			[
				'https://roosterplannerdev.onmicrosoft.com/24e46ae1-6114-444b-87cc-788d5380f819/Read',
			],
		],
		[
			'https://roosterplanner-api-dev.azurewebsites.net/api/',
			[
				'https://roosterplannerdev.onmicrosoft.com/24e46ae1-6114-444b-87cc-788d5380f819/Read',
			],
		],
		[
			'https://roosterplanner-api-prd.azurewebsites.net/api/',
			[
				'https://roosterplannerdev.onmicrosoft.com/24e46ae1-6114-444b-87cc-788d5380f819/Read',
			],
		],
		[
			'https://roosterplanner-api-tst.azurewebsites.net/api/',
			[
				'https://roosterplannerdev.onmicrosoft.com/24e46ae1-6114-444b-87cc-788d5380f819/Read',
			],
		],
		[
			'https://rooster-api.hartigesamaritaan.nl/api/',
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
	backendUrl: 'https://localhost:5001/',

	appInsights: {
		instrumentationKey: '9001510e-6341-4bf0-b9c4-dbed80189dd8', //Dit is afhankelijk van de deploy omgeving
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
