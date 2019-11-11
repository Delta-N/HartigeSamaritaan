import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ProjectComponent } from 'src/app/pages/project/project.component';
import { AddProjectComponent } from '../../components/add-project/add-project.component';

const routes: Routes = [
  {
    path: '',
    component: ProjectComponent,
    pathMatch: 'full'
  },
  {
    path: 'add',
    component: AddProjectComponent,
    pathMatch: 'full'
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ProjectRoutingModule {}
