import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { MsalModule, MsalInterceptor } from '@azure/msal-angular';
import { LogLevel } from 'msal';

import { environment } from '../environments/environment';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './modules/home/home.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MaterialModule } from './modules/material/material.module';
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    MaterialModule,
    MsalModule.forRoot({
      clientID: environment.options.appId,
      authority: environment.options.authority,
      redirectUri: environment.options.redirectUri,
      level: LogLevel.Verbose,
      protectedResourceMap: environment.options.protectedResourceMap as [
        string,
        string[]
      ][]
    }),
    BrowserAnimationsModule,
    ReactiveFormsModule
  ],
  entryComponents: [ ],
  exports: [],
  providers: [
    {
    provide: HTTP_INTERCEPTORS,
    useClass: MsalInterceptor,
    multi: true
  }],
  bootstrap: [AppComponent]
})
export class AppModule {}
