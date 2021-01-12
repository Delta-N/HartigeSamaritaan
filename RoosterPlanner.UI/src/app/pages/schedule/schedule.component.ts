import {Component, OnInit, ViewChild} from '@angular/core';
import {ActivatedRoute, ParamMap} from "@angular/router";
import {Availability} from "../../models/availability";
import {AvailabilityService} from "../../services/availability.service";
import {MatTableDataSource} from "@angular/material/table";
import {MatPaginator} from "@angular/material/paginator";
import {MatSort} from "@angular/material/sort";
import {TextInjectorService} from "../../services/text-injector.service";
import * as moment from "moment"
import {BreadcrumbService} from "../../services/breadcrumb.service";
import {Breadcrumb} from "../../models/breadcrumb";


@Component({
  selector: 'app-schedule',
  templateUrl: './schedule.component.html',
  styleUrls: ['./schedule.component.scss']
})
export class ScheduleComponent implements OnInit {
  loaded: boolean = false;
  today: Date = new Date();
  displayedColumns: string[] = [];
  dataSource: MatTableDataSource<Availability> = new MatTableDataSource<Availability>();
  paginator: MatPaginator;
  sort: MatSort;


  @ViewChild(MatSort) set matSort(ms: MatSort) {
    this.sort = ms;
    this.setDataSourceAttributes()
  }

  @ViewChild(MatPaginator) set matPaginator(mp: MatPaginator) {
    this.paginator = mp;
    this.setDataSourceAttributes()
  }

  constructor(private route: ActivatedRoute,
              private availabilityService: AvailabilityService,
              private breadcrumbService: BreadcrumbService) {
  }

  ngOnInit(): void {
    this.displayedColumns = TextInjectorService.availabilitiesTableColumnNames;
    this.route.paramMap.subscribe(async (params: ParamMap) => {
      await this.availabilityService.getScheduledAvailabilities(params.get('id'))
        .then(res => {
          if (res&& res.length>0) {
            //push old availabilities to the back
            let old: Availability[] = []
            let all: Availability[] = []
            res.forEach(a => {
              if (moment(a.shift.date).startOf("day").toDate() < moment().startOf("day").toDate())
                old.push(a)
              else
                all.push(a)
            })
            old.forEach(a => all.push(a))

            this.dataSource = new MatTableDataSource<Availability>(all)

            // @ts-ignore
            this.dataSource.sortingDataAccessor = (item, property) => {
              switch (property) {
                case 'Taak':
                  return item.shift.task != null ? item.shift.task.name : null;
                case 'Datum':
                  return item.shift.date;
                case 'Vanaf':
                  return item.shift.startTime;
                case 'Tot':
                  return item.shift.endTime;
                default:
                  return item[property];
              }
            };
            let previous: Breadcrumb = new Breadcrumb();
            previous.label = res[0].participation.project.name
            previous.url = "/project/" + res[0].participation.project.id
            let current: Breadcrumb = new Breadcrumb();
            current.label = 'Mijn shifts';
            let array: Breadcrumb[] = [this.breadcrumbService.dashboardcrumb, previous, current];
            this.breadcrumbService.replace(array);

          }
          this.loaded = true
        })
    })
  }

  setDataSourceAttributes() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  openInstructions(url: string | null) {
    if (url)
      window.open(url, "_blank")
  }
}
