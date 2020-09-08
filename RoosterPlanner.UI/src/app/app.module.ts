import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './pages/home/home.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MaterialModule } from './modules/material/material.module';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NotFoundComponent } from './pages/not-found/not-found.component';
import {MatSidenavModule} from "@angular/material/sidenav";
import {MatCardModule} from "@angular/material/card";
import { ProjectCardComponent } from './components/project-card/project-card.component';
import { AdminComponent } from './pages/admin/admin.component';


@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    NotFoundComponent,
    ProjectCardComponent,
    AdminComponent,

  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MaterialModule,
    NgbModule,
    MatSidenavModule,
    MatCardModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
