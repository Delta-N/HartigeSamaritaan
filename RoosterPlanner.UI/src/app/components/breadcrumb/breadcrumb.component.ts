import {Component, OnInit} from '@angular/core';
import {Breadcrumb} from "../../models/breadcrumb";
import { NavigationStart, Router} from "@angular/router";
import {BreadcrumbService} from "../../services/breadcrumb.service";
import {filter} from "rxjs/operators";

@Component({
  selector: 'app-breadcrumb',
  templateUrl: './breadcrumb.component.html',
  styleUrls: ['./breadcrumb.component.scss']
})
export class BreadcrumbComponent implements OnInit {
  breadcrumbs: Breadcrumb[]=[];

  constructor(private breadcrumbService:BreadcrumbService, private router:Router) {
    router.events.pipe(filter(event => event instanceof NavigationStart))
      .subscribe((event: NavigationStart) => {
        this.breadcrumbs=[];
      });

    breadcrumbService.breadcrumbObs.subscribe(values=>console.log(values))
  }

  ngOnInit(): void {
  }

}
