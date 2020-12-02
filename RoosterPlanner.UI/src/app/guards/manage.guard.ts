﻿import {Injectable} from "@angular/core";
import {ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot} from "@angular/router";
import {UserService} from "../services/user.service";


@Injectable()
export class ManageGuard implements CanActivate {

  constructor(private router: Router,
              private userService: UserService) {
  }

  canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    if (this.userService.userIsAdminFrontEnd() || this.userService.userIsProjectAdminFrontEnd()) {
      return true;
    } else {
      this.router.navigateByUrl('').then();
      return false;
    }
  }
}
