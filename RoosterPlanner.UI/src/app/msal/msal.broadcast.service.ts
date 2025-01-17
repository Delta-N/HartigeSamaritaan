import { Inject, Injectable } from '@angular/core';
import { Observable, Subject } from 'rxjs';
import { MSAL_INSTANCE } from './constants';
import { EventMessage, IPublicClientApplication } from '@azure/msal-browser';

@Injectable()
export class MsalBroadcastService {
	msalSubject$: Observable<any>;
	private _msalSubject: Subject<any>;

	constructor(
		@Inject(MSAL_INSTANCE) private msalInstance: IPublicClientApplication
	) {
		this._msalSubject = new Subject<EventMessage>();
		this.msalSubject$ = this._msalSubject.asObservable();
		this.msalInstance.addEventCallback((message: EventMessage) => {
			this._msalSubject.next(message);
		});
	}
}
