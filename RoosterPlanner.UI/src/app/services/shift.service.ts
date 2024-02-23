import {Injectable} from '@angular/core';
import {ApiService} from './api.service';
import {Shift} from '../models/shift';
import {EntityHelper} from '../helpers/entity-helper';
import {HttpResponse} from '@angular/common/http';
import {HttpRoutes} from '../helpers/HttpRoutes';
import {ErrorService} from './error.service';
import {Scheduledata} from '../models/scheduledata';
import {ShiftFilter} from '../models/filters/shift-filter';
import {F} from '@angular/cdk/keycodes';
import {Searchresult} from '../models/searchresult';
import {Shiftdata} from '../models/helper-models/shiftdata';

@Injectable({
  providedIn: 'root'
})
export class ShiftService {

  constructor(private apiService: ApiService,
              private errorService: ErrorService) {
  }

  async getShifts(filter: ShiftFilter): Promise<Searchresult<Shift>> {
    if (!filter || !filter.projectId) {
      this.errorService.error('Ongeldige filter');
      return null;
    }
    let searchResult: Searchresult<Shift> = null;
    await this.apiService.post<HttpResponse<Searchresult<Shift>>>(`${HttpRoutes.shiftApiUrl}/search`, filter)
      .toPromise()
      .then(res => {
        if (res.ok) {
          searchResult = res.body;
          if (searchResult != null) {
            searchResult.resultList.forEach(s => s.date = new Date(s.date));
          }
        }
      }, Error => {
        this.errorService.httpError(Error);
      });

    return searchResult;
  }

  async getShiftData(projectId: string): Promise<Shiftdata> {
    if (!projectId || projectId === EntityHelper.returnEmptyGuid()) {
      this.errorService.error('Ongeldige Id');
      return null;
    }
    let data: Shiftdata = null;
    await this.apiService.get<HttpResponse<Shiftdata>>(`${HttpRoutes.shiftApiUrl}/unique/${projectId}`)
      .toPromise()
      .then(res => {
        if (res.ok) {
          data = res.body;
          if (data.dates && data.dates.length > 0) {
            const dates: Date[] = [];
            data.dates.forEach(d => dates.push(new Date(d)));
            data.dates = dates;
          }
        }
      }, Error => {
        this.errorService.httpError(Error);
      });

    return data;
  }

  async getAllShiftsWithAvailabilities(projectId: string): Promise<Shift[]> {
    if (!projectId) {
      this.errorService.error('ProjectId mag niet leeg zijn');
      return null;
    }
    let shifts: Shift[] = [];
    await this.apiService.get<HttpResponse<Shift[]>>(`${HttpRoutes.shiftApiUrl}/withAvailabilities/${projectId}`)
      .toPromise()
      .then(res => {
        if (res.ok) {
          shifts = res.body;
          if (shifts != null) {
            shifts.forEach(s => s.date = new Date(s.date));
          }
        }
      }, Error => {
        this.errorService.httpError(Error);
      });

    return shifts;
  }

  async getAllShiftsOnDateWithUserAvailability(projectId: string, userId: string, date: Date): Promise<Shift[]> {
    if (!projectId) {
      this.errorService.error('ProjectId mag niet leeg zijn');
      return null;
    }
    if (!userId) {
      this.errorService.error('userId mag niet leeg zijn');
      return null;
    }
    if (!date) {
      this.errorService.error('Datum mag niet leeg zijn');
      return null;
    }
    let shifts: Shift[] = [];
    await this.apiService.get<HttpResponse<Shift[]>>(`${HttpRoutes.shiftApiUrl}/${projectId}/${userId}/${date.toISOString()}`)
      .toPromise()
      .then(res => {
        if (res.ok) {
          shifts = res.body;
          if (shifts != null) {
            shifts.forEach(s => s.date = new Date(s.date));
          }
        }
      }, Error => {
        this.errorService.httpError(Error);
      });

    return shifts;
  }

