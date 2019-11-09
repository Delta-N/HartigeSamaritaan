import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from '../material/material.module';
import { ProjectComponent } from 'src/app/pages/project/project.component';
import { ProjectRoutingModule } from './project-routing.module';

@NgModule({
  declarations: [ProjectComponent],
  imports: [CommonModule, MaterialModule, ProjectRoutingModule]
})
export class ProjectModule {}
