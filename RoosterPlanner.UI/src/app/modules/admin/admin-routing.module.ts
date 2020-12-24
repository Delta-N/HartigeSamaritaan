import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {AdminComponent} from "../../pages/admin/admin.component";
import {MsalGuard} from "../../msal";
import {AuthorizationGuard} from "../../guards/authorization.guard";
import {ProfileComponent} from "../../pages/profile/profile.component";
import {AllTasksComponent} from "../../pages/all-tasks/all-tasks.component";
import {CategoryComponent} from "../../pages/category/category.component";

const routes: Routes = [
  { path: '',
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
      MsalGuard,
      AuthorizationGuard
    ]
  },
  {
    path: 'tasks',
    component: AllTasksComponent,
    children:[],
    canActivate: [
      MsalGuard,
      AuthorizationGuard
    ]
  },
  {
    path: 'tasks/category/:id',
    component: CategoryComponent,
    canActivate: [
      MsalGuard,
      AuthorizationGuard
    ]
  },

  ];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
