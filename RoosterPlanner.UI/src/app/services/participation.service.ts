import {Injectable} from '@angular/core';
import {ApiService} from "./api.service";
import {ToastrService} from "ngx-toastr";
import {Participation} from "../models/participation";
import {HttpResponse} from "@angular/common/http";
import {HttpRoutes} from "../helpers/HttpRoutes";

@Injectable({
  providedIn: 'root'
})
export class ParticipationService {

  constructor(private apiService: ApiService, private toastr: ToastrService) {
  }

  postParticipation(participation: Participation) {
    if (participation === null) {
      this.toastr.error("Lege participation in participationservice")
      return null;
    }
    if (participation.project === null || participation.person === null||participation.MaxWorkingHoursPerWeek==null) {
      this.toastr.error("Ongeldige participation in participationservice")
      return null;
    }
    if (participation.id === null || participation.id === "") {
      participation.id = "00000000-0000-0000-0000-000000000000";
    }
    if(participation.MaxWorkingHoursPerWeek>40){
      participation.MaxWorkingHoursPerWeek=40;
    }
    return this.apiService.post<HttpResponse<Participation>>(`${HttpRoutes.participationApiUrl}`,participation).toPromise()


  }
}
