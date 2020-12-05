import {BrowserModule} from '@angular/platform-browser';
import {ErrorHandler, NgModule} from '@angular/core';
import {HTTP_INTERCEPTORS, HttpClientModule} from '@angular/common/http';

import {CommonModule} from "@angular/common";
import {ToastrModule} from "ngx-toastr";

import {AppRoutingModule} from './app-routing.module';
import {AppComponent} from './app.component';
import {HomeComponent} from './pages/home/home.component';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {MaterialModule} from './modules/material/material.module';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import {NotFoundComponent} from './pages/not-found/not-found.component';
import {MatSidenavModule} from "@angular/material/sidenav";
import {MatCardModule} from "@angular/material/card";
import {MatDialogModule} from "@angular/material/dialog";
import {ProjectCardComponent} from './components/project-card/project-card.component';
import {AdminComponent} from './pages/admin/admin.component';
import {ProfileComponent} from './pages/profile/profile.component';
import {AddProjectComponent} from './components/add-project/add-project.component';
import {MatCheckboxModule} from "@angular/material/checkbox";
import {ShiftComponent} from './pages/shift/shift.component';
import {AuthorizationGuard} from "./guards/authorization.guard";
import {CreateProjectComponent} from "./components/create-project/create-project.component";
import {FormBuilder, FormsModule, ReactiveFormsModule} from "@angular/forms";
import {AddAdminComponent} from './components/add-admin/add-admin.component';
import {DatePipe, FilterPipe, ManagerFilterPipe, TaskFilterPipe} from "./helpers/filter.pipe";
import {ChangeProfileComponent} from './components/change-profile/change-profile.component';
import {ProjectComponent} from './pages/project/project.component';
import {ConfirmDialogComponent} from './components/confirm-dialog/confirm-dialog.component';
import {InteractionType, IPublicClientApplication, PublicClientApplication} from "@azure/msal-browser";
import {MsalInterceptorConfig} from "./msal/msal.interceptor.config";
import {MSAL_INSTANCE, MsalBroadcastService, MsalGuard, MsalInterceptor, MsalService} from "./msal";
import {MSAL_GUARD_CONFIG, MSAL_INTERCEPTOR_CONFIG} from "./msal/constants";
import {MsalGuardConfiguration} from "./msal/msal.guard.config";
import {environment} from "../environments/environment";
import {TaskComponent} from './pages/task/task.component';
import {AddTaskComponent} from './components/add-task/add-task.component';
import {AllTasksComponent} from './pages/all-tasks/all-tasks.component';
import {CategoryComponent} from './pages/category/category.component';
import {AddCategoryComponent} from './components/add-category/add-category.component';
import {AddProjectTaskComponent} from './components/add-project-task/add-project-task.component';
import {NgxMaterialTimepickerModule} from 'ngx-material-timepicker';

import {
  MAT_MOMENT_DATE_FORMATS,
  MomentDateAdapter,
  MAT_MOMENT_DATE_ADAPTER_OPTIONS,
} from '@angular/material-moment-adapter';
import {DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE} from '@angular/material/core';
import {MatChipsModule} from "@angular/material/chips";
import {NgxMultipleDatesModule} from "ngx-multiple-dates";
import {ShiftOverviewComponent} from './pages/shift-overview/shift-overview.component';
import {AddShiftsComponent} from './components/add-shifts/add-shifts.component';
import {EditShiftComponent} from './components/edit-shift/edit-shift.component';
import {ManageComponent} from './pages/manage/manage.component';
import {ManageGuard} from "./guards/manage.guard";
import {AddManagerComponent} from './components/add-manager/add-manager.component';
import {BreadcrumbComponent} from './components/breadcrumb/breadcrumb.component';
import {ErrorHandlerService} from "./services/logging.service";

export const isIE = window.navigator.userAgent.indexOf('MSIE ') > -1 || window.navigator.userAgent.indexOf('Trident/') > -1;

function MSALInstanceFactory(): IPublicClientApplication {
  return new PublicClientApplication({
    auth: {
      clientId: environment.auth.clientId,
      authority: environment.auth.authority,
      navigateToLoginRequestUrl: environment.auth.navigateToLoginRequestUrl,
      knownAuthorities: environment.auth.knownAuthorities,
      redirectUri: environment.auth.redirectUri
    },
    cache: {
      cacheLocation: 'sessionStorage',
      storeAuthStateInCookie: isIE
    },
    system: {
      tokenRenewalOffsetSeconds: 0,
      loadFrameTimeout: 9000,
    }

  });
}

function MSALInterceptorConfigFactory(): MsalInterceptorConfig {
  const protectedResourceMap = new Map<string, Array<string>>();
  environment.protectedResourceMap.forEach(input => {
    let string1 = input[0] as string
    let string2 = input[1] as string[]
    protectedResourceMap.set(string1, string2)

  })
  return {
    interactionType: InteractionType.Redirect,
    protectedResourceMap,
  };
}

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    NotFoundComponent,
    ProjectCardComponent,
    AdminComponent,
    ProfileComponent,
    AddProjectComponent,
    ShiftComponent,
    CreateProjectComponent,
    AddAdminComponent,
    FilterPipe,
    ChangeProfileComponent,
    ProjectComponent,
    ConfirmDialogComponent,
    TaskComponent,
    AddTaskComponent,
    AllTasksComponent,
    CategoryComponent,
    AddCategoryComponent,
    AddProjectTaskComponent,
    TaskFilterPipe,
    DatePipe,
    ShiftOverviewComponent,
    AddShiftsComponent,
    EditShiftComponent,
    ManageComponent,
    AddManagerComponent,
    ManagerFilterPipe,
    BreadcrumbComponent,
    AvailabilityComponent,

  ],
  imports: [
    CommonModule,
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MaterialModule,
    NgbModule,
    MatSidenavModule,
    MatCardModule,
    MatDialogModule,
    MatCheckboxModule,
    HttpClientModule,
    ReactiveFormsModule,
    FormsModule,
    ToastrModule.forRoot(),
    NgxMaterialTimepickerModule,
    MatChipsModule,
    NgxMultipleDatesModule,

  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: MsalInterceptor,
      multi: true
    },
    {
      provide: MSAL_INSTANCE,
      useFactory: MSALInstanceFactory
    },
    {
      provide: MSAL_GUARD_CONFIG,
      useValue: {
        interactionType: InteractionType.Redirect
      } as MsalGuardConfiguration
    },
    {
      provide: MSAL_INTERCEPTOR_CONFIG,
      useFactory: MSALInterceptorConfigFactory
    },
    {provide: MAT_DATE_LOCALE, useValue: 'nl-NL'},
    {
      provide: DateAdapter,
      useClass: MomentDateAdapter,
      deps: [MAT_DATE_LOCALE, MAT_MOMENT_DATE_ADAPTER_OPTIONS]
    },
    {provide: MAT_DATE_FORMATS, useValue: MAT_MOMENT_DATE_FORMATS},
    MsalService,
    MsalGuard,
    MsalBroadcastService,
    AuthorizationGuard,
    FormBuilder,
    ManageGuard,

    {provide: ErrorHandler, useClass: ErrorHandlerService}
  ],
  bootstrap: [AppComponent],
  entryComponents: [AddProjectComponent]
})
export class AppModule {
}
