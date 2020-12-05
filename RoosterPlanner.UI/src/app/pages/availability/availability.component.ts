import {Component, OnInit} from '@angular/core';
import {BreadcrumbService} from "../../services/breadcrumb.service";
import {Breadcrumb} from "../../models/breadcrumb";
import {ActivatedRoute, ParamMap} from "@angular/router";
import {Shift} from "../../models/shift";
import {ShiftService} from "../../services/shift.service";

@Component({
  selector: 'app-availability',
  templateUrl: './availability.component.html',
  styleUrls: ['./availability.component.scss']
})
export class AvailabilityComponent implements OnInit {
  shifts: Shift[] = [];


  constructor(private breadcrumbService: BreadcrumbService,
              private shiftService: ShiftService,
              private route: ActivatedRoute) {


  }

  ngOnInit(): void {
    this.route.paramMap.subscribe(async (params: ParamMap) => {
      await this.shiftService.getAllShifts(params.get('id')).then(async res => {
        this.shifts = res;
        //create breadcrumbs
        let current: Breadcrumb = new Breadcrumb();
        current.label = 'Beschikbaarheid opgeven';
        let previous: Breadcrumb = new Breadcrumb();
        previous.label = this.shifts[0].project.name
        previous.url = "/project/" + this.shifts[0].project.id

        let array: Breadcrumb[] = [this.breadcrumbService.dashboardcrumb, previous, current];
        this.breadcrumbService.replace(array);
      });
    });


  }

}
