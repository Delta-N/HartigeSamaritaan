import {
  Component,
  OnInit,
  AfterViewInit,
  ChangeDetectionStrategy,
  ViewChild,
  Renderer2,
} from '@angular/core';
import {BreadcrumbService} from "../../services/breadcrumb.service";
import {Breadcrumb} from "../../models/breadcrumb";
import {ActivatedRoute, ParamMap, Router} from "@angular/router";
import {Shift} from "../../models/shift";
import {ShiftService} from "../../services/shift.service";
import {
  CalendarEventAction,
  CalendarView,
  CalendarEvent,
  CalendarDateFormatter,
  CalendarEventTitleFormatter, CalendarDayViewComponent
} from 'angular-calendar';
import * as moment from "moment"
import {CustomDateFormatter} from "../../helpers/custom-date-formatter.provider";
import {DomSanitizer} from "@angular/platform-browser";
import {MatCalendar} from "@angular/material/datepicker";
import {Moment} from "moment";
import {MatCheckboxChange} from "@angular/material/checkbox";
import {BehaviorSubject, Subject} from "rxjs";
import {AvailabilityService} from "../../services/availability.service";
import {AvailabilityData} from "../../models/availabilitydata";
import {Task} from 'src/app/models/task';
import {CustomEventTitleFormatter} from "../../helpers/custon-event-title-formatter.provider";
import {take} from "rxjs/operators";
import {Project} from "../../models/project";
import {ProjectService} from "../../services/project.service";


@Component({
  selector: 'app-plan',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './plan.component.html',
  styleUrls: ['./plan.component.scss'],
  providers: [
    {
      provide: CalendarDateFormatter,
      useClass: CustomDateFormatter,
    },
    {
      provide: CalendarEventTitleFormatter,
      useClass: CustomEventTitleFormatter,
    },
  ],
})
export class PlanComponent implements OnInit, AfterViewInit {
  @ViewChild('calendar') calendar: MatCalendar<Moment>;
  @ViewChild('schedule') schedule: CalendarDayViewComponent;

  project: Project;
  availabilityData: AvailabilityData;
  displayedProjectTasks: Task[] = [];
  shifts: Shift[] = [];

  selectedDate: Moment;

  view: CalendarView = CalendarView.Day;
  viewDate: Date;

  minDate: Date;
  maxDate: Date;

  startHour: number = 0;
  endHour: number = 23;
  prevBtnDisabled: boolean;
  nextBtnDisabled: boolean;

  filteredEvents: CalendarEvent[] = [];
  filteredEventsObservable: BehaviorSubject<CalendarEvent[]> = new BehaviorSubject<CalendarEvent[]>(this.filteredEvents);
  allEvents: CalendarEvent[] = [];
  refresh: Subject<any> = new Subject();

  scheduledLabel: any = this.sanitizer.bypassSecurityTrustHtml('<span class="mat-button custom-button scheduled">Plannen</span>');


  constructor(private breadcrumbService: BreadcrumbService,
              private shiftService: ShiftService,
              private availabilityService: AvailabilityService,
              private route: ActivatedRoute,
              private sanitizer: DomSanitizer,
              private renderer: Renderer2,
              private projectService: ProjectService,
              private router: Router,) {
  }

  async ngOnInit(): Promise<void> {

    this.route.paramMap.subscribe(async (params: ParamMap) => {
      let projectId: string = params.get('id');
      let date: string = params.get('date')
      if (date && date !== 'Invalid Date')
        this.viewDate = moment(date).toDate();
      else
        this.viewDate = new Date();


      //get basic data
      await this.availabilityService.getAvailabilityDataOfProject(projectId).then(res => {
        if (res)
          this.availabilityData = res;
        this.displayedProjectTasks = this.availabilityData.projectTasks;

      })

      //getproject
      await this.projectService.getProject(projectId).then(res => {
        if (res) {
          this.project = res;
          this.minDate = this.project.participationStartDate >= new Date() ? this.project.participationStartDate : new Date();
          this.maxDate = this.project.participationEndDate;
          this.calendar.minDate = moment(this.minDate);
          this.calendar.maxDate = moment(this.maxDate);
          this.calendar.updateTodaysDate()
        }
      })


      //create breadcrumbs
      let current: Breadcrumb = new Breadcrumb();
      current.label = 'Plannen';
      let array: Breadcrumb[] = [this.breadcrumbService.dashboardcrumb, this.breadcrumbService.managecrumb, current];
      this.breadcrumbService.replace(array);
    })
  }

