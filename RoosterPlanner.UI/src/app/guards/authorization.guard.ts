import {inject, Injectable} from "@angular/core";
import {
  ActivatedRouteSnapshot,
  CanActivate, CanActivateFn,
  CanLoad,
  Route,
  Router,
  RouterStateSnapshot,
  UrlSegment,
  UrlTree,
} from "@angular/router";
import {UserService} from "../services/user.service";
import {Observable, of} from "rxjs";


@Injectable({providedIn: 'root'})
export class AuthorizationGuard implements CanActivate, CanLoad {

  constructor(private router: Router,
              private userService: UserService) {
  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | Observable<boolean> {
    if (this.userService.userIsAdminFrontEnd())
      return true
    else {
      this.router.navigate(['home'])
      return false;
    }
  }

  canLoad(route: Route, segments: UrlSegment[]): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    if (this.userService.userIsAdminFrontEnd())
      return true
    else {
      this.router.navigate(['home'])
      return false;
    }
  }
}

export const authCanActivateGuard: CanActivateFn = (route: ActivatedRouteSnapshot, state: RouterStateSnapshot) => {
  if (inject(UserService).userIsAdminFrontEnd())
    return true
  else {
    inject(Router).navigate(['home'])
    return false;
  }
}

