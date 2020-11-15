import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {ApiService} from "./api.service";
import {HttpRoutes} from "../helpers/HttpRoutes";

@Injectable({providedIn: 'root'})
export class UploadService {

  constructor(private http: HttpClient,
              private apiService: ApiService) {
  }

  uploadInstruction(formData: FormData):Promise<any> {
    return this.apiService.postMultiPartFormData(`${HttpRoutes.uploadApiUrl}`, formData).toPromise();
  }
  deleteIfExists(url:string){
    return this.apiService.delete(`${HttpRoutes.uploadApiUrl}?Url=${url}`).toPromise();
  }
}
