import {
	ApplicationConfig,
	ErrorHandler,
	importProvidersFrom,
} from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { HTTP_INTERCEPTORS, provideHttpClient, withFetch, withInterceptorsFromDi } from '@angular/common/http';
import {
	MSAL_GUARD_CONFIG,
	MSAL_INSTANCE,
	MSAL_INTERCEPTOR_CONFIG,
	MsalBroadcastService,
	MsalGuard,
	MsalInterceptor,
	MsalModule,
	MsalService,
} from '@azure/msal-angular';
import {
	msalGuardConfigFactory,
	msalInstanceFactory,
	msalInterceptorConfigFactory,
} from './authentication/msal';
import { provideToastr } from 'ngx-toastr';
import { provideAnimations } from '@angular/platform-browser/animations';
import { MOMENT } from 'angular-calendar';
import moment from 'moment/moment';
import { ErrorHandlerService } from './services/logging.service';
import { AuthorizationGuard } from './guards/authorization.guard';
import { ManageGuard } from './guards/manage.guard';
import { FormBuilder } from '@angular/forms';
import { API_URL } from './modules/shared/utilities/injection-tokens';
import { environment } from '../environments/environment';
import { ApiModule, Configuration } from '@RoosterPlanner/openapi';

export const appConfig: ApplicationConfig = {
	providers: [
		provideRouter(routes),
		provideHttpClient(withFetch(), withInterceptorsFromDi()),
		provideToastr(),
		importProvidersFrom(
			MsalModule,
			ApiModule.forRoot(
				() => new Configuration({ basePath: environment.apiUrl })
			)
		),
		{
			provide: HTTP_INTERCEPTORS,
			useClass: MsalInterceptor,
			multi: true,
		},
		{
			provide: API_URL,
			useValue: environment.apiUrl,
		},
		{
			provide: MSAL_INSTANCE,
			useFactory: msalInstanceFactory,
		},
		{
			provide: MSAL_GUARD_CONFIG,
			useFactory: msalGuardConfigFactory,
		},
		{
			provide: MSAL_INTERCEPTOR_CONFIG,
			useFactory: msalInterceptorConfigFactory,
		},
		{
			provide: MOMENT,
			useValue: moment,
		},
		{
			provide: ErrorHandler,
			useClass: ErrorHandlerService,
		},
		AuthorizationGuard,
		ManageGuard,
		FormBuilder,
		MsalService,
		MsalGuard,
		MsalBroadcastService,
		provideAnimations(),
	],
};
