import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminRoutingModule } from './admin-routing.module';
import {AdminComponent} from "../../pages/admin/admin.component";
import {MaterialModule} from "../material/material.module";
import {AllTasksComponent} from "../../pages/all-tasks/all-tasks.component";
import {AddCategoryComponent} from "../../components/add-category/add-category.component";
import {AddAdminComponent} from "../../components/add-admin/add-admin.component";
import {FilterPipe, ManagerFilterPipe} from "../../helpers/filter.pipe";
import {AddTaskComponent} from "../../components/add-task/add-task.component";
import {AddManagerComponent} from "../../components/add-manager/add-manager.component";
import {CreateProjectComponent} from "../../components/create-project/create-project.component";
import {CategoryComponent} from "../../pages/category/category.component";
import {AuthorizationGuard} from "../../guards/authorization.guard";


@NgModule({
  declarations: [
    AdminComponent,
    AllTasksComponent,
    AddCategoryComponent,
    AddAdminComponent,
    FilterPipe,
    ManagerFilterPipe,
    AddTaskComponent,
    AddManagerComponent,
    CreateProjectComponent,
    CategoryComponent,
  ],
  imports: [
    CommonModule,
    AdminRoutingModule,
    MaterialModule,
  ],
  providers:[]
})
export class AdminModule { }
