import * as moment from 'moment';
import { AddProjectComponent } from './components/add-project/add-project.component';
import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { AvailabilityComponent } from './pages/availability/availability.component';
import { BreadcrumbComponent } from './components/breadcrumb/breadcrumb.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {
	BrowserModule,
	REMOVE_STYLES_ON_COMPONENT_DESTROY,
} from '@angular/platform-browser';
import {
	CalendarModule,
	DateAdapter as CalendarDateAdapter,
	MOMENT,
	CalendarMomentDateFormatter,
	CalendarDateFormatter,
} from 'angular-calendar';
import { ChangeProfileComponent } from './components/change-profile/change-profile.component';
import { CommonModule } from '@angular/common';
import { ConfirmDialogComponent } from './components/confirm-dialog/confirm-dialog.component';
import { ErrorHandler, NgModule } from '@angular/core';
import { ErrorHandlerService } from './services/logging.service';
import { FormBuilder, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { HomeComponent } from './pages/home/home.component';
import {
	InteractionType,
	IPublicClientApplication,
	PublicClientApplication,
} from '@azure/msal-browser';
import { MSAL_GUARD_CONFIG, MSAL_INTERCEPTOR_CONFIG } from './msal/constants';
import {
	MSAL_INSTANCE,
	MsalBroadcastService,
	MsalGuard,
	MsalInterceptor,
	MsalService,
} from './msal';
import { ManageModule } from './modules/manage/manage.module';
import { MaterialModule } from './modules/material/material.module';
import { MsalGuardConfiguration } from './msal/msal.guard.config';
import { MsalInterceptorConfig } from './msal/msal.interceptor.config';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NotFoundComponent } from './pages/not-found/not-found.component';
import { ProfileComponent } from './pages/profile/profile.component';
import { ProjectCardComponent } from './components/project-card/project-card.component';
import { ProjectComponent } from './pages/project/project.component';
import { TaskComponent } from './pages/task/task.component';
import { ToastrModule } from 'ngx-toastr';
import { adapterFactory } from 'angular-calendar/date-adapters/moment';
import { environment } from '../environments/environment';
import { AuthorizationGuard } from './guards/authorization.guard';
import { ManageGuard } from './guards/manage.guard';
import { AdminModule } from './modules/admin/admin.module';
import { ScheduleComponent } from './pages/schedule/schedule.component';
import { ScheduleManagerComponent } from './pages/schedule-manager/schedule-manager.component';
import { AcceptPrivacyPolicyComponent } from './components/accept-privacy-policy/accept-privacy-policy.component';
import { NgxDocViewerModule } from 'ngx-doc-viewer';
import { PrivacyComponent } from './pages/privacy/privacy.component';
import { ChangeProfilePictureComponent } from './components/change-profile-picture/change-profile-picture.component';
import { CertificateComponent } from './pages/certificate/certificate.component';
import { CalendarTaskLink, TableDatePipe } from './helpers/filter.pipe';
import { RequirementComponent } from './pages/requirement/requirement.component';

export function momentAdapterFactory() {
	return adapterFactory(moment);
}

export const isIE =
	window.navigator.userAgent.indexOf('MSIE ') > -1 ||
	window.navigator.userAgent.indexOf('Trident/') > -1;

function MSALInstanceFactory(): IPublicClientApplication {
	return new PublicClientApplication({
		auth: {
			clientId: environment.auth.clientId,
			authority: environment.auth.authority,
			navigateToLoginRequestUrl: environment.auth.navigateToLoginRequestUrl,
			knownAuthorities: environment.auth.knownAuthorities,
			redirectUri: environment.auth.redirectUri,
		},
		cache: {
			cacheLocation: 'sessionStorage',
			storeAuthStateInCookie: isIE,
		},
		system: {
			tokenRenewalOffsetSeconds: 0,
			loadFrameTimeout: 9000,
		},
	});
}

function MSALInterceptorConfigFactory(): MsalInterceptorConfig {
	const protectedResourceMap = new Map<string, Array<string>>();
	environment.protectedResourceMap.forEach((input) => {
		const string1 = input[0] as string;
		const string2 = input[1] as string[];
		protectedResourceMap.set(string1, string2);
	});
	return {
		interactionType: InteractionType.Redirect,
		protectedResourceMap,
	};
}

@NgModule({
	declarations: [
		AddProjectComponent,
		AppComponent,
		AvailabilityComponent,
		BreadcrumbComponent,
		ChangeProfileComponent,
		ConfirmDialogComponent,
		HomeComponent,
		NotFoundComponent,
		ProfileComponent,
		ProjectCardComponent,
		ProjectComponent,
		TaskComponent,
		ScheduleComponent,
		ScheduleManagerComponent,
		AcceptPrivacyPolicyComponent,
		PrivacyComponent,
		ChangeProfilePictureComponent,
		CertificateComponent,
		CalendarTaskLink,
		RequirementComponent,
		TableDatePipe,
	],
	imports: [
		AppRoutingModule,
		BrowserAnimationsModule,
		BrowserModule,
		CommonModule,
		FormsModule,
		HttpClientModule,
		MaterialModule,
		NgbModule,
		ReactiveFormsModule,
		ToastrModule.forRoot(),
		NgxDocViewerModule,
		CalendarModule.forRoot(
			{
				provide: CalendarDateAdapter,
				useFactory: momentAdapterFactory,
			},
			{
				dateFormatter: {
					provide: CalendarDateFormatter,
					useClass: CalendarMomentDateFormatter,
				},
			}
		),
		ManageModule,
		AdminModule,
	],
	providers: [
		AuthorizationGuard,
		ManageGuard,
		MsalService,
		MsalGuard,
		MsalBroadcastService,
		FormBuilder,
		{
			provide: HTTP_INTERCEPTORS,
			useClass: MsalInterceptor,
			multi: true,
		},
		{
			provide: MSAL_INSTANCE,
			useFactory: MSALInstanceFactory,
		},
		{
			provide: MSAL_GUARD_CONFIG,
			useValue: {
				interactionType: InteractionType.Redirect,
			} as MsalGuardConfiguration,
		},
		{
			provide: MSAL_INTERCEPTOR_CONFIG,
			useFactory: MSALInterceptorConfigFactory,
		},
		{
			provide: ErrorHandler,
			useClass: ErrorHandlerService,
		},
		{
			provide: MOMENT,
			useValue: moment,
		},
		{ provide: REMOVE_STYLES_ON_COMPONENT_DESTROY, useValue: false },
	],
	bootstrap: [AppComponent],
})
export class AppModule {}
