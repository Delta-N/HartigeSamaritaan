import {Injectable} from '@angular/core';
import {Breadcrumb} from "../models/breadcrumb";
import {Observable, of} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class BreadcrumbService {

  breadcrumbs: Breadcrumb[] = [];
  breadcrumbObs = of(this.breadcrumbs);

  constructor() {
  }

  replace(newBreadcrumbs: Breadcrumb[]) {
    this.clear();
    this.breadcrumbs = newBreadcrumbs;
    console.log(this.breadcrumbs)
    console.log(this.breadcrumbObs)
  }

  clear() {
    this.breadcrumbs = [];
  }
}
