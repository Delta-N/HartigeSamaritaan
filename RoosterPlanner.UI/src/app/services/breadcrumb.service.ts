import {Injectable} from '@angular/core';
import {Breadcrumb} from "../models/breadcrumb";
import {BehaviorSubject} from "rxjs";
import {filter} from "rxjs/operators";
import {NavigationStart, Router} from "@angular/router";

@Injectable({
  providedIn: 'root'
})
export class BreadcrumbService {

  breadcrumbs: Breadcrumb[] = [];
  behaviourSubject: BehaviorSubject<Breadcrumb[]> = new BehaviorSubject<Breadcrumb[]>(this.breadcrumbs);

  dashboardcrumb: Breadcrumb = new Breadcrumb("Dashboard", "/home");
  admincrumb: Breadcrumb = new Breadcrumb("Admin", "/admin");
  managecrumb: Breadcrumb = new Breadcrumb("Beheer", "/manage");

  constructor(private router: Router) {


    router.events.pipe(filter(event => event instanceof NavigationStart))
      .subscribe((event: NavigationStart) => {
        this.clear();
      });
  }

  replace(newBreadcrumbs: Breadcrumb[]) {
    this.breadcrumbs = newBreadcrumbs;
    this.behaviourSubject.next(this.breadcrumbs);
  }

  clear() {
    this.breadcrumbs = [];
    this.behaviourSubject.next(this.breadcrumbs);
  }

  backcrumb() {
    let backcrumb: Breadcrumb = new Breadcrumb("Terug", null);
    this.breadcrumbs = [backcrumb];
    this.behaviourSubject.next(this.breadcrumbs);
  }
}
