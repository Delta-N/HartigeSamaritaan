import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '../material/material.module';
import { ProjectComponent } from 'src/app/pages/project/project.component';
import { ProjectRoutingModule } from './project-routing.module';
import { AddProjectComponent } from '../../components/add-project/add-project.component';

@NgModule({
  declarations: [AddProjectComponent, ProjectComponent],
  imports: [CommonModule, MaterialModule, ProjectRoutingModule]
})
export class ProjectModule {}
