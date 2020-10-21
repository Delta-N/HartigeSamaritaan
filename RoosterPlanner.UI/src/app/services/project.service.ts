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

  postProject(project: Project) {
    if(project==null){
      window.alert("Leeg project in project service");
      return;
    }
    if(project.id==null || project.id==""){
      project.id="00000000-0000-0000-0000-000000000000"
    }
    if(project.endDate!=null && project.endDate.toString() == "" ){
      project.endDate=null;
    }
    if(project.startDate.toString()!=""){
      try{
        project.startDate=new Date(project.startDate);
      }catch (e) {
        console.error(e)
        project.startDate=null;
      }
    }
    return this.apiService.post<HttpResponse<Project>>(`${HttpRoutes.projectApiUrl}`,project).toPromise();
  }
}
