import {Injectable} from '@angular/core';
import {ApiService} from "./api.service";
import {Project} from "../models/project";
import {HttpResponse} from "@angular/common/http";
import {HttpRoutes} from "../helpers/HttpRoutes";
import {DateConverter} from "../helpers/date-converter";
import {EntityHelper} from "../helpers/entity-helper";
import {Searchresult} from "../models/searchresult";
import {ErrorService} from "./error.service";

@Injectable({
  providedIn: 'root'
})
export class ProjectService {

  constructor(private apiService: ApiService,
              private errorService: ErrorService) {
  }

  async getProject(guid: string | null): Promise<Project | null> {
    if (!guid) {
      this.errorService.error("ProjectId mag niet leeg zijn")
      return null;
    }
    let project: Project | null = null;
    await this.apiService.get<HttpResponse<Project>>(`${HttpRoutes.projectApiUrl}/${guid}`)
      .toPromise()
      .then(res => {
        if (res?.ok)
          project = res.body
      }, Error => {
        this.errorService.httpError(Error)
      });
    return project;
  }

  //todo aanpassen
  async getAllProjects(offset: number, pageSize: number): Promise<Project[]> {
    let projects: Project[] | undefined= [];
    await this.apiService.get<HttpResponse<Searchresult<Project>>>(`${HttpRoutes.projectApiUrl}?offset=${offset}&pageSize=${pageSize}`)
      .toPromise()
      .then(res => {
          if (res?.ok)
            projects = res.body?.resultList
        }, Error => {
          this.errorService.httpError(Error)
        }
      );
    return projects
  }

  //todo deze aanpassen
  async getActiveProjects(): Promise<Project[]> {
    let projects: Project[] | undefined = [];
    await this.apiService.get<HttpResponse<Searchresult<Project>>>(`${HttpRoutes.projectApiUrl}?closed=false&endDate=${new Date().toISOString()}`)
      .toPromise()
      .then(res => {
          if (res?.ok)
            projects = res.body?.resultList
        }, Error => {
          this.errorService.httpError(Error)
        }
      );
    return projects
  }

  async postProject(project: Project): Promise<Project | null> {
    if (!project) {
      this.errorService.error("Leeg project in project service")
      return null;
    }
    if (!project.id) {
      project.id = EntityHelper.returnEmptyGuid()
    }
    try {
      if (project.participationEndDate) {
        project.participationEndDate = DateConverter.toDate(project.participationEndDate);
      }
      project.participationStartDate = DateConverter.toDate(project.participationStartDate);
      project.projectEndDate = DateConverter.toDate(project.projectEndDate);
      project.projectStartDate = DateConverter.toDate(project.projectStartDate);
    } catch (e:any) {
      this.errorService.error(e)
    }
    let postedProject : Project | null  = null;
    await this.apiService.post<HttpResponse<Project>>(`${HttpRoutes.projectApiUrl}`, project)
      .toPromise()
      .then(res => {
        if (res?.ok) {
          postedProject = res.body
        }
      }, Error => {
        this.errorService.httpError(Error)
      })
    return postedProject;
  }

  async updateProject(project: Project): Promise<Project | null> {
    if (!project) {
      this.errorService.error("Leeg project in project service")
      return null;
    }
    if (!project.id) {
      this.errorService.error("ProjectId is leeg")
      return null;
    }
    try {
      if (project.participationEndDate) {
        project.participationEndDate = DateConverter.toDate(project.participationEndDate);
      }
      project.participationStartDate = DateConverter.toDate(project.participationStartDate);
      project.projectEndDate = DateConverter.toDate(project.projectEndDate);
      project.projectStartDate = DateConverter.toDate(project.projectStartDate);
    } catch (e: any) {
      this.errorService.error(e)
    }
    let updatedProject: Project | null = null;
    await this.apiService.put<HttpResponse<Project>>(`${HttpRoutes.projectApiUrl}`, project)
      .toPromise()
      .then(res => {
        if (res?.ok) {
          updatedProject = res.body;
        }
      }, Error => {
        this.errorService.httpError(Error)
      })
    return updatedProject;
  }
}