  ngAfterViewInit(): void {
    const buttons = document.querySelectorAll('.mat-calendar-previous-button, .mat-calendar-next-button');
    if (buttons) {
      Array.from(buttons).forEach(button => {
        this.renderer.listen(button, 'click', () => {
          this.colorInMonth();
        });
      });
    }

    this.calendar.stateChanges.pipe(take(1)).subscribe(() => {
      this.getShifts(this.viewDate).then(() => {
        this.colorInMonth();
      })
    })
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
    await this.getShifts(this.viewDate).then(() => {
      if (this.viewDate < this.minDate) {
        this.changeDate(this.minDate);
      } else if (this.viewDate > this.maxDate) {
        this.changeDate(this.maxDate);
      }
    });

    this.prevBtnDisabled = moment(this.viewDate).subtract(1, "day") < moment(this.minDate);
    this.nextBtnDisabled = moment(this.viewDate).add(1, "day") > moment(this.maxDate)
  }


  Plan(id: string | number) {
    this.router.navigate(['/plan/shift', id]).then();

  }

  actions: CalendarEventAction[] = [
    {
      label: this.scheduledLabel,
      a11yLabel: 'Schedule',
      onClick: ({event}: { event: CalendarEvent }): void => {
        this.Plan(event.id)
      },
    },
  ]

  colorInMonth() {

    for (const ka of this.availabilityData.knownAvailabilities) {
      let date: Date = moment(ka.date).toDate();
      let color: string = "Red"
      if (ka.status === 1)
        color = "Green"
      else if (ka.status === 2)
        color = "Blue"

      this.colorInDay(date, color)
    }
  }

  colorInDay(date: Date, color: string) {
    let label = moment(date).local().format("D MMMM YYYY").toLowerCase()
    let element: HTMLElement = document.querySelector("[aria-label=" + CSS.escape(label) + "]");

    if (element) {
      let child: any = element.children[0]
      child.style.background = color;
      child.style.color = 'white';
    }
  }

  async getShifts(date: Date) {
    await this.shiftService.getAllShiftsOnDate(this.project.id, moment(date).set("hour", 12).toDate()).then(async res => {
      this.shifts = res;
    });
    if (this.shifts.length > 0) {
      this.addEvents();
      setTimeout(() => {
        this.fillSpacer()
      }, 100)
    }
  }

  addEvents() {

    this.allEvents = [];
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
        id: s.id
      };

      this.allEvents.push(event)
    })
    this.filterEvents();
    this.refresh.next();
  }

  filterEvents() {
    this.filteredEvents = []
    this.allEvents.forEach(e => {
      let contains: boolean = false;
      this.displayedProjectTasks.forEach(d => {
        if (d.name == e.title) {
          contains = true;
        }
      })
      if (contains)
        this.filteredEvents.push(e);
    })


    this.setHours()
  }

  setHours() {
    let start: Date[] = []
    this.filteredEvents.forEach(fe => start.push(fe.start))
    start.sort()

    let end: Date[] = []
    this.filteredEvents.forEach(fe => end.push(fe.end))
    end.sort()

    if (start && start.length > 0)
      this.startHour = moment(start[0]).subtract(1, "hour").hour()
    else
      this.startHour = 12;
    if (end && end.length > 0)
      this.endHour = moment(end[end.length - 1]).hour()
    else
      this.endHour = 17

    if (this.endHour - this.startHour < 5)
      this.endHour = this.startHour + 5;
  }

  fillSpacer() {
    this.filteredEvents.forEach(e => {
      let shift = this.shifts.find(s => s.id == e.id)

      let necessaryElement = document.getElementById('necessary-' + shift.id);
      let scheduledElement = document.getElementById('scheduled-' + shift.id)
      let availableElement = document.getElementById('available-' + shift.id)

      necessaryElement.innerText = shift.participantsRequired + " Nodig";

      let availableNumber = shift.availabilities ? shift.availabilities.filter(a => a.type === 2).length : 0;
      availableElement.innerText = availableNumber + " Beschikbaar";

      let scheduledNumber = shift.availabilities ? shift.availabilities.filter(a => a.type === 3).length : 0
      scheduledElement.innerText = scheduledNumber + " Ingeroosterd";
    })

  }

  OnCheckboxChange($event: MatCheckboxChange) {
    let task = this.availabilityData.projectTasks.find(pt => pt.id == $event.source.id)
    if ($event.checked) {
      if (!this.displayedProjectTasks.includes(task))
        this.displayedProjectTasks.push(task)
    } else {
      this.displayedProjectTasks = this.displayedProjectTasks.filter(t => t !== task)
    }
    this.filterEvents();
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
