import { Inject, Injectable } from '@angular/core';
import {
	AccountInfo,
	AuthenticationResult,
	AuthorizationUrlRequest,
	EndSessionRequest,
	IPublicClientApplication,
	PopupRequest,
	RedirectRequest,
	SilentRequest,
} from '@azure/msal-browser';
import { MSAL_INSTANCE } from './constants';
import { from, Observable } from 'rxjs';
import { Location } from '@angular/common';

interface IMsalService {
	acquireTokenPopup(request: PopupRequest): Observable<AuthenticationResult>;

	acquireTokenRedirect(request: RedirectRequest): Observable<void>;

	acquireTokenSilent(
		silentRequest: SilentRequest
	): Observable<AuthenticationResult>;

	getAccountByUsername(userName: string): AccountInfo | null;

	getAllAccounts(): AccountInfo[];

	handleRedirectObservable(): Observable<AuthenticationResult | null>;

	loginPopup(request?: PopupRequest): Observable<AuthenticationResult>;

	loginRedirect(request?: RedirectRequest): Observable<void>;

	logout(logoutRequest?: EndSessionRequest): Observable<void>;

	ssoSilent(request: AuthorizationUrlRequest): Observable<AuthenticationResult>;
}

@Injectable()
export class MsalService implements IMsalService {
	private redirectHash: string;

	constructor(
		@Inject(MSAL_INSTANCE) private msalInstance: IPublicClientApplication,
		private location: Location
	) {
		// Cache the code hash before Angular router clears it
		const hash = this.location.path(true).split('#').pop();
		if (hash) {
			this.redirectHash = `#${hash}`;
		}
	}

	acquireTokenPopup(request: PopupRequest): Observable<AuthenticationResult> {
		return from(this.msalInstance.acquireTokenPopup(request));
	}

	acquireTokenRedirect(request: RedirectRequest): Observable<void> {
		return from(this.msalInstance.acquireTokenRedirect(request));
	}

	acquireTokenSilent(
		silentRequest: SilentRequest
	): Observable<AuthenticationResult> {
		return from(this.msalInstance.acquireTokenSilent(silentRequest));
	}

	getAccountByUsername(userName: string): AccountInfo {
		return this.msalInstance.getAccountByUsername(userName);
	}

	getAllAccounts(): AccountInfo[] {
		return this.msalInstance.getAllAccounts();
	}

	handleRedirectObservable(): Observable<AuthenticationResult> {
		return from(this.msalInstance.handleRedirectPromise(this.redirectHash));
	}

	loginPopup(request?: PopupRequest): Observable<AuthenticationResult> {
		return from(this.msalInstance.loginPopup(request));
	}

	loginRedirect(request?: RedirectRequest): Observable<void> {
		return from(this.msalInstance.loginRedirect(request));
	}

	logout(logoutRequest?: EndSessionRequest): Observable<void> {
		return from(this.msalInstance.logout(logoutRequest));
	}

	ssoSilent(request: SilentRequest): Observable<AuthenticationResult> {
		return from(this.msalInstance.ssoSilent(request));
	}
}
