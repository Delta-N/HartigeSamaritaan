import {AvailabilityComponent} from "./pages/availability/availability.component";
import {HomeComponent} from "./pages/home/home.component";
import {MsalGuard} from "./msal";
import {NgModule} from '@angular/core';
import {NotFoundComponent} from "./pages/not-found/not-found.component";
import {ProfileComponent} from "./pages/profile/profile.component";
import {ProjectComponent} from "./pages/project/project.component";
import {RouterModule, Routes} from '@angular/router';
import {TaskComponent} from "./pages/task/task.component";
import {AuthorizationGuard} from "./guards/authorization.guard";
import {ManageGuard} from "./guards/manage.guard";
import {ScheduleComponent} from "./pages/schedule/schedule.component";
import {PrivacyComponent} from "./pages/privacy/privacy.component";
import {CertificateComponent} from "./pages/certificate/certificate.component";
import {RequirementComponent} from "./pages/requirement/requirement.component";


const routes: Routes = [
  {path: 'home', component: HomeComponent, canActivate: [MsalGuard]},
  {path: 'availability/:id', component: AvailabilityComponent, canActivate: [MsalGuard]},
  {path: 'profile', component: ProfileComponent, canActivate: [MsalGuard]},
  {path: 'project/:id', component: ProjectComponent, canActivate: [MsalGuard]},
  {path: 'task/:id', component: TaskComponent, canActivate: [MsalGuard]},
  {path: 'requirement/:id', component: RequirementComponent, canActivate: [MsalGuard]},
  {path: 'schedule/:id', component: ScheduleComponent, canActivate: [MsalGuard]},
  {path: 'privacy', component: PrivacyComponent},
  {path: 'certificate/:id', component: CertificateComponent},
  {path: 'manage',loadChildren: () => import('./modules/manage/manage.module').then(m => m.ManageModule),canLoad: [ManageGuard]  },
  {path: 'admin',loadChildren: () => import('./modules/admin/admin.module').then(m => m.AdminModule),canLoad: [AuthorizationGuard]},
  {path: '', component: HomeComponent, canActivate: [MsalGuard]},
  {path: '**', component: NotFoundComponent,}
]


const isIframe = window !== window.parent && !window.opener;

@NgModule({
  imports: [RouterModule.forRoot(routes, {
    useHash: false,
  })],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
