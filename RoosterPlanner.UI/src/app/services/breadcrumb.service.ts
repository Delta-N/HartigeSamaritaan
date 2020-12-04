import {Injectable} from '@angular/core';
import {Breadcrumb} from "../models/breadcrumb";
import {Observable, of, from, BehaviorSubject} from "rxjs";
import {filter} from "rxjs/operators";
import {NavigationStart, Router} from "@angular/router";

@Injectable({
  providedIn: 'root'
})
export class BreadcrumbService {

  breadcrumbs: Breadcrumb[] = [];
  behaviourSubject: BehaviorSubject<Breadcrumb[]> = new BehaviorSubject<Breadcrumb[]>(this.breadcrumbs);

  previousUrl: string;
  currentUrl: string;

  dashboardcrumb: Breadcrumb = new Breadcrumb();
  admincrumb: Breadcrumb = new Breadcrumb();
  managecrumb: Breadcrumb = new Breadcrumb();
  takencrumb: Breadcrumb = new Breadcrumb();
  shiftOverviewCrumb: Breadcrumb = new Breadcrumb();

  constructor(private router: Router) {
    this.dashboardcrumb.url = "/home";
    this.dashboardcrumb.label = "Dashboard";

    this.admincrumb.url = "/admin";
    this.admincrumb.label = "Admin";

    this.managecrumb.url = "/manage";
    this.managecrumb.label = "Beheer";

    this.takencrumb.url = "/tasks";
    this.takencrumb.label = "Taken";

    router.events.pipe(filter(event => event instanceof NavigationStart))
      .subscribe((event: NavigationStart) => {
        this.clear();
        this.previousUrl = this.currentUrl;
        this.currentUrl = event.url;
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
    let backcrumb: Breadcrumb = new Breadcrumb();
    backcrumb.url = this.previousUrl;
    backcrumb.label = "Terug";
    this.breadcrumbs = [backcrumb];
    this.behaviourSubject.next(this.breadcrumbs);
  }
}
