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

  public createOrUpdate(task: Task): Observable<HttpResponse<Task>> {
    return this.apiService.post(`${this.controllerName}`, task);
  }

  public delete(taskId: string): Observable<HttpResponse<Task>> {
    return this.apiService.delete(`${this.controllerName}/${taskId}`);
  }

  public get(taskId: string): Observable<HttpResponse<Task>> {
    return this.apiService.get(`${this.controllerName}/${taskId}`);
  }

  public getAll(): Observable<HttpResponse<Array<Task>>> {
    return this.apiService.get(`${this.controllerName}`);
  }
}
