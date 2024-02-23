import {
  Component,
  OnInit,
  AfterViewInit,
  ChangeDetectionStrategy,
  ViewChild,
  Renderer2,
} from '@angular/core';
import {BreadcrumbService} from '../../services/breadcrumb.service';
import {Breadcrumb} from '../../models/breadcrumb';
import {ActivatedRoute, ParamMap, Router} from '@angular/router';
import {Shift} from '../../models/shift';
import {ShiftService} from '../../services/shift.service';
import {
  CalendarView,
  CalendarEvent,
  CalendarDateFormatter,
  CalendarDayViewComponent
} from 'angular-calendar';
import * as moment from 'moment';
import {CustomDateFormatter} from '../../helpers/custom-date-formatter.provider';
import {MatCalendar} from '@angular/material/datepicker';
import {Moment} from 'moment';
import {MatCheckboxChange} from '@angular/material/checkbox';
import {Subject} from 'rxjs';
import {AvailabilityService} from '../../services/availability.service';
import {AvailabilityData} from '../../models/availabilitydata';
import {Task} from 'src/app/models/task';
import {take} from 'rxjs/operators';
import {Project} from '../../models/project';
import {ProjectService} from '../../services/project.service';
import {TextInjectorService} from '../../services/text-injector.service';
import {AvailabilityComponent} from '../availability/availability.component';
import {faCalendarCheck, faCalendarTimes, faHandsHelping} from '@fortawesome/free-solid-svg-icons';


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
  ],
})
export class PlanComponent implements OnInit, AfterViewInit {
  unavailableIcon = faCalendarTimes;
  availableIcon = faCalendarCheck;
  scheduledIcon = faHandsHelping;
  @ViewChild('calendar') calendar: MatCalendar<Moment>;
  @ViewChild('schedule') schedule: CalendarDayViewComponent;

  project: Project;
  availabilityData: AvailabilityData;
  displayedProjectTasks: Task[] = [];
  shifts: Shift[] = [];
  numberOfOverlappingShifts = 0;


  selectedDate: Moment;

  view: CalendarView = CalendarView.Day;
  viewDate: Date;

  minDate: Date;
  maxDate: Date;

  startHour = 12;
  endHour = 17;
  prevBtnDisabled: boolean;
  nextBtnDisabled: boolean;

  filteredEvents: CalendarEvent[] = [];
  allEvents: CalendarEvent[] = [];
  refresh: Subject<any> = new Subject();

  constructor(private breadcrumbService: BreadcrumbService,
              private shiftService: ShiftService,
              private availabilityService: AvailabilityService,
              private route: ActivatedRoute,
              private renderer: Renderer2,
              private projectService: ProjectService,
              private router: Router, ) {
  }

