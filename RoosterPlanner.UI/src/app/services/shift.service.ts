import {Injectable} from '@angular/core';
import {ApiService} from "./api.service";
import {Shift} from "../models/shift";
import {EntityHelper} from "../helpers/entity-helper";
import {HttpResponse} from "@angular/common/http";
import {HttpRoutes} from "../helpers/HttpRoutes";
import {ErrorService} from "./error.service";

@Injectable({
  providedIn: 'root'
})
export class ShiftService {

  constructor(private apiService: ApiService,
              private errorService: ErrorService) {
  }

  async getAllShifts(projectId: string): Promise<Shift[]> {
    if (!projectId) {
      this.errorService.error("ProjectId mag niet leeg zijn")
      return null;
    }
    let shifts: Shift[] = [];
    await this.apiService.get<HttpResponse<Shift[]>>(`${HttpRoutes.shiftApiUrl}/project/${projectId}`)
      .toPromise()
      .then(res => {
        if (res.ok) {
          shifts = res.body
          if (shifts != null) {
            shifts.forEach(s => s.date = new Date(s.date))
          }
        }
      }, Error => {
        this.errorService.httpError(Error)
      })

    return shifts;
  }

  async getAllShiftsWithAvailabilities(projectId: string): Promise<Shift[]> {
    if (!projectId) {
      this.errorService.error("ProjectId mag niet leeg zijn")
      return null;
    }
    let shifts: Shift[] = [];
    await this.apiService.get<HttpResponse<Shift[]>>(`${HttpRoutes.shiftApiUrl}/withAvailabilities/${projectId}`)
      .toPromise()
      .then(res => {
        if (res.ok) {
          shifts = res.body
          if (shifts != null) {
            shifts.forEach(s => s.date = new Date(s.date))
          }
        }
      }, Error => {
        this.errorService.httpError(Error)
      })

    return shifts;
  }

  async getAllShiftsOnDateWithUserAvailability(projectId: string, userId:string, date:Date): Promise<Shift[]> {
    if (!projectId) {
      this.errorService.error("ProjectId mag niet leeg zijn")
      return null;
    }
    if (!userId) {
      this.errorService.error("userId mag niet leeg zijn")
      return null;
    }
    if (!date) {
      this.errorService.error("Datum mag niet leeg zijn")
      return null;
    }
    let shifts: Shift[] = [];
    await this.apiService.get<HttpResponse<Shift[]>>(`${HttpRoutes.shiftApiUrl}/${projectId}/${userId}/${date.toISOString()}`)
      .toPromise()
      .then(res => {
        if (res.ok) {
          shifts = res.body
          if (shifts != null) {
            shifts.forEach(s => s.date = new Date(s.date))
          }
        }
      }, Error => {
        this.errorService.httpError(Error)
      })

    return shifts;
  }

  async getAllShiftsOnDate(projectId: string,date: Date): Promise<Shift[]> {
    if (!projectId) {
      this.errorService.error("ProjectId mag niet leeg zijn")
      return null;
    }
    if (!date) {
      this.errorService.error("Datum mag niet leeg zijn")
      return null;
    }
    let shifts: Shift[] = [];
    await this.apiService.get<HttpResponse<Shift[]>>(`${HttpRoutes.shiftApiUrl}/${projectId}/${date.toISOString()}`)
      .toPromise()
      .then(res => {
        if (res.ok) {
          shifts = res.body
          if (shifts != null) {
            shifts.forEach(s => s.date = new Date(s.date))
          }
        }
      }, Error => {
        this.errorService.httpError(Error)
      })

    return shifts;
  }

  async getShift(shiftId: string): Promise<Shift> {
    if (!shiftId) {
      this.errorService.error("ShiftId mag niet leeg zijn")
      return null;
    }
    let shift: Shift = null;
    await this.apiService.get<HttpResponse<Shift>>(`${HttpRoutes.shiftApiUrl}/shift/${shiftId}`)
      .toPromise()
      .then(res => {
        if (res.ok) {
          shift = res.body
          shift.date != null ? shift.date = new Date(shift.date) : null;
        }
      }, Error => {
        this.errorService.httpError(Error)
      })
    return shift;
  }

  async getShiftWithAvailabilaties(shiftId: string): Promise<Shift> {
    if (!shiftId) {
      this.errorService.error("ShiftId mag niet leeg zijn")
      return null;
    }
    let shift: Shift = null;
    await this.apiService.get<HttpResponse<Shift>>(`${HttpRoutes.shiftApiUrl}/availabilities/${shiftId}`)
      .toPromise()
      .then(res => {
        if (res.ok) {
          shift = res.body
          shift.date != null ? shift.date = new Date(shift.date) : null;
        }
      }, Error => {
        this.errorService.httpError(Error)
      })
    return shift;
  }

  async postShifts(shifts: Shift[]): Promise<Shift[]> {
    if (!shifts || !shifts.length) {
      this.errorService.error("Geen shiften om te posten")
      return null;
    }
    shifts.forEach(shift => {
      if (!shift.id) {
        shift.id = EntityHelper.returnEmptyGuid()
      }
      if (!shift.project || !shift.task || !shift.date || !shift.startTime || !shift.endTime || !shift.participantsRequired) {
        this.errorService.error("Niet alle shiften zijn geldig");
        return null;
      }
    })
    let postedShifts: Shift[] = null;
    await this.apiService.post<HttpResponse<Shift[]>>(`${HttpRoutes.shiftApiUrl}`, shifts)
      .toPromise()
      .then(res => {
        if (res.ok) {
          postedShifts = res.body
        }
      }, Error => {
        this.errorService.httpError(Error)
      })
    return postedShifts;
  }

  async updateShift(shift: Shift): Promise<Shift> {
    if (!shift || !shift.project || !shift.task || !shift.date || !shift.startTime || !shift.endTime || !shift.participantsRequired) {
      this.errorService.error("Kan shift niet updaten: Ongeldig formaat");
      return null;
    }
    let updatedShift: Shift = null;
    await this.apiService.put<HttpResponse<Shift>>(`${HttpRoutes.shiftApiUrl}`, shift)
      .toPromise()
      .then(res => {
        if (res.ok) {
          updatedShift = res.body;
          updatedShift.date != null ? updatedShift.date = new Date(updatedShift.date) : null;
        }
      }, Error => {
        this.errorService.httpError(Error)
      })
    return updatedShift;
  }

  async deleteShift(shiftId: string): Promise<boolean> {
    if (!shiftId) {
      this.errorService.error("ShiftId mag niet leeg zijn")
      return null;
    }
    let deleted: boolean = false;
    await this.apiService.delete<HttpResponse<Shift>>(`${HttpRoutes.shiftApiUrl}?shiftId=${shiftId}`)
      .toPromise()
      .then(res => {
          if (res.ok) {
            deleted = true;
          }
        }
        , Error => {
          this.errorService.httpError(Error)
        }
      );
    return deleted;
  }
}
