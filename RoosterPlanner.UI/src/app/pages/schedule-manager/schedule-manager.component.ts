import {Component, OnInit, ViewChild} from '@angular/core';
import {Moment} from "moment";
import {MatCalendar} from "@angular/material/datepicker";
import {ActivatedRoute, ParamMap, Router} from "@angular/router";
import {MatTableDataSource} from "@angular/material/table";
import {Availability} from "../../models/availability";
import {MatPaginator} from "@angular/material/paginator";
import {MatSort} from "@angular/material/sort";
import {AvailabilityService} from "../../services/availability.service";
import {TextInjectorService} from "../../services/text-injector.service";
import {ProjectService} from "../../services/project.service";
import {BreadcrumbService} from "../../services/breadcrumb.service";
import {Breadcrumb} from "../../models/breadcrumb";

@Component({
  selector: 'app-schedule-manager',
  templateUrl: './schedule-manager.component.html',
  styleUrls: ['./schedule-manager.component.scss']
})
export class ScheduleManagerComponent implements OnInit {
  @ViewChild('calendar') calendar: MatCalendar<Moment>;

  selectedDate: Moment;
  viewDate: Date = new Date();
  minDate: Date;
  maxDate: Date;

  displayedColumns: string[] = [];
  dataSource: MatTableDataSource<Availability> = new MatTableDataSource<Availability>();
  paginator: MatPaginator;
  sort: MatSort;
  loaded: boolean = false;

  @ViewChild(MatSort) set matSort(ms: MatSort) {
    this.sort = ms;
    this.setDataSourceAttributes()
  }

  @ViewChild(MatPaginator) set matPaginator(mp: MatPaginator) {
    this.paginator = mp;
    this.setDataSourceAttributes()
  }

  projectId: string;


  constructor(private route: ActivatedRoute,
              private router:Router,
              private availabilityService: AvailabilityService,
              private projectService:ProjectService,
              private breadcrumbService:BreadcrumbService) {
    let breadcrumb: Breadcrumb = new Breadcrumb();
    breadcrumb.label = 'Rooster';
    let array: Breadcrumb[] = [this.breadcrumbService.dashboardcrumb,
      this.breadcrumbService.managecrumb, breadcrumb];

    this.breadcrumbService.replace(array);
  }

  ngOnInit(): void {
    this.displayedColumns = TextInjectorService.scheduleTableColumnNames;

    this.route.paramMap.subscribe(async (params: ParamMap) => {
      this.projectId = params.get('id');
      this.projectService.getProject(this.projectId).then(res=>{
        this.minDate=res.participationStartDate
        this.maxDate=res.participationEndDate
      })
      await this.getAvailabilities(this.viewDate).then(() => {

        this.loaded = true;
      })
    })
  }

  setDatasource(availabilaties: Availability[]) {
    this.dataSource = new MatTableDataSource<Availability>(availabilaties)
    this.setDataSourceAttributes();


    this.dataSource.sortingDataAccessor = (item, property) => {
      switch (property) {
        case 'Taak':
          return item.shift.task != null ? item.shift.task.name : null;
        case 'Vanaf':
          return item.shift.startTime;
        case 'Tot':
          return item.shift.endTime;
        default:
          return item[property];
      }
    };
  }

  dateChanged() {
    this.calendar.activeDate = this.selectedDate;
    this.changeDate(this.selectedDate.toDate())
  }

  changeDate(date: Date): void {
    this.viewDate = date;
    this.dateOrViewChanged();
  }

  async dateOrViewChanged(): Promise<void> {
    await this.getAvailabilities(this.viewDate).then(() => {
      if (this.viewDate < this.minDate) {
        this.changeDate(this.minDate);
      } else if (this.viewDate > this.maxDate) {
        this.changeDate(this.maxDate);
      }
    });

  }

  async getAvailabilities(viewDate: Date) {
    await this.availabilityService.getScheduledAvailabilitiesOnDate(this.projectId, viewDate).then(res => {
      this.setDatasource(res)
    })
  }

  setDataSourceAttributes() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  details(personId: string) {
    this.router.navigate(['manage/profile', personId])
  }
}

