import { Component, OnInit } from '@angular/core';
import { TaskService } from '../../core/task/task.service';
import { Task } from '../../models/task.model';

@Component({
  selector: 'app-task',
  templateUrl: './task.component.html',
  styleUrls: ['./task.component.less']
})
export class TaskComponent implements OnInit {
  public dataColumns = ['name', 'color', 'documentUri'];
  public tasks: Array<Task>;

  constructor(private taskService: TaskService) {}

  ngOnInit() {
    this.tasks = new Array<Task>();
    this.taskService.getAll().subscribe(result => {
      if (result) {
        this.tasks = result;
      }
    });
  }
}
