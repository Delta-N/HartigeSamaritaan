import {Component, Inject, OnInit} from '@angular/core';
import {Task} from 'src/app/models/task';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {TaskService} from "../../services/task.service";
import {Project} from "../../models/project";
import {Projecttask} from "../../models/projecttask";
import {EntityHelper} from "../../helpers/entity-helper";
import {ToastrService} from "ngx-toastr";

@Component({
  selector: 'app-add-project-task',
  templateUrl: './add-project-task.component.html',
  styleUrls: ['./add-project-task.component.scss']
})
export class AddProjectTaskComponent implements OnInit {
  loaded: boolean;
  project: Project;
  allTasks: Task[] | null = [];
  currentProjectTasks: Task[] | null;
  title: string;
  reasonableMaxInteger = 10000;
  currentPage: number = 1;
  pageSize: number = 5;
  searchText: string = '';

  constructor(@Inject(MAT_DIALOG_DATA) public data: any,
              public dialogRef: MatDialogRef<AddProjectTaskComponent>,
              private taskService: TaskService,
              private toastr: ToastrService) {
  }

  async ngOnInit(): Promise<void> {
    this.project = this.data.project;
    if (this.data.currentProjectTasks.length >= this.pageSize) {
      await this.getCurrentProjectTasks()
    } else {
      this.currentProjectTasks = this.data.currentProjectTasks;
    }

    if (this.data.modifier === 'add') {
      this.title = 'Toevoegen';
      await this.taskService.getAllTasks(0, this.reasonableMaxInteger).then(tasks => {
          tasks?.forEach(t => {
            if (!this.currentProjectTasks?.find(cpt => cpt.id === t.id)) {
              this.allTasks?.push(t)
            }
          })
        }
      )
      this.loaded = true;
    }

    if (this.data.modifier === 'remove') {
      this.title = 'Verwijderen';
      this.allTasks = this.currentProjectTasks;
      this.loaded = true;
    }
    this.allTasks?.sort((a, b) => a.name > b.name ? 1 : -1)
  }

  async getCurrentProjectTasks() {
    await this.taskService.getAllProjectTasks(this.project.id).then(cpt => this.currentProjectTasks = cpt)
  }

  prevPage() {
    if (this.currentPage != 1) {
      this.currentPage--;
    }
  }

  nextPage() {
    if (this.currentPage != Math.ceil(this.allTasks?.length ?? 0 / this.pageSize)) {
      this.currentPage++;
    }
  }

  resetPage() {
    this.currentPage = 1;
  }

  close() {
    this.dialogRef.close();
  }

  async modProjectTask(id: string) {
    let task: Task | undefined = this.allTasks?.find(t => t.id === id)
    if (this.data.modifier === 'add') {
      let projectTask: Projecttask = new Projecttask();
      projectTask.projectId = this.project.id;
      projectTask.taskId = task?.id ?? '';
      projectTask.id = EntityHelper.returnEmptyGuid();
      await this.taskService.addTaskToProject(projectTask).then(res => {
          if (res) {
            this.currentProjectTasks?.push(res)
            this.toastr.success("Taak toegevoegd")
            this.dialogRef.close();
          }
        }
      )
    }
    if (this.data.modifier === 'remove') {
      await this.taskService.removeTaskFromProject(this.project.id, id).then(res => {
        if (res) {
          this.currentProjectTasks = this.currentProjectTasks?.filter(cpt => cpt.id !== id)??[];
          this.toastr.success("Taak verwijderd")
          this.dialogRef.close(this.currentProjectTasks);
        }
      })
    }


  }
}
