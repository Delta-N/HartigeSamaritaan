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
    let task: Task;
    await this.apiService.get<HttpResponse<Task>>(`${HttpRoutes.taskApiUrl}/${guid}`).toPromise().then(response =>
      task = response.body);
    return task
  }

  async getAllTasks(offset: number, pageSize: number): Promise<Task[]> {
    let tasks: Task[] = [];
    await this.apiService.get<HttpResponse<Task[]>>(`${HttpRoutes.taskApiUrl}?offset=${offset}&pageSize=${pageSize}`).toPromise().then(response =>
      tasks = response.body
    );
    return tasks
  }

  postTask(task: Task) {
    if (task === null || task.name === null || task.category === null || task.category.id === null) {
      this.toastr.error("Ongeldige taak")
      return null;
    }

    if (task.id === null || task.id === "") {
      task.id = EntityHelper.returnEmptyGuid()
    }
    return this.apiService.post<HttpResponse<Task>>(`${HttpRoutes.taskApiUrl}`, task).toPromise()

  }

  updateTask(updatedTask: Task) {
    if (updatedTask === null || updatedTask.name === null || updatedTask.category === null || updatedTask.category.id === null) {
      this.toastr.error("Ongeldige taak")
      return null;
    }
    if (updatedTask.id === null || updatedTask.id === "") {
      this.toastr.error("TaskID is leeg")
      return null;
    }
    return this.apiService.put<HttpResponse<Task>>(`${HttpRoutes.taskApiUrl}`, updatedTask).toPromise()
  }

  deleteTask(guid: string) {
    if (guid === null || guid == "") {
      this.toastr.error("TaskId is leeg")
      return null;
    }
    return this.apiService.delete<HttpResponse<Number>>(`${HttpRoutes.taskApiUrl}/${guid}`).toPromise()
  }

  async getAllProjectTasks(guid: string): Promise<Task[]> {
    if (guid == null) {
      this.toastr.error("ProjectTaskId is leeg")
      return null;
    }
    let projecttasks: Task[] = [];
    await this.apiService.get<HttpResponse<Task[]>>(`${HttpRoutes.taskApiUrl}/GetAllProjectTasks/${guid}`).toPromise().then(response =>
      projecttasks = response.body
    );
    return projecttasks
  }

  addTaskToProject(projectTask: Projecttask) {
    if (projectTask === null) {
      this.toastr.error("ProjectTask is leeg")
      return null;
    }
    return this.apiService.post<HttpResponse<Task>>(`${HttpRoutes.taskApiUrl}/AddTaskToProject`,projectTask).toPromise()
  }
  removeTaskFromProject(projectId:string,taskId:string){
    if(projectId==null||taskId==null){
      this.toastr.error("Ongeldige projectId en/of taskId")
      return null;
    }
    return this.apiService.delete<HttpResponse<string>>(`${HttpRoutes.taskApiUrl}/RemoveTaskFromProject/${projectId}/${taskId}`).toPromise()
  }


}
