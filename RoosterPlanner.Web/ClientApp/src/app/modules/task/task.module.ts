import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TaskComponent } from '../../pages/task/task.component';
import { MaterialModule } from '../material/material.module';
import { TaskRoutingModule } from './task-routing.module';

@NgModule({
  declarations: [TaskComponent],
  imports: [CommonModule, MaterialModule, TaskRoutingModule]
})
export class TaskModule {}