  async ngOnInit(): Promise<void> {

    this.route.paramMap.subscribe(async (params: ParamMap) => {
      const projectId: string = params.get('id');
      const date: string = params.get('date');

      // get basic data
      await this.availabilityService.getAvailabilityDataOfProject(projectId).then(res => {
        if (res) {
          this.availabilityData = res;
        }
        this.displayedProjectTasks = this.availabilityData.projectTasks;
      });

      // getproject
      await this.projectService.getProject(projectId).then(res => {
        if (res) {
          this.project = res;
          this.minDate = moment(this.project.participationStartDate).toDate() >= new Date() ? moment(this.project.participationStartDate).toDate() : moment().startOf('day').toDate();
          this.maxDate = this.project.participationEndDate;
          this.calendar.minDate = moment(this.minDate);
          this.calendar.maxDate = moment(this.maxDate);
          if (date && date !== 'Invalid Date') {
            this.viewDate = moment(date).toDate();
          } else {
            this.viewDate = this.minDate;
          }
          this.calendar.activeDate = moment(this.viewDate);
          this.calendar.updateTodaysDate();
          this.dateOrViewChanged();

        }
      });


      // create breadcrumbs
      const current: Breadcrumb = new Breadcrumb('Plannen', null);

      const array: Breadcrumb[] = [this.breadcrumbService.dashboardcrumb, this.breadcrumbService.managecrumb, current];
      this.breadcrumbService.replace(array);
    });
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
      });
    });

  }

  changeDate(date: Date): void {
    this.viewDate = date;
    this.calendar.selected = moment(this.viewDate);
    this.calendar.activeDate = moment(this.viewDate);
    this.dateOrViewChanged();
  }

  dateChanged() {
    this.calendar.activeDate = this.selectedDate;
    this.changeDate(this.selectedDate.toDate());
  }

  increment(): void {
    this.changeDate(moment(this.viewDate).add(1, 'day').toDate());
  }

  decrement(): void {
    this.changeDate(moment(this.viewDate).subtract(1, 'day').toDate());
  }

  async dateOrViewChanged(): Promise<void> {
    await this.getShifts(this.viewDate).then(() => {
      if (this.viewDate < this.minDate) {
        this.changeDate(this.minDate);
      } else if (this.viewDate > this.maxDate) {
        this.changeDate(this.maxDate);
      }
    });
    this.prevBtnDisabled = moment(this.viewDate).startOf('day').subtract(1, 'day') < moment(this.minDate).startOf('day');
    this.nextBtnDisabled = moment(this.viewDate).startOf('day').add(1, 'day') > moment(this.maxDate).startOf('day');
  }


  Plan(id: string | number) {
    this.router.navigate(['manage/plan/shift', id]).then();

  }

  colorInMonth() {

    for (const ka of this.availabilityData.knownAvailabilities) {
      const date: Date = moment(ka.date).toDate();
      let color = 'Red';
      if (ka.status === 1) {
        color = 'Green';
      }
      else if (ka.status === 2) {
        color = 'Blue';
 }

      this.colorInDay(date, color);
    }
  }

  colorInDay(date: Date, color: string) {
    const label = moment(date).local().format('D MMMM YYYY').toLowerCase();
    const element: HTMLElement = document.querySelector('[aria-label=' + CSS.escape(label) + ']');

    if (element) {
      const child: any = element.children[0];
      child.style.background = color;
      child.style.color = 'white';
    }
  }

  async getShifts(date: Date) {
    await this.shiftService.getAllShiftsOnDate(this.project.id, moment(date).set('hour', 12).toDate()).then(async res => {
      this.shifts = res;
    });
    if (this.shifts.length > 0) {
      this.numberOfOverlappingShifts = AvailabilityComponent.calculateOverlap(this.shifts);
      this.addEvents();
      setTimeout(() => {
        this.fillSpacer();
      }, 100);
    } else {
      this.setDefaultHours();
    }
  }

  addEvents() {

    this.allEvents = [];
    this.shifts.forEach(s => {

      const event: CalendarEvent = {
        start: moment(s.date)
          .set('hour', Number(s.startTime.substring(0, 2)))
          .set('minutes', Number(s.startTime.substring(3, 6))).toDate(),

        end: moment(s.date)
          .set('hour', Number(s.endTime.substring(0, 2)))
          .set('minutes', Number(s.endTime.substring(3, 6))).toDate(),

        title: s.task.name,
        color: TextInjectorService.getColor(s.task.color),
        id: s.id
      };

      this.allEvents.push(event);
    });
    this.filterEvents();
    this.refresh.next();
  }

  filterEvents() {
    this.filteredEvents = [];
    this.allEvents.forEach(e => {
      let contains = false;
      this.displayedProjectTasks.forEach(d => {
        if (d.name == e.title) {
          contains = true;
        }
      });
      if (contains) {
        this.filteredEvents.push(e);
      }
    });


    this.setHours();
  }

  setDefaultHours() {
    this.startHour = 12;
    this.endHour = 17;
    this.refresh.next();
  }

  setHours() {
    const start: Date[] = [];
    this.filteredEvents.forEach(fe => start.push(fe.start));
    start.sort();

    const end: Date[] = [];
    this.filteredEvents.forEach(fe => end.push(fe.end));
    end.sort();

    if (start && start.length > 0) {
      this.startHour = moment(start[0]).hour() > 0 ? moment(start[0]).subtract(1, 'hour').hour() : 0;
    }
    else {
      this.startHour = 12;
    }

    if (end && end.length > 0) {
      this.endHour = moment(end[end.length - 1]).hour();
    }
    else {
      this.endHour = 17;
    }

    if (this.endHour - this.startHour < 5) {
      this.endHour = this.startHour + 5;
    }
  }

  getTitleElement(event: CalendarEvent): HTMLElement {
    return document.getElementById('title-' + event.id);
  }

  fillSpacer() {
    this.filteredEvents.forEach(e => {
        const shift = this.shifts.find(s => s.id == e.id);
        if ((e.end.getTime() - e.start.getTime()) / 3600000 <= 1) {
        // verberg title
          const element = this.getTitleElement(e);
          if (element) {
            element.style.display = 'none';
          }
          for (let i = 0; i < element.children.length; i++) {
            const child: HTMLElement = element.children[i] as HTMLElement;
            child.style.display = 'none';

          }
        }
        if ((e.end.getTime() - e.start.getTime()) / 3600000 > 4) {


          const necessaryElement = document.getElementById('necessary-' + shift.id);
          const scheduledElement = document.getElementById('scheduled-' + shift.id);

          const availableElement = document.getElementById('available-' + shift.id);
          necessaryElement.innerText = shift.participantsRequired + ' Nodig';

          const availableNumber = shift.availabilities ? shift.availabilities.filter(a => a.type === 2).length : 0;
          availableElement.innerText = availableNumber + ' Beschikbaar';

          const scheduledNumber = shift.availabilities ? shift.availabilities.filter(a => a.type === 3).length : 0;
          scheduledElement.innerText = scheduledNumber + ' Ingeroosterd';

        } else {
          const planElement = document.getElementById('plan-' + shift.id);
          planElement.style.cssText = 'padding: 3px !important';
        }

        if (this.numberOfOverlappingShifts > 7) {
          const planElement = document.getElementById('plan-' + shift.id);
          planElement.style.cssText = 'padding: 3px !important';
        }
      }
    );
  }

  OnCheckboxChange($event: MatCheckboxChange) {
    const task = this.availabilityData.projectTasks.find(pt => pt.id == $event.source.id);
    if ($event.checked) {
      if (!this.displayedProjectTasks.includes(task)) {
        this.displayedProjectTasks.push(task);
      }
    } else {
      this.displayedProjectTasks = this.displayedProjectTasks.filter(t => t !== task);
    }
    this.filterEvents();
    setTimeout(() => {
      this.fillSpacer();
    }, 100);
  }
}

