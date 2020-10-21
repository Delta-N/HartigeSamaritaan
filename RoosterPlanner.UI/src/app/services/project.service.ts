import {Injectable} from '@angular/core';
import {ApiService} from "./api.service";
import {Project} from "../models/project";
import {HttpResponse} from "@angular/common/http";
import {HttpRoutes} from "../helpers/HttpRoutes";

@Injectable({
  providedIn: 'root'
})
export class ProjectService {
  projects: Project[] = [];

  constructor(private apiService: ApiService) {
  }

  async getProject(guid?: string) {
    if (guid == null) {
      //return all projects
      await this.apiService.get<HttpResponse<Project[]>>(`${HttpRoutes.projectApiUrl}`).toPromise().then(response => {
        this.projects = response.body;
      });
    } else {
      await this.apiService.get<HttpResponse<Project[]>>(`${HttpRoutes.projectApiUrl}/${guid}`).toPromise().then(response => {
        this.projects = response.body;
      });
    }
    return this.projects
  }
}