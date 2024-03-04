import {inject, Injectable} from "@angular/core";
import {
  ActivatedRouteSnapshot, CanActivateFn,
  Router,
  RouterStateSnapshot,
} from "@angular/router";
import {UserService} from "../services/user.service";


@Injectable()
export class ManageGuard {

  constructor(private router: Router,
              private userService: UserService) {
  }
}


export const manageCanActivateGuard: CanActivateFn = (route: ActivatedRouteSnapshot, state: RouterStateSnapshot) => {
  if (inject(UserService).userIsProjectAdminFrontEnd())
    return true
  else {
    inject(Router).navigate(['home'])
    return false;
  }
}
