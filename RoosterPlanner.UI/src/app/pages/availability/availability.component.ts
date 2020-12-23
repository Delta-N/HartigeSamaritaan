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
import {ActivatedRoute, ParamMap} from "@angular/router";
import {Shift} from "../../models/shift";
import {ShiftService} from "../../services/shift.service";
import {
  CalendarView,
  CalendarEvent,
  CalendarDateFormatter,
  CalendarDayViewComponent
} from 'angular-calendar';
import * as moment from "moment"
import {CustomDateFormatter} from "../../helpers/custom-date-formatter.provider";
import {DomSanitizer} from "@angular/platform-browser";
import {MatCalendar} from "@angular/material/datepicker";
import {Moment} from "moment";
import {MatCheckboxChange} from "@angular/material/checkbox";
import {UserService} from "../../services/user.service";
import {BehaviorSubject, Subject} from "rxjs";
import {Participation} from "../../models/participation";
import {ParticipationService} from "../../services/participation.service";
import {AvailabilityService} from "../../services/availability.service";
import {AvailabilityData, ScheduleStatus} from "../../models/availabilitydata";
import {Task} from 'src/app/models/task';
import {Availability} from "../../models/availability";
import {take} from "rxjs/operators";


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
export class AvailabilityComponent implements OnInit, AfterViewInit {
  @ViewChild('calendar') calendar: MatCalendar<Moment>;
  @ViewChild('schedule') schedule: CalendarDayViewComponent;

  projectId: string;
  userId: string;
  participation: Participation;
  availabilityData: AvailabilityData;
  displayedProjectTasks: Task[] = [];
  currentDayProjectTasks: Task[] = [];
  shifts: Shift[] = [];

  selectedDate: Moment;

  view: CalendarView = CalendarView.Day;
  viewDate: Date = new Date(); //today

  minDate: Date;
  maxDate: Date;

  startHour: number = 1;
  endHour: number = 23;
  prevBtnDisabled: boolean;
  nextBtnDisabled: boolean;

  filteredEvents: CalendarEvent[] = [];
  filteredEventsObservable: BehaviorSubject<CalendarEvent[]> = new BehaviorSubject<CalendarEvent[]>(this.filteredEvents);
  allEvents: CalendarEvent[] = [];
  refresh: Subject<any> = new Subject();
  activeProjectTasks: Task[] = [];

  constructor(private breadcrumbService: BreadcrumbService,
              private shiftService: ShiftService,
              private userService: UserService,
              private participationService: ParticipationService,
              private availabilityService: AvailabilityService,
              private route: ActivatedRoute,
              private sanitizer: DomSanitizer,
              private renderer: Renderer2) {
  }

  async ngOnInit(): Promise<void> {
    this.userId = this.userService.getCurrentUserId();
    this.route.paramMap.subscribe(async (params: ParamMap) => {
      this.projectId = params.get('id');

      //get basic data
      await this.availabilityService.getAvailabilityData(this.projectId, this.userId)
        .then(res => {
          this.availabilityData = res;
          this.displayedProjectTasks = this.availabilityData.projectTasks;
        })


      //get participation including project
      await this.participationService.getParticipation(this.userId, this.projectId)
        .then(async res => {
          if (res) {
            this.participation = res

            this.minDate = this.participation.project.participationStartDate >= new Date() ? this.participation.project.participationStartDate : moment().startOf("day").toDate();
            this.maxDate = this.participation.project.participationEndDate;

            this.calendar.minDate = moment(this.minDate);
            this.calendar.maxDate = moment(this.maxDate);
            this.calendar.updateTodaysDate()
            setTimeout(() => {
              this.changeLoadedShiftBordersAndStar()
            }, 750) // dit is best nog wel tricky
          }

          //create breadcrumbs
          let current: Breadcrumb = new Breadcrumb();
          current.label = 'Beschikbaarheid opgeven';
          let previous: Breadcrumb = new Breadcrumb();
          previous.label = this.participation.project.name
          previous.url = "/project/" + this.participation.project.id
          let array: Breadcrumb[] = [this.breadcrumbService.dashboardcrumb, previous, current];
          this.breadcrumbService.replace(array);
        })
    })
  }

