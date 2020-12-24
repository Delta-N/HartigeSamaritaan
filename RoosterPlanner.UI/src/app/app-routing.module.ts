import {AvailabilityComponent} from "./pages/availability/availability.component";
import {HomeComponent} from "./pages/home/home.component";
import {MsalGuard} from "./msal";
import {NgModule} from '@angular/core';
import {NotFoundComponent} from "./pages/not-found/not-found.component";
import {ProfileComponent} from "./pages/profile/profile.component";
import {ProjectComponent} from "./pages/project/project.component";
import {RouterModule, Routes} from '@angular/router';
import {TaskComponent} from "./pages/task/task.component";


const routes: Routes = [
  {
    path: '',
    component: HomeComponent,
    canActivate: [
      MsalGuard
    ]
  },
  {
    path: 'home',
    component: HomeComponent,
    canActivate: [
      MsalGuard
    ]
  },
  {
    path: 'availability/:id',
    component: AvailabilityComponent,
    canActivate: [
      MsalGuard
    ]
  },
  {
    path: 'profile',
    component: ProfileComponent,
    canActivate: [
      MsalGuard
    ]
  },

  {
    path: 'project/:id',
    component: ProjectComponent,
    canActivate: [
      MsalGuard
    ]
  },
  {
    path: 'task/:id',
    component: TaskComponent,
    canActivate: [
      MsalGuard
    ]
  },
  { path: 'admin', loadChildren: () => import('./modules/admin/admin.module').then(m => m.AdminModule) },
  { path: 'manage', loadChildren: () => import('./modules/manage/manage.module').then(m => m.ManageModule) },

  {
    path: '**',
    component: NotFoundComponent
  }
];
const isIframe = window !== window.parent && !window.opener;

@NgModule({
  imports: [RouterModule.forRoot(routes, {
    useHash: false,
    // Don't perform initial navigation in iframes
    initialNavigation: !isIframe ? 'enabled' : 'disabled'
  })],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
