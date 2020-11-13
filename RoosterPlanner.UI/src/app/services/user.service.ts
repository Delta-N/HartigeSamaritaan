import {Injectable} from '@angular/core';
import {HttpResponse} from "@angular/common/http";
import {User} from "../models/user";
import {ApiService} from "./api.service";
import {HttpRoutes} from "../helpers/HttpRoutes";
import {JwtHelper} from "../helpers/jwt-helper";
import {ToastrService} from "ngx-toastr";

@Injectable({
  providedIn: 'root'
})
export class UserService {
  user: User;
  administrators: User[] = [];
  allUsers: User[] = [];

  constructor(private apiService: ApiService, private toastr: ToastrService) {
  }

  async getUser(guid: string): Promise<User> {
    await this.apiService.get<HttpResponse<User>>(`${HttpRoutes.personApiUrl}/${guid}`).toPromise().then(response => {
      this.user = response.body
    }, Error => {
      if (Error.status == 401) {
        this.toastr.error("U bent ongeauthoriseerd");
        return null;
      }
    });
    return this.user
  }

  async getAdministrators(offset: number, pageSize: number): Promise<User[]> {
    await this.apiService.get<HttpResponse<User[]>>(`${HttpRoutes.personApiUrl}?Userrole=1&offset=${offset}&pageSize=${pageSize}`).toPromise().then(response => {
      this.administrators = response.body
    }).catch();
    return this.administrators;
  }

  async makeAdmin(GUID: string) {
    await this.apiService.put<HttpResponse<User>>(`${HttpRoutes.personApiUrl}/modifyadmin/${GUID}/1`).toPromise().then(response => {
    }).catch();
  }

  async removeAdmin(GUID: string) {
    await this.apiService.put<HttpResponse<User>>(`${HttpRoutes.personApiUrl}/modifyadmin/${GUID}/4`).toPromise().then(response => {
    }).catch();
  }

  async getAllUsers(): Promise<User[]> {
    await this.apiService.get<HttpResponse<User[]>>(`${HttpRoutes.personApiUrl}`).toPromise().then(response => {
      this.allUsers = response.body
    }).catch();
    return this.allUsers;
  }

  async getRangeOfUsers(offset:number, pageSize:number):Promise<User[]>{
    let rangeOfUsers:User[]=[]
    await this.apiService.get<HttpResponse<User[]>>(`${HttpRoutes.personApiUrl}?offset=${offset}&pageSize=${pageSize}`).toPromise().then(response => {
      rangeOfUsers=response.body
    })
    return rangeOfUsers
  }

  userIsAdminFrontEnd(): boolean {
    const idToken = JwtHelper.decodeToken(sessionStorage.getItem('msal.idtoken'))
    if (idToken === null) {
      return false;
    }
    return ((idToken.extension_UserRole === 1) || (idToken.extension_UserRole === 2))
  }

  async updateUser(updateUser: User): Promise<User> {
    let user: User = new User();
    await this.apiService.put<HttpResponse<User>>(`${HttpRoutes.personApiUrl}/`, updateUser).toPromise().then(response => {
      user = response.body
    })
    return user
  }

  async getCurrentUser() {
    let id = this.getCurrentUserId();
    if (this.getCurrentUserId() != false) {
      return this.getUser(id);
    }
    return false;
  }

  getCurrentUserId() {
    const idToken = JwtHelper.decodeToken(sessionStorage.getItem('msal.idtoken'))
    if (idToken === null) {
      return false;
    }
    return idToken.oid;
  }
}
