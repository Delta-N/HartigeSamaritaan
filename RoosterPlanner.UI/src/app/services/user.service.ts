import {Injectable} from '@angular/core';
import {HttpResponse} from "@angular/common/http";
import {User} from "../models/user";
import {ApiService} from "./api.service";
import {HttpRoutes} from "../helpers/HttpRoutes";
import {JwtHelper} from "../helpers/jwt-helper";

@Injectable({
  providedIn: 'root'
})
export class UserService {
  user: User = new User('');
  administrators: User[] = [];
  allUsers: User[] = [];

  constructor(private apiService: ApiService) {
  }

  async getUser(guid: string): Promise<User> {
    await this.apiService.get<HttpResponse<User>>(`${HttpRoutes.personApiUrl}/${guid}`).toPromise().then(response => {
      this.user = response.body
    }).catch();
    return this.user
  }

  async getAdministrators(): Promise<User[]> {
    await this.apiService.get<HttpResponse<User[]>>(`${HttpRoutes.personApiUrl}?Userrole=1`).toPromise().then(response => {
      this.administrators = response.body
    }).catch();
    return this.administrators;
  }

  async makeAdmin(GUID: string) {
    await this.apiService.patch<HttpResponse<User>>(`${HttpRoutes.personApiUrl}/modifyadmin/${GUID}/1`).toPromise().then(response => {
    }).catch();
  }

  async removeAdmin(GUID: string) {
    await this.apiService.patch<HttpResponse<User>>(`${HttpRoutes.personApiUrl}/modifyadmin/${GUID}/4`).toPromise().then(response => {
    }).catch();
  }

  async getAllUsers(): Promise<User[]> {
    await this.apiService.get<HttpResponse<User[]>>(`${HttpRoutes.personApiUrl}`).toPromise().then(response => {
      this.allUsers = response.body
    }).catch();
    return this.allUsers;
  }

  userIsAdminFrontEnd(): boolean {
    const idToken = JwtHelper.decodeToken(sessionStorage.getItem('msal.idtoken'))
    if (idToken === null) {
      return false;
    }
    return ((idToken.extension_UserRole === 1) || (idToken.extension_UserRole === 2))
  }

  async updateUser(updateUser: User) {
    await this.apiService.patch<HttpResponse<User>>(`${HttpRoutes.personApiUrl}/`, updateUser).toPromise().then(response => {

    }).catch()
  }
}