  async getAllShiftsOnDate(projectId: string, date: Date): Promise<Shift[]> {
    if (!projectId) {
      this.errorService.error('ProjectId mag niet leeg zijn');
      return null;
    }
    if (!date) {
      this.errorService.error('Datum mag niet leeg zijn');
      return null;
    }
    let shifts: Shift[] = [];
    await this.apiService.get<HttpResponse<Shift[]>>(`${HttpRoutes.shiftApiUrl}/${projectId}/${date.toISOString()}`)
      .toPromise()
      .then(res => {
        if (res.ok) {
          shifts = res.body;
          if (shifts != null) {
            shifts.forEach(s => s.date = new Date(s.date));
          }
        }
      }, Error => {
        this.errorService.httpError(Error);
      });

    return shifts;
  }

  async getShift(shiftId: string): Promise<Shift> {
    if (!shiftId) {
      this.errorService.error('ShiftId mag niet leeg zijn');
      return null;
    }
    let shift: Shift = null;
    await this.apiService.get<HttpResponse<Shift>>(`${HttpRoutes.shiftApiUrl}/${shiftId}`)
      .toPromise()
      .then(res => {
        if (res.ok) {
          shift = res.body;
          shift.date != null ? shift.date = new Date(shift.date) : null;
        }
      }, Error => {
        this.errorService.httpError(Error);
      });
    return shift;
  }

  async getScheduleData(shiftId: string): Promise<Scheduledata> {
    if (!shiftId) {
      this.errorService.error('ShiftId mag niet leeg zijn');
      return null;
    }
    let scheduledata: Scheduledata = null;
    await this.apiService.get<HttpResponse<Scheduledata>>(`${HttpRoutes.shiftApiUrl}/schedule/${shiftId}`)
      .toPromise()
      .then(res => {
        if (res.ok) {
          scheduledata = res.body;
        }
      }, Error => {
        this.errorService.httpError(Error);
      });
    return scheduledata;
  }

  async postShifts(shifts: Shift[]): Promise<Shift[]> {
    if (!shifts || !shifts.length) {
      this.errorService.error('Geen shiften om te posten');
      return null;
    }
    shifts.forEach(shift => {
      if (!shift.id) {
        shift.id = EntityHelper.returnEmptyGuid();
      }
      if (!shift.project || !shift.task || !shift.date || !shift.startTime || !shift.endTime || !shift.participantsRequired) {
        this.errorService.error('Niet alle shiften zijn geldig');
        return null;
      }
    });
    let postedShifts: Shift[] = null;
    await this.apiService.post<HttpResponse<Shift[]>>(`${HttpRoutes.shiftApiUrl}`, shifts)
      .toPromise()
      .then(res => {
        if (res.ok) {
          postedShifts = res.body;
        }
      }, Error => {
        this.errorService.httpError(Error);
      });
    return postedShifts;
  }

  async updateShift(shift: Shift): Promise<Shift> {
    if (!shift || !shift.project || !shift.task || !shift.date || !shift.startTime || !shift.endTime || !shift.participantsRequired) {
      this.errorService.error('Kan shift niet updaten: Ongeldig formaat');
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
        this.errorService.httpError(Error);
      });
    return updatedShift;
  }

  async deleteShift(shiftId: string): Promise<boolean> {
    if (!shiftId) {
      this.errorService.error('ShiftId mag niet leeg zijn');
      return null;
    }
    let deleted = false;
    await this.apiService.delete<HttpResponse<Shift>>(`${HttpRoutes.shiftApiUrl}?shiftId=${shiftId}`)
      .toPromise()
      .then(res => {
          if (res.ok) {
            deleted = true;
          }
        }
        , Error => {
          this.errorService.httpError(Error);
        }
      );
    return deleted;
  }

  async GetExportableData(projectId: string): Promise<Shift[]> {
    if (!projectId) {
      this.errorService.error('ShiftId mag niet leeg zijn');
      return null;
    }
    let data: Shift[] = null;
    await this.apiService.get<HttpResponse<Shift[]>>(`${HttpRoutes.shiftApiUrl}/export/${projectId}`)
      .toPromise()
      .then(res => {
          if (res.ok) {
            data = res.body;
          }
        }
        , Error => {
          this.errorService.httpError(Error);
        }
      );
    return data;
  }
}
