import {Injectable} from '@angular/core';
import {ApiService} from "./api.service";
import {Project} from "../models/project";
import {HttpResponse} from "@angular/common/http";
import {HttpRoutes} from "../helpers/HttpRoutes";
import {DateConverter} from "../helpers/date-converter";
import {ToastrService} from "ngx-toastr";
import {EntityHelper} from "../helpers/entity-helper";

@Injectable({
  providedIn: 'root'
})
export class ProjectService {
  projects: Project[] = [];
  project: any;

  constructor(private apiService: ApiService,
              private toastr: ToastrService) {
  }

  async getProject(guid: string): Promise<Project> {
      await this.apiService.get<HttpResponse<Project[]>>(`${HttpRoutes.projectApiUrl}/${guid}`).toPromise().then(response => {
        this.project = response.body
        this.projects.push(this.project)
      });
    return this.project
  }

  async getAllProjects(offset:number,pageSize:number): Promise<Project[]>{
    this.projects = []
      await this.apiService.get<HttpResponse<Project[]>>(`${HttpRoutes.projectApiUrl}?offset=${offset}&pageSize=${pageSize}`).toPromise().then(response => {
        this.projects = response.body;
      });
    return this.projects
  }

  async getActiveProjects(): Promise<Project[]> {
    let currentDate = new Date().toISOString();
    this.projects = []
    await this.apiService.get<HttpResponse<Project[]>>(`${HttpRoutes.projectApiUrl}?closed=false&endDate=${currentDate}`).toPromise().then(response => {
      this.projects = response.body;
    })

    return this.projects
  }

  postProject(project: Project) {
    if (project === null) {
      this.toastr.error("Leeg project in project service")
      return null;
    }
    if (project.id === null || project.id === "") {
      project.id = EntityHelper.returnEmptyGuid()
    }
    if (project.endDate !== null) {
      if (project.endDate === undefined || project.endDate.toString() === "") {
        project.endDate = null;
      } else {
        project.endDate = DateConverter.toDate(project.endDate);
      }
    }

    if (project.startDate.toString() !== "") {
      try {

        project.startDate = DateConverter.toDate(project.startDate);
      } catch (e) {
        this.toastr.error(e)
        project.startDate = null;
      }
    }
    return this.apiService.post<HttpResponse<Project>>(`${HttpRoutes.projectApiUrl}`, project).toPromise();
  }

  updateProject(project: Project) {
    if (project === null) {
      this.toastr.error("Leeg project in project service")
      return;
    }
    if (project.id === null || project.id === "") {
      this.toastr.error("ProjectId is leeg")
      return;
    }
    if (project.endDate !== null) {
      if (project.endDate === undefined || project.endDate.toString() === "") {
        project.endDate = null;
      } else {
        project.endDate = DateConverter.toDate(project.endDate);
      }
    }

    if (project.startDate.toString() !== "") {
      try {
        project.startDate = DateConverter.toDate(project.startDate);
      } catch (e) {
        this.toastr.error(e)
        project.startDate = null;
      }
    }
    return this.apiService.put<HttpResponse<Project>>(`${HttpRoutes.projectApiUrl}`, project).toPromise()
  }
}
