import {Component, OnInit, ViewChild} from '@angular/core';
import {ActivatedRoute, ParamMap} from "@angular/router";
import {Availability} from "../../models/availability";
import {AvailabilityService} from "../../services/availability.service";
import {MatTableDataSource} from "@angular/material/table";
import {MatPaginator} from "@angular/material/paginator";
import {MatSort} from "@angular/material/sort";
import {TextInjectorService} from "../../services/text-injector.service";
import * as moment from "moment"


@Component({
  selector: 'app-schedule',
  templateUrl: './schedule.component.html',
  styleUrls: ['./schedule.component.scss']
})
export class ScheduleComponent implements OnInit {

  today: Date = new Date();
  displayedColumns: string[] = [];
  dataSource: MatTableDataSource<Availability> = new MatTableDataSource<Availability>();
  paginator: MatPaginator;
  sort: MatSort;
  loaded: boolean=false;

  @ViewChild(MatSort) set matSort(ms: MatSort) {
    this.sort = ms;
    this.setDataSourceAttributes()
  }

  @ViewChild(MatPaginator) set matPaginator(mp: MatPaginator) {
    this.paginator = mp;
    this.setDataSourceAttributes()
  }

  constructor(private route: ActivatedRoute,
              private availabilityService: AvailabilityService) {
  }

  ngOnInit(): void {
    this.displayedColumns = TextInjectorService.availabilitiesTableColumnNames;
    this.route.paramMap.subscribe(async (params: ParamMap) => {
      await this.availabilityService.getScheduledAvailabilities(params.get('id'))
        .then(res => {
          if (res) {
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
            this.loaded=true;
          }
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
