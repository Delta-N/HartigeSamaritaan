import {Component, OnInit, ChangeDetectionStrategy} from '@angular/core';
import {BreadcrumbService} from "../../services/breadcrumb.service";
import {Breadcrumb} from "../../models/breadcrumb";
import {ActivatedRoute, ParamMap} from "@angular/router";
import {Shift} from "../../models/shift";
import {ShiftService} from "../../services/shift.service";
import {CalendarEventAction, CalendarView, CalendarEvent, CalendarDateFormatter} from 'angular-calendar';
import * as moment from "moment"
import {CustomDateFormatter} from "../../helpers/custom-date-formatter.provider";


const colors: any = {
  red: {
    primary: '#ad2121',
    secondary: '#FAE3E3',
  },
  blue: {
    primary: '#1e90ff',
    secondary: '#D1E8FF',
  },
  yellow: {
    primary: '#e3bc08',
    secondary: '#FDF1BA',
  },
};
@Component({
  selector: 'app-availability',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './availability.component.html',
  styleUrls: ['./availability.component.scss'],
  providers: [
    {
      provide: CalendarDateFormatter,
      useClass: CustomDateFormatter,
    },
  ],
})
export class AvailabilityComponent implements OnInit {
  shifts: Shift[] = [];

  view: CalendarView = CalendarView.Day;
  viewDate: Date = new Date(); //today

  minDate: Date;
  maxDate: Date;

  startHour: number=9;
  endHour: number=22;

  dateIsValid(date: Date): boolean {
    return date >= this.minDate && date <= this.maxDate;
  }

  changeDate(date: Date): void {
    this.viewDate = date;
    this.dateOrViewChanged();
  }

  increment(): void {
    this.changeDate(moment(this.viewDate).add(1,"day").toDate());
  }

  decrement(): void {
    this.changeDate(moment(this.viewDate).subtract(1, "day").toDate());
  }

  dateOrViewChanged(): void {
  /*  this.prevBtnDisabled = !this.dateIsValid(
      endOfPeriod(this.view, subPeriod(this.view, this.viewDate, 1))
    );
    this.nextBtnDisabled = !this.dateIsValid(
      startOfPeriod(this.view, addPeriod(this.view, this.viewDate, 1))
    );
    if (this.viewDate < this.minDate) {
      this.changeDate(this.minDate);
    } else if (this.viewDate > this.maxDate) {
      this.changeDate(this.maxDate);
    }*/
  }

  handleEvent(action: string, event: CalendarEvent): void {
    /*this.modalData = {event, action};
    this.modal.open(this.modalContent, {size: 'lg'});*/
  }


  actions: CalendarEventAction[] = [
    {
      label: '<i class="fas fa-fw fa-pencil-alt"></i>',
      a11yLabel: 'Edit',
      onClick: ({event}: { event: CalendarEvent }): void => {
        this.handleEvent('Edited', event);
      },
    },
    {
      label: '<i class="fas fa-fw fa-trash-alt"></i>',
      a11yLabel: 'Delete',
      onClick: ({event}: { event: CalendarEvent }): void => {
        this.events = this.events.filter((iEvent) => iEvent !== event);
        this.handleEvent('Deleted', event);
      },
    },
  ];

  events: CalendarEvent[] = [
    {
      start: moment(new Date()).toDate(),
      end: moment(new Date).add("hours",3).toDate(),
      title: 'A draggable and resizable event',
      color: colors.red,
      actions: this.actions,
      resizable: {
        beforeStart: true,
        afterEnd: true,
      },
      draggable: true,
    },

    {
      start: moment(new Date()).add(1,"hour").toDate(),
      end: moment(new Date).add(3,"hours").toDate(),
      title: 'A draggable and resizable event',
      color: colors.red,
      actions: this.actions,
      resizable: {
        beforeStart: true,
        afterEnd: true,
      },
      draggable: true,
    },
  ];


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
