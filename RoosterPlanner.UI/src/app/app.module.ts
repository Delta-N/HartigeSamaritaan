import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';
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
import {FilterPipe} from "./helpers/filter.pipe";
import {ChangeProfileComponent} from './components/change-profile/change-profile.component';
import {ProjectComponent} from './pages/project/project.component';
import {ConfirmDialogComponent} from './components/confirm-dialog/confirm-dialog.component';
import {InteractionType, IPublicClientApplication, PublicClientApplication} from "@azure/msal-browser";
import {MsalInterceptorConfig} from "./msal/msal.interceptor.config";
import {MSAL_INSTANCE, MsalBroadcastService, MsalGuard, MsalInterceptor, MsalService} from "./msal";
import {MSAL_GUARD_CONFIG, MSAL_INTERCEPTOR_CONFIG} from "./msal/constants";
import {MsalGuardConfiguration} from "./msal/msal.guard.config";
import {environment} from "../environments/environment";


function MSALInstanceFactory(): IPublicClientApplication {
  return new PublicClientApplication({
    auth: {
      clientId: environment.auth.clientId,
      authority: environment.auth.authority,
      navigateToLoginRequestUrl: environment.auth.navigateToLoginRequestUrl,
      knownAuthorities: environment.auth.knownAuthorities,
      redirectUri: environment.auth.redirectUri
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
    ConfirmDialogComponent

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
    MsalService,
    MsalGuard,
    MsalBroadcastService,
    AuthorizationGuard,
    FormBuilder
  ],
  bootstrap: [AppComponent],
  entryComponents: [AddProjectComponent]
})
export class AppModule {
}
