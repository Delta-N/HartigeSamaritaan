import {Injectable} from '@angular/core';
import {ApiService} from './api.service';
import {Participation} from '../models/participation';
import {HttpResponse} from '@angular/common/http';
import {HttpRoutes} from '../helpers/HttpRoutes';
import {DateConverter} from '../helpers/date-converter';
import {EntityHelper} from '../helpers/entity-helper';
import {ErrorService} from './error.service';

@Injectable({
  providedIn: 'root'
})
export class ParticipationService {

  constructor(private apiService: ApiService,
              private errorService: ErrorService) {
  }

  async getParticipations(userId: string): Promise<Participation[]> {
    if (!userId) {
      this.errorService.error('UserId mag niet leeg zijn');
      return null;
    }
    let participations: Participation[] = [];
    await this.apiService.get<HttpResponse<Participation[]>>(`${HttpRoutes.participationApiUrl}/${userId}`)
      .toPromise()
      .then(res => {
        if (res.ok) {
          participations = res.body;
        }
      }, Error => {
        this.errorService.httpError(Error);
      });
    return participations;
  }

  async getParticipation(userId: string, projectId: string): Promise<Participation> {
    if (!userId || !projectId) {
      this.errorService.error('UserId en/of ProjectId mag niet leeg zijn');
      return null;
    }
    let participation: Participation = null;
    await this.apiService.get<HttpResponse<Participation>>(`${HttpRoutes.participationApiUrl}/GetParticipation/${userId}/${projectId}`)
      .toPromise()
      .then(res => {
        if (res.ok) {
          participation = res.body;
        }
      }, Error => {
        this.errorService.httpError(Error);
      });
    return participation;
  }

  async getAllParticipations(projectId: string): Promise<Participation[]>{
    if (!projectId) {
      this.errorService.error('projectId mag niet leeg zijn');
      return null;
    }
    let participations: Participation[] = [];
    await this.apiService.get<HttpResponse<Participation[]>>(`${HttpRoutes.participationApiUrl}/project/${projectId}`)
      .toPromise()
      .then(res => {
        if (res.ok) {
          participations = res.body;
        }
      }, Error => {
        this.errorService.httpError(Error);
      });
    return participations;
  }

  async postParticipation(participation: Participation): Promise<Participation> {
    if (!participation) {
      this.errorService.error('Lege participation in participationservice');
      return null;
    }
    if (!participation.project || !participation.person) {
      this.errorService.error('Ongeldige participation in participationservice');
      return null;
    }

    if (!participation.id) {
      participation.id = EntityHelper.returnEmptyGuid();
    }
    if (participation.maxWorkingHoursPerWeek > 40) {
      participation.maxWorkingHoursPerWeek = 40;
    }
    let postedParticipation: Participation = null;
    await this.apiService.post<HttpResponse<Participation>>(`${HttpRoutes.participationApiUrl}`, participation)
      .toPromise()
      .then(res => {
        if (res.ok) {
          postedParticipation = res.body;
        }
      }, Error => {
        this.errorService.httpError(Error);
      });
    return postedParticipation;
  }

  async updateParticipation(participation: Participation): Promise<Participation> {
    if (!participation) {
      this.errorService.error('Lege participation in participation service');
      return null;
    }
    if (!participation.project.id) {
      this.errorService.error('ProjectId is leeg');
      return null;
    }
    if (!participation.person.id) {
      this.errorService.error('PersonId is leeg');
      return null;
    }
    try {
      if (participation.project.participationEndDate) {
        participation.project.participationEndDate = DateConverter.toDate(participation.project.participationEndDate);
      }
      participation.project.participationStartDate = DateConverter.toDate(participation.project.participationStartDate);
      participation.project.projectEndDate = DateConverter.toDate(participation.project.projectEndDate);
      participation.project.projectStartDate = DateConverter.toDate(participation.project.projectStartDate);
    } catch (e) {
      this.errorService.error(e);
    }
    let updatedParticipation = null;
    await this.apiService.put<HttpResponse<Participation>>(`${HttpRoutes.participationApiUrl}`, participation)
      .toPromise()
      .then(res => {
        if (res.ok) {
          updatedParticipation = res.body;
        }
      }, Error => {
        this.errorService.httpError(Error);
      });
    return updatedParticipation;
  }

  async deleteParticipation(participation: Participation): Promise<boolean> {
    if (participation === null) {
      this.errorService.error('Fout tijdens het uitschrijven bij een project');
      return null;
    }
    let deleted = false;
    await this.apiService.delete<HttpResponse<Participation>>(`${HttpRoutes.participationApiUrl}/${participation.id}`)
      .toPromise()
      .then(res => {
        if (res.ok) {
          deleted = true;
        }
      }, Error => {
        this.errorService.httpError(Error);
      });
    return deleted;
  }
}
