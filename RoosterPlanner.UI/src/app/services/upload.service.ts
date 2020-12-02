import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {ApiService} from "./api.service";
import {HttpRoutes} from "../helpers/HttpRoutes";
import {Uploadresult} from "../models/uploadresult";

@Injectable({providedIn: 'root'})
export class UploadService {

  constructor(private http: HttpClient,
              private apiService: ApiService) {
  }

  async uploadInstruction(formData: FormData): Promise<Uploadresult> {
    let uploadResult: Uploadresult = new Uploadresult();
    await this.apiService.postMultiPartFormData<Uploadresult>(`${HttpRoutes.uploadApiUrl}`, formData)
      .toPromise().then(res => {
        uploadResult = res;
      });
    return uploadResult
  }

  async uploadProjectPicture(formData: FormData): Promise<Uploadresult> {
    let uploadResult: Uploadresult = new Uploadresult();
    await this.apiService.postMultiPartFormData<Uploadresult>(`${HttpRoutes.uploadApiUrl}/UploadProjectPicture`, formData)
      .toPromise().then(res => {
        uploadResult = res;
      });
    return uploadResult
  }

  async uploadProfilePicture(formData: FormData): Promise<Uploadresult> {
    let uploadResult: Uploadresult = new Uploadresult();
    await this.apiService.postMultiPartFormData<Uploadresult>(`${HttpRoutes.uploadApiUrl}/UploadProfilePicture`, formData)
      .toPromise().then(res => {
        uploadResult = res;
      });
    return uploadResult
  }

  async deleteIfExists(url: string): Promise<Uploadresult> {
    let uploadResult: Uploadresult = new Uploadresult();

    await this.apiService.delete<Uploadresult>(`${HttpRoutes.uploadApiUrl}?Url=${url}`).toPromise().then(res => {
      uploadResult = res;
    });
    return uploadResult;
  }
}
