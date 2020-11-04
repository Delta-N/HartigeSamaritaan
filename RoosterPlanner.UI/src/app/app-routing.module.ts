import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {HomeComponent} from "./pages/home/home.component";
import {NotFoundComponent} from "./pages/not-found/not-found.component";
import {AdminComponent} from "./pages/admin/admin.component";
import {ProfileComponent} from "./pages/profile/profile.component";
import {ShiftComponent} from "./pages/shift/shift.component";
import {AuthorizationGuard} from "./guards/authorization.guard";
import {ProjectComponent} from "./pages/project/project.component";
import {MsalGuard} from "./msal";

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
    path: 'admin',
    component: AdminComponent,
    canActivate: [
      MsalGuard,
      AuthorizationGuard
    ]
  },
  {
    path: 'profile/:id',
    component: ProfileComponent,
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
    path: 'shift',
    component: ShiftComponent,
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
    // Needed for hash routing
    path: 'code',
    component: HomeComponent
  },
  {
    // todo
    path: 'state',
    component: HomeComponent
  },
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
