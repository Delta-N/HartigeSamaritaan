import {Injectable} from '@angular/core';
import {HttpResponse} from "@angular/common/http";
import {User} from "../models/user";
import {ApiService} from "./api.service";
import {HttpRoutes} from "../helpers/HttpRoutes";
import {JwtHelper} from "../helpers/jwt-helper";
import {Manager} from "../models/manager";
import {Searchresult} from "../models/searchresult";
import {ErrorService} from "./error.service";

@Injectable({
  providedIn: 'root'
})
export class UserService {
  user: User;
  administrators: User[] = [];
  allUsers: User[] = [];

  constructor(private apiService: ApiService,
              private errorService: ErrorService) {
  }

  async getUser(guid: string | null): Promise<User | null> {
    if (!guid) {
      this.errorService.error("UserId is leeg")
    }
    let user: User | null = null;
    await this.apiService.get<HttpResponse<User>>(`${HttpRoutes.personApiUrl}/${guid}`)
      .toPromise()
      .then(res => {
        if (res?.ok)
          user = res.body
      }, Error => {
        this.errorService.httpError(Error)
      });
    return user
  }

//todo aanpassen optimaal gebruikt te maken van searchresult list
  async getAdministrators(offset: number, pageSize: number): Promise<User[]> {
    let users: User[] | undefined = [];
    await this.apiService.get<HttpResponse<Searchresult<User>>>(`${HttpRoutes.personApiUrl}?Userrole=1&offset=${offset}&pageSize=${pageSize}`)
      .toPromise()
      .then(res => {
        if (res?.ok)
          users = res.body?.resultList
      }, Error => {
        this.errorService.httpError(Error)
      });
    return users;
  }

  async makeAdmin(GUID: string): Promise<User | null> {
    if (!GUID) {
      this.errorService.error("Ongeldige UserId")
      return null;
    }
    let user: User | null = null;
    await this.apiService.put<HttpResponse<User>>(`${HttpRoutes.personApiUrl}/modifyadmin/${GUID}/1`)
      .toPromise()
      .then(res => {
        if (res?.ok)
          user = res.body
      }, Error => {
        this.errorService.httpError(Error)
      });
    return user;
  }

  async removeAdmin(GUID: string): Promise<User | null> {
    if (!GUID) {
      this.errorService.error("Ongeldige UserId")
      return null;
    }
    let user: User | null = null;
    await this.apiService.put<HttpResponse<User>>(`${HttpRoutes.personApiUrl}/modifyadmin/${GUID}/4`)
      .toPromise()
      .then(res => {
        if (res?.ok)
          user = res.body
      }, Error => {
        this.errorService.httpError(Error)
      });
    return user;
  }

//todo om optimaal gebruikt te maken van searchresult list
  async getAllUsers(): Promise<User[]> {
    const resonableLargeNumber = 10000;
    let users: User[] | undefined = [];
    await this.apiService.get<HttpResponse<Searchresult<User>>>(`${HttpRoutes.personApiUrl}?pageSize=${resonableLargeNumber}`)
      .toPromise()
      .then(res => {
        if (res?.ok)
          users = res.body?.resultList
      }, Error => {
        this.errorService.httpError(Error)
      });
    return users;
  }


  async getAllParticipants(projectId: string):Promise<User[]> {
    let users: User[] | null= [] ;
    await this.apiService.get<HttpResponse<User[]>>(`${HttpRoutes.personApiUrl}/participants/${projectId}`)
      .toPromise()
      .then(res => {
        if (res?.ok)
          users = res.body
      }, Error => {
        this.errorService.httpError(Error)
      });
    return users;
  }

  async getAllProjectManagers(projectId: string): Promise<Manager[] | null> {
    if (!projectId) {
      this.errorService.error("ProjectId mag niet leeg zijn")
      return null;
    }
    let managers: Manager[] | null= [];
    await this.apiService.get<HttpResponse<Manager[]>>(`${HttpRoutes.personApiUrl}/managers/${projectId}`)
      .toPromise()
      .then(res => {
        if (res?.ok) {
          managers = res.body
        }
      }, Error => {
        this.errorService.httpError(Error)
      });
    return managers
  }

