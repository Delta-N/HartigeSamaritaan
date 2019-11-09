import { Injectable } from '@angular/core';
import { ApiService } from '../http/api.service';
import { Project } from 'src/app/models/project';
import { Observable } from 'rxjs';
import { HttpResponse } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ProjectService {
  private controllerName = 'projects';

  constructor(private apiService: ApiService) {}

  public createOrUpdateProject(project: Project): Observable<Project> {
    return this.apiService.post(`${this.controllerName}`, project);
  }

  public deleteProject(projectId: string): Observable<HttpResponse<Project>> {
    return this.apiService.delete(`${this.controllerName}/${projectId}`);
  }

  public getProject(projectId: string): Observable<HttpResponse<Project>> {
    return this.apiService.get(`${this.controllerName}/${projectId}`);
  }

  public addPersonToProject(id: string, personId: string): Observable<any> {
    return this.apiService.post(`${this.controllerName}/${id}/addperson/${personId}`, {});
  }

  public getAllProjects(): Observable<Array<Project>> {
    return this.apiService.get(`${this.controllerName}?name=&city=`);
  }
}
