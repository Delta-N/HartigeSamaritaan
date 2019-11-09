import { Injectable } from '@angular/core';
import { ApiService } from '../http/api.service';
import { Observable } from 'rxjs';
import { Task } from '../../models/task.model';
import { HttpResponse } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class TaskService {
  private controllerName = 'tasks';

  constructor(private apiService: ApiService) {}

  public createOrUpdateTask(task: Task): Observable<HttpResponse<Task>> {
    return this.apiService.post(`${this.controllerName}`, task);
  }

  public deleteTask(taskId: string): Observable<HttpResponse<Task>> {
    return this.apiService.delete(`${this.controllerName}/${taskId}`);
  }

  public getTask(taskId: string): Observable<HttpResponse<Task>> {
    return this.apiService.get(`${this.controllerName}/${taskId}`);
  }

  public getAllTasks(): Observable<HttpResponse<Array<Task>>> {
    return this.apiService.get(`${this.controllerName}/all`);
  }
}
