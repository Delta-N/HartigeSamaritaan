import {Injectable} from '@angular/core';
import {ApiService} from "./api.service";
import {ToastrService} from "ngx-toastr";
import {Task} from "../models/task";
import {EntityHelper} from "../helpers/entity-helper";
import {HttpResponse} from "@angular/common/http";
import {HttpRoutes} from "../helpers/HttpRoutes";
import {Projecttask} from "../models/projecttask";

@Injectable({
  providedIn: 'root'
})
export class TaskService {

  constructor(private apiService: ApiService,
              private toastr: ToastrService) {
  }

  async getTask(guid: string): Promise<Task> {
    if (!guid) {
      this.toastr.error("taskId mag niet leeg zijn")
      return null
    }
    let task: Task = null;
    await this.apiService.get<HttpResponse<Task>>(`${HttpRoutes.taskApiUrl}/${guid}`).toPromise().then(response => {
      if (response.status === 200) {
        task = response.body
      } else {
        this.toastr.error("Fout tijdens het ophalen van de taak")
      }
    });
    return task
  }

  async getAllTasks(offset: number, pageSize: number): Promise<Task[]> {
    let tasks: Task[] = [];
    await this.apiService.get<HttpResponse<Task[]>>(`${HttpRoutes.taskApiUrl}?offset=${offset}&pageSize=${pageSize}`).toPromise().then(response => {
        if (response.status === 200) {
          tasks = response.body
        } else {
          this.toastr.error("Fout tijdens het ophalen van de taken.")
        }
      }
    );
    return tasks
  }

  async postTask(task: Task) {
    if (task === null || task.name === null || task.category === null || task.category.id === null) {
      this.toastr.error("Ongeldige taak")
      return null;
    }

    if (task.id === null || task.id === "") {
      task.id = EntityHelper.returnEmptyGuid()
    }
    let resTask: Task = null;
    await this.apiService.post<HttpResponse<Task>>(`${HttpRoutes.taskApiUrl}`, task).toPromise().then(response => {
      if (response.status === 200) {
        resTask = response.body
      } else {
        this.toastr.error("Fout tijdens het posten van een taak")
      }
    })
    return resTask;
  }

  async updateTask(updatedTask: Task) {
    if (updatedTask === null || updatedTask.name === null || updatedTask.category === null || updatedTask.category.id === null) {
      this.toastr.error("Ongeldige taak")
      return null;
    }
    if (updatedTask.id === null || updatedTask.id === "") {
      this.toastr.error("TaskID is leeg")
      return null;
    }
    let resTask: Task = null
    await this.apiService.put<HttpResponse<Task>>(`${HttpRoutes.taskApiUrl}`, updatedTask).toPromise().then(response => {
      if (response.status === 200) {
        resTask = response.body
      } else {
        this.toastr.error("Fout tijdens het updaten van de taak")
      }
    })
    return resTask;
  }

  async deleteTask(guid: string) {
    if (guid === null || guid == "") {
      this.toastr.error("TaskId is leeg")
      return null;
    }
    let result: boolean = false;
    await this.apiService.delete<HttpResponse<Number>>(`${HttpRoutes.taskApiUrl}/${guid}`).toPromise().then(response => {
      if (response.status === 200) {
        result = true;
      } else {
        this.toastr.error("Fout tijdens het verwijderen van de taak")
      }
    })
    return result;
  }

  async getAllProjectTasks(guid: string): Promise<Task[]> {
    if (guid == null) {
      this.toastr.error("ProjectTaskId is leeg")
      return null;
    }
    let projecttasks: Task[] = [];
    await this.apiService.get<HttpResponse<Task[]>>(`${HttpRoutes.taskApiUrl}/GetAllProjectTasks/${guid}`).toPromise().then(response => {
        if (response.status === 200) {
          projecttasks = response.body
        } else {
          this.toastr.error("Fout tijdens het ophalen van project taken");
        }
      }
    )
    return projecttasks;
  }

  async addTaskToProject(projectTask: Projecttask) {
    if (projectTask === null) {
      this.toastr.error("ProjectTask is leeg")
      return null;
    }
    let task: Task = null;
    await this.apiService.post<HttpResponse<Task>>(`${HttpRoutes.taskApiUrl}/AddTaskToProject`, projectTask).toPromise().then(response => {
      if (response.status === 200) {
        task = response.body
      } else {
        this.toastr.error("Fout tijdens het toevoegen van de taak aan het project")
      }
    })
    return task;
  }

  async removeTaskFromProject(projectId: string, taskId: string) {
    if (projectId == null || taskId == null) {
      this.toastr.error("Ongeldige projectId en/of taskId")
      return null;
    }
    let id: string = null;
    await this.apiService.delete<HttpResponse<string>>(`${HttpRoutes.taskApiUrl}/RemoveTaskFromProject/${projectId}/${taskId}`).toPromise().then(response => {
      if (response.status === 200) {
        id = response.body
      } else {
        this.toastr.error("Fout tijdens het verwijderen van de taak uit het project")
      }
    })
    return id;
  }
}
