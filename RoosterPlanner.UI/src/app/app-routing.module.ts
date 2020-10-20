import {NgModule} from '@angular/core';
import {Routes, RouterModule} from '@angular/router';
import {HomeComponent} from "./pages/home/home.component";
import {NotFoundComponent} from "./pages/not-found/not-found.component";
import {AdminComponent} from "./pages/admin/admin.component";
import {ProfileComponent} from "./pages/profile/profile.component";
import {ShiftComponent} from "./pages/shift/shift.component";
import {MsalGuard} from "@azure/msal-angular";
import {AuthorizationGuard} from "./guards/authorization.guard";

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
    path: '**',
    component: NotFoundComponent
  }

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
