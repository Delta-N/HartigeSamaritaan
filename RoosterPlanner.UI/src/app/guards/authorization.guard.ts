﻿import { Injectable } from '@angular/core';
import {
	ActivatedRouteSnapshot,
	CanActivate,
	CanLoad,
	Route,
	Router,
	RouterStateSnapshot,
	UrlSegment,
	UrlTree,
} from '@angular/router';
import { UserService } from '../services/user.service';
import { Observable, of } from 'rxjs';

@Injectable()
export class AuthorizationGuard implements CanActivate, CanLoad {
	constructor(private router: Router, private userService: UserService) {}

	canActivate(
		route: ActivatedRouteSnapshot,
		state: RouterStateSnapshot
	): boolean | Observable<boolean> {
		if (this.userService.userIsAdminFrontEnd()) return true;
		else {
			this.router.navigate(['home']);
			return false;
		}
	}

	canLoad(
		route: Route,
		segments: UrlSegment[]
	):
		| Observable<boolean | UrlTree>
		| Promise<boolean | UrlTree>
		| boolean
		| UrlTree {
		if (this.userService.userIsAdminFrontEnd()) return true;
		else {
			this.router.navigate(['home']);
			return false;
		}
	}
}
