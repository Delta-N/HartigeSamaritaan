import {Component, OnInit} from '@angular/core';
import {Breadcrumb} from '../../models/breadcrumb';
import {BreadcrumbService} from '../../services/breadcrumb.service';

@Component({
  selector: 'app-breadcrumb',
  templateUrl: './breadcrumb.component.html',
  styleUrls: ['./breadcrumb.component.scss']
})
export class BreadcrumbComponent implements OnInit {
  breadcrumbs: Breadcrumb[] = [];


  constructor(private breadcrumbService: BreadcrumbService, ) {
    breadcrumbService.behaviourSubject.subscribe(values => {
      this.breadcrumbs = values;
    });
  }

  ngOnInit(): void {
  }

}
