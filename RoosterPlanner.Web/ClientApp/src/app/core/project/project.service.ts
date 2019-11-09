import { Injectable } from '@angular/core';
import { ApiService } from '../http/api.service';
import { Observable } from 'rxjs';
import { HttpResponse } from '@angular/common/http';
import { Project } from '../../models/project.model';

@Injectable({
  providedIn: 'root'
})
export class ProjectService {
  private controllerName = 'projects';

  constructor(private apiService: ApiService) {}

  public createOrUpdateProject(project: Project): Observable<HttpResponse<Project>> {
    return this.apiService.post(`${this.controllerName}`, project);
  }

  public deleteProject(projectId: string): Observable<HttpResponse<Project>> {
    return this.apiService.delete(`${this.controllerName}/${projectId}`);
  }

  public getProject(projectId: string): Observable<HttpResponse<Project>> {
    return this.apiService.get(`${this.controllerName}/${projectId}`);
  }

  public getAllProjects(): Observable<HttpResponse<Array<Project>>> {
    return this.apiService.get(`${this.controllerName}?name=&city=`);
  }
}
