import {Injectable} from '@angular/core';
import {ApiService} from "./api.service";
import {ToastrService} from "ngx-toastr";
import {Participation} from "../models/participation";
import {HttpResponse} from "@angular/common/http";
import {HttpRoutes} from "../helpers/HttpRoutes";
import {DateConverter} from "../helpers/date-converter";

@Injectable({
  providedIn: 'root'
})
export class ParticipationService {
  participations: Participation[] = []
  participation: Participation;


  constructor(private apiService: ApiService, private toastr: ToastrService) {
  }

  async getParticipations(userGuid: string): Promise<Participation[]> {
    await this.apiService.get<HttpResponse<Participation[]>>(`${HttpRoutes.participationApiUrl}?personGuid=${userGuid}`).toPromise().then(response => {
      this.participations = response.body;
    }, Error => {
      //hoe worden errors traditioneel gelogt?
    })
    return this.participations
  }

  async getParticipation(userGuid: string, projectGuid) {
    await this.apiService.get<HttpResponse<Participation>>(`${HttpRoutes.participationApiUrl}/GetParticipation/${userGuid}/${projectGuid}`).toPromise().then(response => {
      this.participation = response.body;
    }, Error => {
    })
    return this.participation
  }

  postParticipation(participation: Participation) {
    if (participation === null) {
      this.toastr.error("Lege participation in participationservice")
      return null;
    }
    if (participation.project === null || participation.person === null) {
      this.toastr.error("Ongeldige participation in participationservice")
      return null;
    }

    if (participation.id === null || participation.id === "") {
      participation.id = "00000000-0000-0000-0000-000000000000";
    }
    if (participation.maxWorkingHoursPerWeek > 40) {
      participation.maxWorkingHoursPerWeek = 40;
    }
    return this.apiService.post<HttpResponse<Participation>>(`${HttpRoutes.participationApiUrl}`, participation).toPromise()
  }

  deleteParticipation(participation: Participation) {
    if (participation === null) {
      this.toastr.error("Fout tijdens het uitschrijven bij een projcet")
    }
    return this.apiService.delete<HttpResponse<Number>>(`${HttpRoutes.participationApiUrl}?id=${participation.id}`).toPromise().then();
  }

  updateParticipation(participation: Participation) {
    if (participation === null) {
      this.toastr.error("Lege participation in participation service")
      return;
    }
    if (participation.project.id === null || participation.project.id === "") {
      this.toastr.error("ProjectId is leeg")
      return;
    }
    if (participation.person.id === null || participation.person.id === "") {
      this.toastr.error("PersonId is leeg")
      return;
    }
    if (participation.project.endDate !== null) {
      if (participation.project.endDate === undefined || participation.project.endDate.toString() === "") {
        participation.project.endDate = null;
      } else {
        participation.project.endDate = DateConverter.toDate(participation.project.endDate);
      }
    }
    if (participation.project.startDate.toString() !== "") {
      try {
        participation.project.startDate = DateConverter.toDate(participation.project.startDate);
      } catch (e) {
        this.toastr.error(e)
        participation.project.startDate = null;
      }
    }
    return this.apiService.patch<HttpResponse<Participation>>(`${HttpRoutes.participationApiUrl}`, participation).toPromise()
  }
}
