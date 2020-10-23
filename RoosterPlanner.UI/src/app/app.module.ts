import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';
import {HTTP_INTERCEPTORS, HttpClientModule} from '@angular/common/http';
import {Configuration} from 'msal';

import {
  MsalModule,
  MsalInterceptor,
  MSAL_CONFIG,
  MSAL_CONFIG_ANGULAR,
  MsalService,
  MsalAngularConfiguration
} from '@azure/msal-angular';

import {msalConfig, msalAngularConfig} from './app-config';

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
import {FormBuilder, ReactiveFormsModule} from "@angular/forms";

function MSALConfigFactory(): Configuration {
  return msalConfig;
}

function MSALAngularConfigFactory(): MsalAngularConfiguration {
  return msalAngularConfig;
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

  ],
  imports: [
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
    MsalModule,
    ReactiveFormsModule
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: MsalInterceptor,
      multi: true
    },
    {
      provide: MSAL_CONFIG,
      useFactory: MSALConfigFactory
    },
    {
      provide: MSAL_CONFIG_ANGULAR,
      useFactory: MSALAngularConfigFactory
    },

    MsalService,
    AuthorizationGuard,
    FormBuilder
  ],
  bootstrap: [AppComponent],
  entryComponents: [AddProjectComponent]
})
export class AppModule {
}
