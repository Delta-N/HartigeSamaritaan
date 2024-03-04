import {
	MsalGuardConfiguration,
	MsalInterceptorConfiguration,
} from '@azure/msal-angular';
import {
	InteractionType,
	IPublicClientApplication,
	LogLevel,
	PublicClientApplication,
} from '@azure/msal-browser';
import { environment } from 'src/environments/environment';

const isIE =
	window.navigator.userAgent.indexOf('MSIE ') > -1 ||
	window.navigator.userAgent.indexOf('Trident/') > -1; // Remove this line to use Angular Universal

export function loggerCallback(logLevel: LogLevel, message: string): void {
	console.log(message);
}

export function msalInstanceFactory(): IPublicClientApplication {
	return new PublicClientApplication({
		auth: {
			clientId: environment.msalConfig.auth.clientId,
			authority: environment.msalConfig.auth.authority,
			redirectUri: environment.msalConfig.auth.redirectUri,
			postLogoutRedirectUri: environment.msalConfig.auth.postLogoutRedirectUri,
			knownAuthorities: [environment.msalConfig.auth.authorityDomain],
		},
		cache: {
			cacheLocation: environment.msalConfig.cache
				.cacheLocation,
			storeAuthStateInCookie: isIE,
		},
		system: {
			loggerOptions: {
				loggerCallback,
				logLevel: LogLevel.Warning,
				piiLoggingEnabled: false,
			},
		},
	});
}

export function msalInterceptorConfigFactory(): MsalInterceptorConfiguration {
	const protectedResourceMap = new Map<string, Array<string>>();
  environment.protectedResourceMap.forEach(input => {
    let string1 = input[0] as string
    let string2 = input[1] as string[]
    protectedResourceMap.set(string1, string2)

  });

	return {
		interactionType: InteractionType.Redirect,
		protectedResourceMap,
	};
}

export function msalGuardConfigFactory(): MsalGuardConfiguration {
	return {
		interactionType: InteractionType.Redirect,
		authRequest: {
			scopes: [...environment.msalConfig.consentScopes],
		},
	};
}
