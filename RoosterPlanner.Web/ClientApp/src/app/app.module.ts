import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { MsalModule, MsalInterceptor } from '@azure/msal-angular';
import { LogLevel } from 'msal';

import { environment } from '../environments/environment';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './pages/home/home.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MaterialModule } from './modules/material/material.module';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { AdminComponent } from './pages/admin/admin.component';
import { NgxTuiCalendarModule } from 'ngx-tui-calendar';
import { QuestionsComponent } from './components/questions/questions.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    AdminComponent,
    QuestionsComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    NgxTuiCalendarModule.forRoot(),
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
