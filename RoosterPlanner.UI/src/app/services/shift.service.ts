import {Injectable} from '@angular/core';
import {ApiService} from "./api.service";
import {Shift} from "../models/shift";
import {ToastrService} from "ngx-toastr";
import {EntityHelper} from "../helpers/entity-helper";
import {HttpResponse} from "@angular/common/http";
import {HttpRoutes} from "../helpers/HttpRoutes";

@Injectable({
  providedIn: 'root'
})
export class ShiftService {

  constructor(private apiService: ApiService,
              private toastr: ToastrService) {
  }

  async getAllShifts(projectId: string): Promise<Shift[]> {
    if (!projectId) {
      this.toastr.error("ProjectId mag niet leeg zijn")
      return null;
    }
    let shifts: Shift[] = [];
    await this.apiService.get<HttpResponse<Shift[]>>(`${HttpRoutes.shiftApiUrl}/project/${projectId}`)
      .toPromise()
      .then(res => {
          if (res.status === 200) {
            shifts = res.body
            if (shifts != null) {
              shifts.forEach(s => s.date = new Date(s.date))
            }
          } else {
            this.toastr.error("Fout tijdens het ophalen van shiften bij project " + projectId)
            return null;
          }
        }
      )
    return shifts;
  }

  async getShift(shiftId: string): Promise<Shift> {
    if (!shiftId) {
      this.toastr.error("ShiftId mag niet leeg zijn")
      return null;
    }
    let shift: Shift = null;
    await this.apiService.get<HttpResponse<Shift>>(`${HttpRoutes.shiftApiUrl}/shift/${shiftId}`)
      .toPromise()
      .then(res => {
        if (res.status === 200) {
          shift = res.body
          shift.date = new Date(shift.date);
        } else {
          this.toastr.error("Fout tijdens het ophalen van shift " + shiftId)
          return null;
        }
      })
    return shift;
  }

  async postShifts(shifts: Shift[]): Promise<Shift[]> {
    if (!shifts || !shifts.length) {
      this.toastr.error("Geen shiften om te posten")
      return null;
    }
    shifts.forEach(shift => {
      if (!shift.id) {
        shift.id = EntityHelper.returnEmptyGuid()
      }
      if (!shift.project || !shift.task || !shift.date || !shift.startTime || !shift.endTime || !shift.participantsRequired) {
        this.toastr.error("Niet alle shiften zijn geldig");
        return null;
      }
    })
    let postedShifts: Shift[] = null;
    await this.apiService.post<HttpResponse<Shift[]>>(`${HttpRoutes.shiftApiUrl}`, shifts)
      .toPromise()
      .then(res => {
        if (res.status === 200) {
          postedShifts = res.body
        } else {
          this.toastr.error("Fout tijdens het aanmaken van shiften")
          return null;
        }
      });
    return postedShifts;
  }

  async updateShift(shift: Shift): Promise<Shift> {
    if (!shift || !shift.project || !shift.task || !shift.date || !shift.startTime || !shift.endTime || !shift.participantsRequired) {
      this.toastr.error("Kan shift niet updaten: Ongeldig formaat");
      return null;
    }
    let updatedShift: Shift = null;
    await this.apiService.put<HttpResponse<Shift>>(`${HttpRoutes.shiftApiUrl}`, shift)
      .toPromise()
      .then(res => {
        if (res.status === 200) {
          updatedShift = res.body;
          updatedShift.date = new Date(updatedShift.date)
        } else {
          this.toastr.error("Fout tijdens het updaten van shift: " + shift.id)
          return null;
        }
      })
    return updatedShift;
  }

  async deleteShift(shiftId: string): Promise<boolean> {
    if (!shiftId) {
      this.toastr.error("ShiftId mag niet leeg zijn")
      return null;
    }
    return await this.apiService.delete<HttpResponse<number>>(`${HttpRoutes.shiftApiUrl}?shiftId=${shiftId}`)
      .toPromise()
      .then(res => {
        if (res.status === 200) {
          return true;
        } else {
          this.toastr.error("Fout tijdens het verwijderen van shift " + shiftId)
          return false;
        }
      });
  }

}