  ngAfterViewInit(): void {
    const buttons = document.querySelectorAll('.mat-calendar-previous-button, .mat-calendar-next-button');
    if (buttons) {
      Array.from(buttons).forEach(button => {
        this.renderer.listen(button, 'click', () => {
          this.colorInMonth();
          this.changeLoadedShiftBordersAndStar()
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

      setTimeout(() => {
        this.changeLoadedShiftBordersAndStar()
      }, 300);
    });

    this.prevBtnDisabled = moment(this.viewDate).startOf("day").subtract(1, "day") < moment(this.minDate);
    this.nextBtnDisabled = moment(this.viewDate).startOf("day").add(1, "day") > moment(this.maxDate)
  }

  async handleEvent(action: string, event: CalendarEvent): Promise<void> {
    //check if availibility existes
    //find shift
    let shift = this.findShift(event)
    let availability: Availability = null;

    if (shift.availabilities)
      availability = shift.availabilities[0];
    else
      shift.availabilities = [];

    //yes? mod
    if (availability) {
      if (action !== "Preference")
        availability.type = this.getAvailabilityTypeNumber(action)
      else if (action === "Preference")
        availability.preference = !availability.preference

      await this.availabilityService.updateAvailability(availability)
    }
    //no? create
    else {
      availability = new Availability();
      availability.participation = this.participation;
      availability.participationId = this.participation.id;
      availability.shift = shift;
      availability.shiftId = shift.id;

      if (action !== "Preference")
        availability.type = this.getAvailabilityTypeNumber(action)
      else if (action === "Preference") {
        availability.preference = !availability.preference
        availability.type = 2; // if preference is true then type = 'ok'
        action = "Yes"
      }

      await this.availabilityService.postAvailability(availability).then(res => {
        if (res) {
          //add to list of availabilities
          shift.availabilities.push(res)

          //color in calender
          let counter: number = 0;
          let color: string = "Red"
          this.shifts.forEach(s => {
            if (s.availabilities && s.availabilities.length > 0 && s.availabilities[0].type == 3)
              color = "Blue"
            else if (s.availabilities && s.availabilities.length > 0)
              counter++;
          });
          if (counter === this.shifts.length) {
            color = "Green";
            let daySchedule = new ScheduleStatus();
            daySchedule.date = this.viewDate.toISOString()
            daySchedule.status = 1;
            this.availabilityData.knownAvailabilities.push(daySchedule)
          }

          this.colorInDay(this.viewDate, color)
        }
      })
    }
    this.changeBorders(event, action)
  }

  openInstructions(id: string | number) {
    let url: string = this.shifts.find(s => s.id === id).task?.documentUri;
    if (url)
      window.open(url, "_blank")
  }

  getAvailabilityTypeNumber(action: string): number {
    if (action === "No")
      return 0;
    if (action === "Yes")
      return 2;
  }

  getAvailabilityTypeActionName(actionNumber: number): string {
    if (actionNumber === 0)
      return "No"
    if (actionNumber === 2)
      return "Yes"
    if (actionNumber === 3)
      return "Preference"
  }

  findShift(event: CalendarEvent): Shift {
    return this.shifts.find(s => s.task.name == event.title && s.endTime == moment(event.end).format("HH:mm") && s.startTime == moment(event.start).format("HH:mm"))
  }

  changeLoadedShiftBordersAndStar() {
    this.shifts.forEach(s => {
      if (s.availabilities && s.availabilities.length > 0) {
        let availability = s.availabilities[0];

        let action: string = this.getAvailabilityTypeActionName(availability.type)
        let event: CalendarEvent = this.allEvents.find(e => e.id == s.id)
        if (event && action) {
          this.changeBorders(event, action)
        }
        if (availability && availability.preference === true) {
          this.colorStar(s.id)
        }

      }
    })
  }

  changeBorders(event: CalendarEvent, label: String) {
    if (label === "Preference")
      label = "Yes";
    let farDecendend = this.getActionElement(event)
    for (let k = 0; k < farDecendend.children.length; k++) {
      let child: any = farDecendend.children[k];
      if (child.ariaLabel == label) {
        child.style.border = "solid 2px black";
      } else {
        child.style.border = "none";
      }
    }

  }

  getActionElement(event: CalendarEvent): HTMLElement {
    moment.locale('en')
    let aria = "\n      " + moment(event.start).format("dddd MMMM DD,") + "\n      " + event.title + ", from " + moment(event.start).format("hh:mm A") + "\n     to " +
      moment(event.end).format("hh:mm A");
    let x: HTMLCollection = (document.getElementsByClassName("cal-event"))
    for (let i = 0; i < x.length; i++) {
      let element: any = x[i]
      if (element.ariaLabel == aria) {
        let itemHolder = element.children[0];
        for (let j = 0; j < itemHolder.children.length; j++) {
          let child: any = itemHolder.children[j];
          if (child.id == "actions") {
            return child;
          }
        }
      }
    }
  }

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
    moment.locale("nl")
    await this.shiftService.getAllShiftsOnDateWithUserAvailability(this.projectId, this.userId, moment(date).set("hour", 12).toDate()).then(async res => {
      this.shifts = res;
    });
    if (this.shifts.length > 0) {
      this.addEvents();
    }
  }

  addEvents() {
    let scheduledId: string[] = [];

    this.allEvents = [];
    this.activeProjectTasks = []
    this.shifts.forEach(s => {
      let scheduled = false;
      if (s.availabilities && s.availabilities[0] && s.availabilities[0].type === 3)
        scheduled = true;

      let event: CalendarEvent = {
        start: moment(s.date)
          .set("hour", Number(s.startTime.substring(0, 2)))
          .set("minutes", Number(s.startTime.substring(3, 6))).toDate(),

        end: moment(s.date)
          .set("hour", Number(s.endTime.substring(0, 2)))
          .set("minutes", Number(s.endTime.substring(3, 6))).toDate(),

        title: s.task.name,
        color: this.getColor(s.task.color),
        id: s.id
      };
      if (scheduled) {
        scheduledId.push(s.id)
        event.color.primary = "#5b5bdc";
      }

      this.activeProjectTasks.push(s.task)
      this.allEvents.push(event)
    })
    this.filterEvents();
    this.refresh.next();

    setTimeout(() => {
      this.changeButtonSize();
      if (scheduledId) {
        scheduledId.forEach(id => {
          this.showScheduledButton(id)
          this.hideActionButton(id)
        })
      }
    }, 200)
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

  changePreference(event) {
    this.colorStar(event.id)
    this.handleEvent("Preference", event);
  }

  colorStar(id) {
    let element = document.getElementById(id)
    let icon = element.children[0].children[0];
    if (icon.innerHTML == "star_border") {
      icon.innerHTML = "star"
      icon.classList.add("yellow")
    } else {
      icon.innerHTML = "star_border"
      icon.classList.remove("yellow")
    }
  }

  showScheduledButton(id: string) {
    let searchId = 'scheduledBtn' + id;
    let element = document.getElementById(searchId);
    if (element)
      element.hidden = false;
  }

  hideActionButton(id: string) {
    let element = this.getActionElement(this.allEvents.find(e => e.id === id))
    if (element)
      element.style.display = "none";
  }

  refuseDay() {
    this.allEvents.forEach(e => {
      this.handleEvent("No", e)
    })
  }

  acceptDay() {
    this.allEvents.forEach(e => {
      this.handleEvent("Yes", e)
    })
  }

  changeButtonSize() {
    this.filteredEvents.forEach(e => {
      if ((e.end.getTime() - e.start.getTime()) / 3600000 < 2) {
        let element = this.getActionElement(e)
        for (let i = 0; i < element.children.length; i++) {
          let child: HTMLElement = element.children[i] as HTMLElement
          child.style.width = "26px";
          child.style.height = "26px";
          child.style.fontSize = "0px";
        }
        let fabElement = document.getElementById("scheduledBtn" + e.id)
        if (fabElement)
          fabElement.classList.add("scheduledBtnSmall")
      }
    })
  }
}
