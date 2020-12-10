import {Component, OnInit, ChangeDetectionStrategy, ViewChild} from '@angular/core';
import {BreadcrumbService} from "../../services/breadcrumb.service";
import {Breadcrumb} from "../../models/breadcrumb";
import {ActivatedRoute, ParamMap} from "@angular/router";
import {Shift} from "../../models/shift";
import {ShiftService} from "../../services/shift.service";
import {CalendarEventAction, CalendarView, CalendarEvent, CalendarDateFormatter} from 'angular-calendar';
import * as moment from "moment"
import {CustomDateFormatter} from "../../helpers/custom-date-formatter.provider";
import {DomSanitizer} from "@angular/platform-browser";
import {MatCalendar} from "@angular/material/datepicker";
import {Moment} from "moment";
import {MatCheckboxChange} from "@angular/material/checkbox";
import {UserService} from "../../services/user.service";
import {Subject} from "rxjs";


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
  @ViewChild('calendar') calendar: MatCalendar<Moment>;
  projectId: string;
  userId: string;
  selectedDate: Moment;

  shifts: Shift[] = [];

  view: CalendarView = CalendarView.Day;
  viewDate: Date = new Date(); //today

  minDate: Date;
  maxDate: Date;

  startHour: number = 1; // dit aanpassen naar start hour van eerste even
  endHour: number = 23; //  dit aanpassen naar end hour van laatste event


  dateIsValid(date: Date): boolean {
    return date >= this.minDate && date <= this.maxDate;
  }

  changeDate(date: Date): void {
    this.viewDate = date;
    this.dateOrViewChanged();
  }

  dateChanged() {
    this.calendar.activeDate = this.selectedDate;
    this.changeDate(this.selectedDate.toDate())
  }

  increment(): void {
    this.changeDate(moment(this.viewDate).add(1, "day").toDate());
  }

  decrement(): void {
    this.changeDate(moment(this.viewDate).subtract(1, "day").toDate());
  }

  async dateOrViewChanged(): Promise<void> {
    await this.getShifts(this.viewDate);
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

  maybeLabel: any = this.sanitizer.bypassSecurityTrustHtml('<span class="mat-button custom-button">?</div></span>');
  yesLabel: any = this.sanitizer.bypassSecurityTrustHtml('<span class="mat-button custom-button" id="yess")">V</span>');
  noLabel: any = this.sanitizer.bypassSecurityTrustHtml('<span class="mat-button custom-button">X</span>');

  changeBorders(event: CalendarEvent, label: String) {
    let aria = "\n      " + moment(event.start).format("dddd MMMM DD,") + "\n      " + event.title + ", from " + moment(event.start).format("hh:mm A") + "\n     to " +
      moment(event.end).format("hh:mm A");
    let x: HTMLCollection = (document.getElementsByClassName("cal-event"))
    for (let i = 0; i < x.length; i++) {
      let element: any = x[i]
      if (element.ariaLabel == aria) {
        for (let j = 0; j < x[i].children.length; j++) {
          let child: any = element.children[j];
          if (child.localName == "mwl-calendar-event-actions") {
            let farDecendend = child.children[0].children;
            for (let k = 0; k < farDecendend.length; k++) {
              if (farDecendend[k].ariaLabel == label) {
                farDecendend[k].children[0].style.border = "solid 3px black";
              } else {
                farDecendend[k].children[0].style.border = "none";
              }
            }
          }
        }
      }
    }
  }

  actions: CalendarEventAction[] = [
    {

      label: this.yesLabel,
      a11yLabel: 'Yes',
      onClick: ({event}: { event: CalendarEvent }): void => {
        this.colorInCalendar(new Date(), "Green")
        this.changeBorders(event, "Yes")

      },
    },
    {
      label: this.maybeLabel,
      a11yLabel: 'Maybe',
      onClick: ({event}: { event: CalendarEvent }): void => {
        this.changeBorders(event, "Maybe")
      },
    },
    {
      label: this.noLabel,
      a11yLabel: 'No',
      onClick: ({event}: { event: CalendarEvent }): void => {
        this.changeBorders(event, "No")
      },
    },

  ];
  //dit is voorbeeld code
  date = new Date();
  five = "17:00";
  fiveFourtyFive = "17:45";
  six = "18:00";
  eight = "20:00";
  eightFiveteen = "20:15";
  eleven = "23:00";
  elevenThirty = "23:30";
  twelve = "24:00";

  constructor(private breadcrumbService: BreadcrumbService,
              private shiftService: ShiftService,
              private userService: UserService,
              private route: ActivatedRoute,
              private sanitizer: DomSanitizer) {
    moment.locale('en')
  }

  colorInCalendar(date: Date, color: string) {
    let label = moment(date).format("DD MMMM YYYY").toLowerCase()
    let element: HTMLElement = document.querySelector("[aria-label=" + CSS.escape(label) + "]");
    let child: any = element.children[0]
    child.style.background = color;
    child.style.color = 'white';
  }

  events: CalendarEvent[] = [
    /* {
       start: moment(new Date())
         .set("hour", Number(this.five.substring(0, 2)))
         .set("minutes", Number(this.five.substring(3, 6))).toDate(),

       end: moment(new Date())
         .set("hour", Number(this.eleven.substring(0, 2)))
         .set("minutes", Number(this.eleven.substring(3, 6))).toDate(),

       title: 'Koken',
       color: colors.red,
       actions: this.actions,
       resizable: {
         beforeStart: true,
         afterEnd: true,
       },
     },

     {
       start: moment(new Date())
         .set("hour", Number(this.six.substring(0, 2)))
         .set("minutes", Number(this.six.substring(3, 6))).toDate(),

       end: moment(new Date())
         .set("hour", Number(this.elevenThirty.substring(0, 2)))
         .set("minutes", Number(this.elevenThirty.substring(3, 6))).toDate(),

       title: 'Bediening',
       color: colors.blue,
       actions: this.actions,
       resizable: {
         beforeStart: true,
         afterEnd: true,
       },
     },
     {
       start: moment(new Date())
         .set("hour", Number(this.fiveFourtyFive.substring(0, 2)))
         .set("minutes", Number(this.fiveFourtyFive.substring(3, 6))).toDate(),

       end: moment(new Date())
         .set("hour", Number(this.eight.substring(0, 2)))
         .set("minutes", Number(this.eight.substring(3, 6))).toDate(),

       title: 'Afwas',
       color: colors.green,
       actions: this.actions,
       resizable: {
         beforeStart: true,
         afterEnd: true,
       },
     },
     {
       start: moment(new Date())
         .set("hour", Number(this.eightFiveteen.substring(0, 2)))
         .set("minutes", Number(this.eightFiveteen.substring(3, 6))).toDate(),

       end: moment(new Date())
         .set("hour", Number(this.twelve.substring(0, 2)))
         .set("minutes", Number(this.twelve.substring(3, 6))).toDate(),

       title: 'Afwas',
       color: colors.green,
       actions: this.actions,
       resizable: {
         beforeStart: true,
         afterEnd: true,
       },
     },*/
  ];
  refresh: Subject<any> = new Subject();

  async getShifts(date: Date) {
    await this.shiftService.getAllShiftsOnDate(this.projectId, this.userId, date).then(async res => {
      this.shifts = res;
      console.log(res);
    });
    this.addEvents();
    console.log(this.events)
  }

  addEvents() {
    this.events = [];
    this.shifts.forEach(s => {
      let event: CalendarEvent = {
        start: moment(s.date)
          .set("hour", Number(s.startTime.substring(0, 2)))
          .set("minutes", Number(s.startTime.substring(3, 6))).toDate(),

        end: moment(s.date)
          .set("hour", Number(s.endTime.substring(0, 2)))
          .set("minutes", Number(s.endTime.substring(3, 6))).toDate(),

        title: s.task.name,
        color: this.getColor(s.task.color),
        actions: this.actions,
        resizable: {
          beforeStart: true,
          afterEnd: true,
        }
      };

      this.events.push(event)
    })
    this.refresh.next();
  }


  ngOnInit(): void {
    console.log(this.events)
    this.userId = this.userService.getCurrentUserId();
    this.route.paramMap.subscribe(async (params: ParamMap) => {
      this.projectId = params.get('id');
      await this.getShifts(this.viewDate).then(() => {
        //create breadcrumbs
        let current: Breadcrumb = new Breadcrumb();
        current.label = 'Beschikbaarheid opgeven';
        let previous: Breadcrumb = new Breadcrumb();
        previous.label = this.shifts[0].project.name
        previous.url = "/project/" + this.shifts[0].project.id

        let array: Breadcrumb[] = [this.breadcrumbService.dashboardcrumb, previous, current];
        this.breadcrumbService.replace(array);
      })
    });
  }

  OnCheckboxChange($event: MatCheckboxChange) {
  }

  getColor(color: string) {
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
      green: {
        primary: '#1f931f',
        secondary: '#c0f2c0'
      },
      orange: {
        primary: '#cc5200',
        secondary: '#ffc299'
      },
      pink: {
        primary: '#cc0052',
        secondary: '#ffb3d1'
      }
    };

    switch (color.toLowerCase()) {
      case "red":
        return colors.red
      case "blue":
        return colors.blue
      case "yellow":
        return colors.yellow
      case "green":
        return colors.green
      case "orange":
        return colors.orange
      case "pink":
        return colors.pink
    }
  }
}
