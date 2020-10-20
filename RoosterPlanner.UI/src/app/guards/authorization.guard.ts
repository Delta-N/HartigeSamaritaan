import {Injectable} from "@angular/core";
import {ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot} from "@angular/router";
import {JwtHelper} from "../helpers/jwt-helper";
import {AuthenticationService} from "../services/authentication.service";
import {UserService} from "../services/user.service";


@Injectable()
export class AuthorizationGuard implements CanActivate {

  constructor(private router: Router,
              private userService: UserService) {
  }

  canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    if (this.userService.userIsAdminFrontEnd()) {
      return true;
    } else {
      this.router.navigateByUrl('').then();
      return false;
    }
  }
}
