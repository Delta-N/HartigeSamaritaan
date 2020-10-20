import {Injectable} from '@angular/core';
import {HttpResponse} from "@angular/common/http";
import {User} from "../models/user";
import {AuthenticationService} from "./authentication.service";
import {ApiService} from "./api.service";
import {HttpRoutes} from "../helpers/HttpRoutes";
import {JwtHelper} from "../helpers/jwt-helper";

@Injectable({
  providedIn: 'root'
})
export class UserService {
  user: User = new User('');

  constructor(private apiService: ApiService) {
  }

  async getUser(guid: string): Promise<User> {
    await this.apiService.get<HttpResponse<User>>(`${HttpRoutes.personApiUrl}/${guid}`).toPromise().then(response => {
      this.user = response.body
    }).catch();
    return this.user
  }

  //todo deze variablen zijn hardcoded
  userIsAdminBackend(user: User): boolean {
    if (user == null || user.userRole == null) {
      return false;
    }
    if (user.userRole == 'Boardmember' || user.userRole == 'Committeemember') {
      return true;
    }
    return false;
  }

  userIsAdminFrontEnd(): boolean {
    const idToken = JwtHelper.decodeToken(sessionStorage.getItem('msal.idtoken'))
    if (idToken == null) {
      return false;
    }
    return ((idToken.extension_UserRole == 1) || (idToken.extension_UserRole == 2))
  }
}
