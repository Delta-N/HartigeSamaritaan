import {inject, Injectable} from "@angular/core";
import {
  ActivatedRouteSnapshot,
   CanActivateFn,

  Router,
  RouterStateSnapshot,

} from "@angular/router";
import {UserService} from "../services/user.service";


@Injectable({providedIn: 'root'})
export class AuthorizationGuard {

  constructor(private router: Router,
              private userService: UserService) {
  }

  // canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | Observable<boolean> {
  //   if (this.userService.userIsAdminFrontEnd())
  //     return true
  //   else {
  //     this.router.navigate(['home'])
  //     return false;
  //   }
  // }
}

export const authCanActivateGuard: CanActivateFn = (route: ActivatedRouteSnapshot, state: RouterStateSnapshot) => {
  if (inject(UserService).userIsAdminFrontEnd())
    return true
  else {
    inject(Router).navigate(['home'])
    return false;
  }
}