  async getProjectsManagedBy(userId: string): Promise<Manager[] | null> {
    if (!userId) {
      this.errorService.error("UserId mag niet leeg zijn")
      return null;
    }
    let managers: Manager[] | null= [];
    await this.apiService.get<HttpResponse<Manager[]>>(`${HttpRoutes.personApiUrl}/projectsmanagedby/${userId}`)
      .toPromise()
      .then(res => {
        if (res?.ok) {
          managers = res.body
        }
      }, Error => {
        this.errorService.httpError(Error)
      });
    return managers
  }

  async makeManager(projectId: string, userId: String): Promise<boolean> {
    if (!projectId || !userId) {
      this.errorService.error("UserId of ProjectId mag niet leeg zijn")
      return false;
    }
    let result = false;
    await this.apiService.post<HttpResponse<User>>(`${HttpRoutes.personApiUrl}/makemanager/${projectId}/${userId}`)
      .toPromise()
      .then(res => {
        if (res?.ok) {
          result = true;
        }
      }, Error => {
        this.errorService.httpError(Error)
      });
    return result;
  }

  async removeManager(projectId: string, userId: String): Promise<boolean> {
    if (!projectId || !userId) {
      return false;
    }
    let result = false;

    await this.apiService.delete<HttpResponse<User>>(`${HttpRoutes.personApiUrl}/removemanager/${projectId}/${userId}`)
      .toPromise()
      .then(res => {
        if (res?.ok) {
          result = true;
        }
      }, Error => {
        this.errorService.httpError(Error)
      });
    return result;
  }

//todo
  async getRangeOfUsers(offset: number, pageSize: number): Promise<User[]> {
    let rangeOfUsers: User[] | undefined = []
    await this.apiService.get<HttpResponse<Searchresult<User>>>(`${HttpRoutes.personApiUrl}?offset=${offset}&pageSize=${pageSize}`)
      .toPromise()
      .then(res => {
        if (res?.ok)
          rangeOfUsers = res.body?.resultList
      }, Error => {
        this.errorService.httpError(Error)
      })
    return rangeOfUsers
  }

  async updateUser(updateUser: User): Promise<User | null> {
    if (!updateUser) {
      this.errorService.error("UpdateUser mag niet leeg zijn");
      return null;
    }
    let user: User | null = null;
    await this.apiService.put<HttpResponse<User>>(`${HttpRoutes.personApiUrl}/`, updateUser)
      .toPromise()
      .then(res => {
        if (res?.ok)
          user = res.body
      }, Error => {
        this.errorService.httpError(Error)
      })
    return user
  }

  async updatePerson(updateUser: User): Promise<User | null> {
    if (!updateUser) {
      this.errorService.error("UpdateUser mag niet leeg zijn");
      return null;
    }
    let user: User | null = null;
    await this.apiService.put<HttpResponse<User>>(`${HttpRoutes.personApiUrl}/UpdatePerson`, updateUser)
      .toPromise()
      .then(res => {
        if (res?.ok)
          user = res.body
      }, Error => {
        this.errorService.httpError(Error)
      })
    return user
  }

  async getCurrentUser(): Promise<User | null> {
    let id = this.getCurrentUserId();
    let user: User |null= null;
    if (id) {
      await this.getUser(id).then(res => {
        if (res)
          user = res;
      })
    }
    return user;
  }

  getIdToken(): any {
    const idToken = JwtHelper.decodeToken(sessionStorage.getItem('msal.idtoken'))
    if (idToken === null) {
      return false;
    }
    return idToken;
  }

  getCurrentUserId() {
    return this.getIdToken().oid;
  }

  userIsAdminFrontEnd(): boolean {
    return ((this.getIdToken().extension_UserRole === 1))
  }

  userIsProjectAdminFrontEnd(): boolean {
    const idToken = this.getIdToken()
    return ((idToken.extension_UserRole === 1) || (idToken.extension_UserRole === 2))
  }
}
