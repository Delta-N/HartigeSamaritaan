import {NgModule} from '@angular/core';
import {Routes, RouterModule} from '@angular/router';
import {AdminComponent} from "../../pages/admin/admin.component";
import {ProfileComponent} from "../../pages/profile/profile.component";
import {AllTasksComponent} from "../../pages/all-tasks/all-tasks.component";
import {CategoryComponent} from "../../pages/category/category.component";

const routes: Routes = [
  {path: '', component: AdminComponent},
  {path: 'profile/:id', component: ProfileComponent},
  {path: 'tasks', component: AllTasksComponent},
  {path: 'tasks/category/:id', component: CategoryComponent},
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule {
}
