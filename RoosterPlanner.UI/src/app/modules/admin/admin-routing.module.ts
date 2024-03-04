import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AdminComponent } from '../../pages/admin/admin.component';
import { ProfileComponent } from '../../pages/profile/profile.component';
import { AllTasksComponent } from '../../pages/all-tasks/all-tasks.component';
import { CategoryComponent } from '../../pages/category/category.component';
import { EmployeeComponent } from '../../pages/employee/employee.component';
import { CertificateTypeComponent } from '../../pages/certificate-type/certificate-type.component';

const routes: Routes = [
	{ path: '', component: AdminComponent },
	{ path: 'profile/:id', component: ProfileComponent },
	{ path: 'tasks', component: AllTasksComponent },
	{ path: 'tasks/category/:id', component: CategoryComponent },
	{ path: 'tasks/certificatetype/:id', component: CertificateTypeComponent },
	{ path: 'employee', component: EmployeeComponent },
];

@NgModule({
	imports: [RouterModule.forChild(routes)],
	exports: [RouterModule],
})
export class AdminRoutingModule {}
