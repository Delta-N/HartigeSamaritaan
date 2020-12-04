import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {ApiService} from "./api.service";
import {HttpRoutes} from "../helpers/HttpRoutes";
import {Uploadresult} from "../models/uploadresult";
import {ErrorService} from "./error.service";

@Injectable({providedIn: 'root'})
export class UploadService {

  constructor(private http: HttpClient,
              private apiService: ApiService,
              private errorService: ErrorService) {
  }

  async uploadInstruction(formData: FormData): Promise<Uploadresult> {
    if (!formData) {
      this.errorService.error("Instuctie-bestand is leeg")
    }
    let uploadResult: Uploadresult = new Uploadresult();
    await this.apiService.postMultiPartFormData<Uploadresult>(`${HttpRoutes.uploadApiUrl}`, formData)
      .toPromise()
      .then(res => {
        if (res.succeeded)
          uploadResult = res;
      });
    return uploadResult
  }

  async uploadProjectPicture(formData: FormData): Promise<Uploadresult> {
    if (!formData) {
      this.errorService.error("Projectfoto-bestand is leeg")
    }
    let uploadResult: Uploadresult = new Uploadresult();
    await this.apiService.postMultiPartFormData<Uploadresult>(`${HttpRoutes.uploadApiUrl}/UploadProjectPicture`, formData)
      .toPromise()
      .then(res => {
        if (res.succeeded)
          uploadResult = res;
      });
    return uploadResult
  }

  async uploadProfilePicture(formData: FormData): Promise<Uploadresult> {
    if (!formData) {
      this.errorService.error("Profielfoto-bestand is leeg")
    }
    let uploadResult: Uploadresult = new Uploadresult();
    await this.apiService.postMultiPartFormData<Uploadresult>(`${HttpRoutes.uploadApiUrl}/UploadProfilePicture`, formData)
      .toPromise()
      .then(res => {
        if (res.succeeded)
          uploadResult = res;
      });
    return uploadResult
  }

  async deleteIfExists(url: string): Promise<Uploadresult> {
    if (!url) {
      this.errorService.error("url is leeg")
    }
    let uploadResult: Uploadresult = new Uploadresult();

    await this.apiService.delete<Uploadresult>(`${HttpRoutes.uploadApiUrl}?Url=${url}`)
      .toPromise()
      .then(res => {
        uploadResult = res;
      });
    return uploadResult;
  }
}
