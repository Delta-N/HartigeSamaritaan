import {
	InteractionType,
	PopupRequest,
	RedirectRequest,
} from '@azure/msal-browser';

export type MsalInterceptorConfig = {
	interactionType: InteractionType.Popup | InteractionType.Redirect;
	protectedResourceMap: Map<string, Array<string>>;
	authRequest?: PopupRequest | RedirectRequest;
};
