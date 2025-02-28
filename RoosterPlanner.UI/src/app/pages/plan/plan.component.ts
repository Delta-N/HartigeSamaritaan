import {
  AfterViewInit,
  ChangeDetectionStrategy,
  ChangeDetectorRef,
  Component,
  inject,
  OnInit,
  Renderer2,
  viewChild,
} from '@angular/core';
import { BreadcrumbService } from '../../services/breadcrumb.service';
import { Breadcrumb } from '../../models/breadcrumb';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';
import { Shift } from '../../models/shift';
import { ShiftService } from '../../services/shift.service';
import {
  CalendarDateFormatter,
  CalendarDayViewComponent,
  CalendarEvent,
  CalendarView,
} from 'angular-calendar';
import moment from 'moment';
import { Moment } from 'moment';
import { CustomDateFormatter } from '../../helpers/custom-date-formatter.provider';
import { MatCalendar } from '@angular/material/datepicker';
import { MatCheckboxChange } from '@angular/material/checkbox';
import { from, Subject } from 'rxjs';
import { AvailabilityService } from '../../services/availability.service';
import { AvailabilityData } from '../../models/availabilitydata';
import { Task } from 'src/app/models/task';
import { switchMap, take, tap } from 'rxjs/operators';
import { Project } from '../../models/project';
import { ProjectService } from '../../services/project.service';
import { TextInjectorService } from '../../services/text-injector.service';
import { AvailabilityComponent } from '../availability/availability.component';
import {
  faCalendarCheck,
  faCalendarTimes,
  faHandsHelping,
} from '@fortawesome/free-solid-svg-icons';

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
  private readonly changeDetector = inject(ChangeDetectorRef);
  unavailableIcon = faCalendarTimes;
  availableIcon = faCalendarCheck;
  scheduledIcon = faHandsHelping;
  readonly calendar = viewChild.required<MatCalendar<Moment>>(
    MatCalendar<Moment>,
  );
  readonly schedule = viewChild.required<CalendarDayViewComponent>(
    CalendarDayViewComponent,
  );

  project: Project;
  availabilityData: AvailabilityData;
  displayedProjectTasks: Task[] = [];
  shifts: Shift[] = [];
  numberOfOverlappingShifts = 0;

  view: CalendarView = CalendarView.Day;
  currentDate: Moment;

  minDate: Moment;
  maxDate: Moment;

  startHour = 12;
  endHour = 17;
  prevBtnDisabled: boolean;
  nextBtnDisabled: boolean;

  filteredEvents: CalendarEvent[] = [];
  allEvents: CalendarEvent[] = [];
  refresh: Subject<void> = new Subject();

  constructor(
    private breadcrumbService: BreadcrumbService,
    private shiftService: ShiftService,
    private availabilityService: AvailabilityService,
    private route: ActivatedRoute,
    private renderer: Renderer2,
    private projectService: ProjectService,
    private router: Router,
  ) {}

  ngOnInit() {
    this.route.paramMap.subscribe((params: ParamMap) => {
      const projectId: string = params.get('id');
      const date: string = params.get('date');

      //get basic data
      from(
        this.availabilityService.getAvailabilityDataOfProject(projectId),
      ).subscribe((res) => {
        if (res) {
          this.availabilityData = res;
        }
        this.displayedProjectTasks = this.availabilityData.projectTasks;
        this.filterEvents();
      });

      from(this.projectService.getProject(projectId)).subscribe((res) => {
        if (res) {
          this.project = res;
          this.minDate =
            moment(this.project.participationStartDate).toDate() >= new Date()
              ? moment(this.project.participationStartDate)
              : moment().startOf('day');

          this.maxDate = moment(this.project.participationEndDate);
          this.currentDate = moment(
            date && date !== 'Invalid Date' ? date : this.minDate,
          );

          this.syncMatCalender();
          this.getShifts(this.currentDate.toDate()).subscribe();
        }
      });

      //create breadcrumbs
      const current: Breadcrumb = new Breadcrumb('Plannen', null);

      const array: Breadcrumb[] = [
        this.breadcrumbService.dashboardcrumb,
        this.breadcrumbService.managecrumb,
        current,
      ];
      this.breadcrumbService.replace(array);
    });
  }

  ngAfterViewInit(): void {
    const buttons = document.querySelectorAll(
      '.mat-calendar-previous-button, .mat-calendar-next-button',
    );

    if (buttons) {
      Array.from(buttons).forEach((button) => {
        this.renderer.listen(button, 'click', () => {
          this.colorInMonth();
        });
      });
    }

    const calendar = this.calendar();

    calendar.stateChanges
      .pipe(
        take(1),
        switchMap(() => this.getShifts(this.currentDate.toDate())),
        tap(() => this.colorInMonth()),
      )
      .subscribe();
  }

  changeDate(date: Moment): void {
    this.currentDate = date;
    this.syncMatCalender();
    this.updateShifts();
  }

  setSelectedCalenderDate(newDate: Moment) {
    this.changeDate(newDate);
  }

  increment(): void {
    const newDate = this.currentDate.add(1, 'day');
    this.changeDate(newDate);
  }

  decrement(): void {
    const newDate = this.currentDate.subtract(1, 'day');
    this.changeDate(newDate);
  }

  syncMatCalender() {
    const calendar = this.calendar();
    calendar.selected = this.currentDate;
    calendar.activeDate = this.currentDate;
    calendar.monthView.activeDate = this.currentDate;
  }

  updateShifts() {
    if (this.currentDate < this.minDate) {
      this.changeDate(this.minDate);
    } else if (this.currentDate > this.maxDate) {
      this.changeDate(this.maxDate);
    }

    this.getShifts(this.currentDate.toDate()).subscribe(() => {
      this.changeDetector.detectChanges();
    });
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
      } else if (ka.status === 2) {
        color = 'Blue';
      }

      this.colorInDay(date, color);
    }
  }

  colorInDay(date: Date, color: string) {
    const label = moment(date).local().format('D MMMM YYYY').toLowerCase();
    const element: HTMLElement = document.querySelector(
      '[aria-label=' + CSS.escape(label) + ']',
    );

    if (element) {
      const child: any = element.children[0];
      child.style.background = color;
      child.style.color = 'white';
    }
  }

  getShifts(date: Date) {
    return from(
      this.shiftService.getAllShiftsOnDate(
        this.project.id,
        moment(date).set('hour', 12).toDate(),
      ),
    ).pipe(
      tap((shifts) => {
        this.shifts = shifts;

        if (!this.shifts.length) {
          this.setDefaultHours();
        }

        this.numberOfOverlappingShifts = AvailabilityComponent.calculateOverlap(
          this.shifts,
        );

        this.allEvents = this.shifts.map(
          (s): CalendarEvent => ({
            start: moment(s.date)
              .set('hour', Number(s.startTime.substring(0, 2)))
              .set('minutes', Number(s.startTime.substring(3, 6)))
              .toDate(),

            end: moment(s.date)
              .set('hour', Number(s.endTime.substring(0, 2)))
              .set('minutes', Number(s.endTime.substring(3, 6)))
              .toDate(),

            title: s.task.name,
            color: TextInjectorService.getColor(s.task.color),
            id: s.id,
          }),
        );

        this.filterEvents();

        this.prevBtnDisabled =
          moment(this.currentDate).startOf('day').subtract(1, 'day') <
          moment(this.minDate).startOf('day');
        this.nextBtnDisabled =
          moment(this.currentDate).startOf('day').add(1, 'day') >
          moment(this.maxDate).startOf('day');

        setTimeout(() => {
          this.fillSpacer();
        }, 100);
      }),
    );
  }

  filterEvents() {
    this.filteredEvents = this.allEvents.filter((e) =>
      this.displayedProjectTasks.some((d) => d.name == e.title),
    );

    this.setHours();
    this.refresh.next();
  }

  setDefaultHours() {
    this.startHour = 12;
    this.endHour = 17;
    this.refresh.next();
  }

  setHours() {
    const start = this.filteredEvents.map((e) => e.start);
    const end = this.filteredEvents.map((e) => e.end);

    start.sort();
    end.sort();

    if (start && start.length > 0) {
      this.startHour =
        moment(start[0]).hour() > 0
          ? moment(start[0]).subtract(1, 'hour').hour()
          : 0;
    } else {
      this.startHour = 12;
    }

    if (end && end.length > 0) {
      this.endHour = moment(end[end.length - 1]).hour();
    } else {
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
    this.filteredEvents.forEach((e) => {
      const shift = this.shifts.find((s) => s.id == e.id);
      if ((e.end.getTime() - e.start.getTime()) / 3600000 <= 1) {
        //verberg title
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
        const necessaryElement = document.getElementById(
          'necessary-' + shift.id,
        );
        const scheduledElement = document.getElementById(
          'scheduled-' + shift.id,
        );

        const availableElement = document.getElementById(
          'available-' + shift.id,
        );
        necessaryElement.innerText = shift.participantsRequired + ' Nodig';

        const availableNumber = shift.availabilities
          ? shift.availabilities.filter((a) => a.type === 2).length
          : 0;
        availableElement.innerText = availableNumber + ' Beschikbaar';

        const scheduledNumber = shift.availabilities
          ? shift.availabilities.filter((a) => a.type === 3).length
          : 0;
        scheduledElement.innerText = scheduledNumber + ' Ingeroosterd';
      } else {
        const planElement = document.getElementById('plan-' + shift.id);
        planElement.style.cssText = 'padding: 3px !important';
      }

      if (this.numberOfOverlappingShifts > 7) {
        const planElement = document.getElementById('plan-' + shift.id);
        planElement.style.cssText = 'padding: 3px !important';
      }
    });
  }

  OnCheckboxChange($event: MatCheckboxChange) {
    const task = this.availabilityData.projectTasks.find(
      (pt) => pt.id == $event.source.id,
    );
    if ($event.checked) {
      if (!this.displayedProjectTasks.includes(task)) {
        this.displayedProjectTasks.push(task);
      }
    } else {
      this.displayedProjectTasks = this.displayedProjectTasks.filter(
        (t) => t !== task,
      );
    }
    this.filterEvents();
    setTimeout(() => {
      this.fillSpacer();
    }, 100);
  }
}
