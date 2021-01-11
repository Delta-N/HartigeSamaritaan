import {Injectable} from '@angular/core';
import {ApiService} from "./api.service";
import {ErrorService} from "./error.service";
import {HttpResponse} from "@angular/common/http";
import {HttpRoutes} from "../helpers/HttpRoutes";

@Injectable({
  providedIn: 'root'
})
export class EmailService {

  constructor(private apiService: ApiService,
              private errorService: ErrorService) {
  }

  async requestAvailability(projectId: string) {
    if (!projectId) {
      this.errorService.error("ProjectId mag niet leeg zijn")
      return null;
    }
    let result = null
    await this.apiService.post<HttpResponse<boolean>>(`${HttpRoutes.participationApiUrl}/availability/${projectId}`)
      .toPromise()
      .then(res => {
        result = res
      })
    return result
  }

  async sendSchedule(projectId: string) {
    if (!projectId) {
      this.errorService.error("ProjectId mag niet leeg zijn")
      return null;
    }
    let result = null
    await this.apiService.post<HttpResponse<boolean>>(`${HttpRoutes.participationApiUrl}/Schedule/${projectId}`)
      .toPromise()
      .then(res => {
        result = res
      })
    return result
  }
}
