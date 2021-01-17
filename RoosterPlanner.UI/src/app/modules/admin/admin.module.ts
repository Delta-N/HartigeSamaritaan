import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminRoutingModule } from './admin-routing.module';
import {AdminComponent} from "../../pages/admin/admin.component";
import {MaterialModule} from "../material/material.module";
import {AllTasksComponent} from "../../pages/all-tasks/all-tasks.component";
import {AddCategoryComponent} from "../../components/add-category/add-category.component";
import {AddAdminComponent} from "../../components/add-admin/add-admin.component";
import {ColorPipe, FilterPipe, ManagerFilterPipe, ProjectClosedPipe} from "../../helpers/filter.pipe";
import {AddTaskComponent} from "../../components/add-task/add-task.component";
import {AddManagerComponent} from "../../components/add-manager/add-manager.component";
import {CreateProjectComponent} from "../../components/create-project/create-project.component";
import {CategoryComponent} from "../../pages/category/category.component";
import {UploadPrivacyPolicyComponent} from "../../components/upload-privacy-policy/upload-privacy-policy.component";
import {EmployeeComponent} from "../../pages/employee/employee.component";
import {ManageModule} from "../manage/manage.module";
import {AddCertificatetypeComponent} from "../../components/add-certificatetype/add-certificatetype.component";
import {CertificateTypeComponent} from "../../pages/certificate-type/certificate-type.component";
import {AddCertificateComponent} from "../../components/add-certificate/add-certificate.component";
import {AddRequirementComponent} from "../../components/add-requirement/add-requirement.component";


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
    UploadPrivacyPolicyComponent,
    EmployeeComponent,
    AddCertificatetypeComponent,
    CertificateTypeComponent,
    AddCertificateComponent,
    ColorPipe,
    ProjectClosedPipe,
    AddRequirementComponent
  ],
    imports: [
        CommonModule,
        AdminRoutingModule,
        MaterialModule,
        ManageModule,
    ],
  exports:[ColorPipe],
  providers:[]
})
export class AdminModule { }
