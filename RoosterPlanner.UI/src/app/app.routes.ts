import { Routes } from '@angular/router';
import {MsalGuard} from "@azure/msal-angular";
import {NotFoundComponent} from "./pages/not-found/not-found.component";
import {HomeComponent} from "./pages/home/home.component";
import {AvailabilityComponent} from "./pages/availability/availability.component";
import {ProfileComponent} from "./pages/profile/profile.component";
import {ProjectComponent} from "./pages/project/project.component";
import {TaskComponent} from "./pages/task/task.component";
import {RequirementComponent} from "./pages/requirement/requirement.component";
import {ScheduleComponent} from "./pages/schedule/schedule.component";
import {PrivacyComponent} from "./pages/privacy/privacy.component";
import {CertificateComponent} from "./pages/certificate/certificate.component";
import {manageCanActivateGuard, ManageGuard} from "./guards/manage.guard";
import {authCanActivateGuard, AuthorizationGuard} from "./guards/authorization.guard";

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'home',
    pathMatch: 'full',
  },
  {path: 'home', component: HomeComponent, canActivate: [MsalGuard]},
  {path: 'availability/:id', component: AvailabilityComponent, canActivate: [MsalGuard]},
  {path: 'profile', component: ProfileComponent, canActivate: [MsalGuard]},
  {path: 'project/:id', component: ProjectComponent, canActivate: [MsalGuard]},
  {path: 'task/:id', component: TaskComponent, canActivate: [MsalGuard]},
  {path: 'requirement/:id', component: RequirementComponent, canActivate: [MsalGuard]},
  {path: 'schedule/:id', component: ScheduleComponent, canActivate: [MsalGuard]},
  {path: 'privacy', component: PrivacyComponent},
  {path: 'certificate/:id', component: CertificateComponent},
  {
    path: 'manage',
    loadChildren: () => import('./modules/manage/manage.module').then(m => m.ManageModule),
    canLoad: [manageCanActivateGuard]
  },
  {
    path: 'admin',
    loadChildren: () => import('./modules/admin/admin.module').then(m => m.AdminModule),
    canLoad: [authCanActivateGuard]
  },
  {path: '', component: HomeComponent, canActivate: [MsalGuard]},
  {path: '**', component: NotFoundComponent,}

];
