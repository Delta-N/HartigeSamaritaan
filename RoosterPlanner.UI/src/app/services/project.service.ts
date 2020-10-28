import {Injectable} from '@angular/core';
import {ApiService} from "./api.service";
import {Project} from "../models/project";
import {HttpResponse} from "@angular/common/http";
import {HttpRoutes} from "../helpers/HttpRoutes";
import {DateConverter} from "../helpers/date-converter";
import {ToastrService} from "ngx-toastr";

@Injectable({
  providedIn: 'root'
})
export class ProjectService {
  projects: Project[] = [];
  project: any;

  constructor(private apiService: ApiService, private toastr: ToastrService) {
  }

  async getProject(guid?: string): Promise<Project[]> {
    if (guid == null) {
      //return all projects
      await this.apiService.get<HttpResponse<Project[]>>(`${HttpRoutes.projectApiUrl}`).toPromise().then(response => {
        this.projects=[]
        this.projects = response.body;
      });
    } else {
      await this.apiService.get<HttpResponse<Project[]>>(`${HttpRoutes.projectApiUrl}/${guid}`).toPromise().then(response => {
        this.projects=[]
        this.project = response.body
        this.projects.push(this.project)
      });
    }
    return this.projects
  }

  postProject(project: Project) {
    if (project === null) {
      this.toastr.error("Leeg project in project service")
      return;
    }
    if (project.id === null || project.id === "") {
      project.id = "00000000-0000-0000-0000-000000000000"
    }
    if (project.endDate !== null) {
      if (project.endDate.toString() === "") {
        project.endDate = null;
      } else {
        project.endDate = DateConverter.toDate(project.endDate);
      }
    }

    if (project.startDate.toString() !== "") {
      try {

        project.startDate = DateConverter.toDate(project.startDate);
      } catch (e) {
        console.error(e)
        project.startDate = null;
      }
    }
    return this.apiService.post<HttpResponse<Project>>(`${HttpRoutes.projectApiUrl}`, project).toPromise();
  }

  patchProject(project: Project) {
    if (project === null) {
      this.toastr.error("Leeg project in project service")
      return;
    }
    if (project.id === null || project.id === "") {
      this.toastr.error("ProjectId is leeg")
      return;
    }
    if (project.endDate !== null) {
      if (project.endDate.toString() === "") {
        project.endDate = null;
      } else {
        project.endDate = DateConverter.toDate(project.endDate);
      }

      if (project.startDate.toString() !== "") {
        try {

          project.startDate = DateConverter.toDate(project.startDate);
        } catch (e) {
          console.error(e)
          project.startDate = null;
        }
      }
      return this.apiService.patch<HttpResponse<Project>>(`${HttpRoutes.projectApiUrl}`, project).toPromise();
    }
  }
}
