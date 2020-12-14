import { Injectable } from '@angular/core';
import {HttpResponse} from "@angular/common/http";
import {HttpRoutes} from "../helpers/HttpRoutes";
import {ErrorService} from "./error.service";
import {ApiService} from "./api.service";
import {AvailabilityData} from "../models/availabilitydata";
import {EntityHelper} from "../helpers/entity-helper";
import {Availability} from "../models/availability";

@Injectable({
  providedIn: 'root'
})
export class AvailabilityService {

  constructor(private errorService:ErrorService,
              private apiService:ApiService) { }

  async getAvailabilityData(projectId: string, userId:string): Promise<AvailabilityData> {
    if (!projectId) {
      this.errorService.error("projectId mag niet leeg zijn")
      return null;
    }
    if (!userId) {
      this.errorService.error("userId mag niet leeg zijn")
      return null;
    }
    let availabilityDate: AvailabilityData = null;
    await this.apiService.get<HttpResponse<AvailabilityData>>(`${HttpRoutes.availabilityApiUrl}/find/${projectId}/${userId}`)
      .toPromise()
      .then(res => {
        if (res.ok)
          availabilityDate = res.body;
      }, Error => {
        this.errorService.httpError(Error)
      })
    return availabilityDate;
  }

  async postAvailability(availability: Availability): Promise<Availability> {
    if (!availability || !availability.participation || !availability.shift || availability.type ==undefined) {
      this.errorService.error("Ongeldige availability")
      return null;
    }

    if (!availability.id) {
      availability.id = EntityHelper.returnEmptyGuid()
    }
    let resAvailability: Availability = null;
    await this.apiService.post<HttpResponse<Availability>>(`${HttpRoutes.availabilityApiUrl}`, availability)
      .toPromise()
      .then(res => {
        if (res.ok)
          resAvailability = res.body
      }, Error => {
        this.errorService.httpError(Error)
      })
    return resAvailability
  }

  async updateAvailability(availability: Availability): Promise<Availability> {
    if (!availability || !availability.participationId || !availability.shiftId || availability.type==undefined) {
      this.errorService.error("Ongeldige availability")
      return null;
    }
    let updatedAvailability: Availability = null;
    if (!availability.id) {
      this.errorService.error("availabilityId is leeg")
      return null;
    }
    await this.apiService.put<HttpResponse<Availability>>(`${HttpRoutes.availabilityApiUrl}`, availability)
      .toPromise()
      .then(res => {
          if (res.ok)
            updatedAvailability = res.body;
        }, Error => {
          this.errorService.httpError(Error)
        }
      )
    return updatedAvailability
  }
}
