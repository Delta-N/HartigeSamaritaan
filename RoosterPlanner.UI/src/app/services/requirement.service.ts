import {Injectable} from '@angular/core';
import {Requirement} from '../models/requirement';
import {HttpResponse} from '@angular/common/http';
import {HttpRoutes} from '../helpers/HttpRoutes';
import {ErrorService} from './error.service';
import {ApiService} from './api.service';
import {EntityHelper} from '../helpers/entity-helper';

@Injectable({
  providedIn: 'root'
})
export class RequirementService {

  constructor(private errorService: ErrorService,
              private apiService: ApiService) {
  }

  async getRequirement(guid: string): Promise<Requirement> {
    if (!guid) {
      this.errorService.error('requirementId mag niet leeg zijn');
      return null;
    }
    let requirement: Requirement = null;
    await this.apiService.get<HttpResponse<Requirement>>(`${HttpRoutes.requirementApiUrl}/${guid}`)
      .toPromise()
      .then(res => {
          if (res.ok) {
            requirement = res.body;
          }
        }
        , Error => {
          this.errorService.httpError(Error);
        });
    return requirement;
  }

  async postRequirement(requirement: Requirement): Promise<Requirement> {
    if (!requirement || !requirement.task || !requirement.certificateType) {
      this.errorService.error('Ongeldig requirement');
      return null;
    }

    if (!requirement.id) {
      requirement.id = EntityHelper.returnEmptyGuid();
    }

    let resRequirement: Requirement = null;
    await this.apiService.post<HttpResponse<Requirement>>(`${HttpRoutes.requirementApiUrl}`, requirement)
      .toPromise()
      .then(res => {
        if (res.ok) {
          resRequirement = res.body;
        }
      }, Error => {
        this.errorService.httpError(Error);
      });
    return resRequirement;
  }

  async updateRequirement(requirement: Requirement): Promise<Requirement> {
    if (!requirement || !requirement.task || !requirement.certificateType || requirement.id == EntityHelper.returnEmptyGuid()) {
      this.errorService.error('Ongeldig requirement');
      return null;
    }
    let resRequirement: Requirement = null;
    await this.apiService.put<HttpResponse<Requirement>>(`${HttpRoutes.requirementApiUrl}`, requirement)
      .toPromise()
      .then(res => {
        if (res.ok) {
          resRequirement = res.body;
        }
      }, Error => {
        this.errorService.httpError(Error);
      });
    return resRequirement;
  }

  async deleteRequirement(guid: string): Promise<boolean> {
    if (!guid) {
      this.errorService.error('RequirementId is leeg');
      return null;
    }
    let result = false;
    await this.apiService.delete<HttpResponse<Requirement>>(`${HttpRoutes.requirementApiUrl}/${guid}`)
      .toPromise()
      .then(res => {
        if (res.ok) {
          result = true;
        }
      }, Error => {
        this.errorService.httpError(Error);
      });
    return result;
  }
}
