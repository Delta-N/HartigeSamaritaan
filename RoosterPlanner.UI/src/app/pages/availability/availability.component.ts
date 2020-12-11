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
import {Participation} from "../../models/participation";
import {ParticipationService} from "../../services/participation.service";
import {AvailabilityService} from "../../services/availability.service";
import {AvailabilityData} from "../../models/availabilitydata";
import {Task} from 'src/app/models/task';
import {DateConverter} from "../../helpers/date-converter";
import {Availability} from "../../models/availability";


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
  participation: Participation;
  availabilityData: AvailabilityData;
  displayedProjectTasks: Task[] = [];

  shifts: Shift[] = [];

  selectedDate: Moment;


  view: CalendarView = CalendarView.Day;
  viewDate: Date = new Date(); //today

  minDate: Date;
  maxDate: Date;

  startHour: number = 1; // dit aanpassen naar start hour van eerste even
  endHour: number = 23; //  dit aanpassen naar end hour van laatste event
  prevBtnDisabled: boolean;
  nextBtnDisabled: boolean;

  filteredEvents: CalendarEvent[] = [];
  allEvents: CalendarEvent[] = [
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

  maybeLabel: any = this.sanitizer.bypassSecurityTrustHtml('<span class="mat-button custom-button">?</div></span>');
  yesLabel: any = this.sanitizer.bypassSecurityTrustHtml('<span class="mat-button custom-button" id="yess")">V</span>');
  noLabel: any = this.sanitizer.bypassSecurityTrustHtml('<span class="mat-button custom-button">X</span>');


  constructor(private breadcrumbService: BreadcrumbService,
              private shiftService: ShiftService,
              private userService: UserService,
              private participationService: ParticipationService,
              private availabilityService: AvailabilityService,
              private route: ActivatedRoute,
              private sanitizer: DomSanitizer) {
    moment.locale('en')
  }

  ngOnInit(): void {
    this.userId = this.userService.getCurrentUserId();
    this.route.paramMap.subscribe(async (params: ParamMap) => {
      this.projectId = params.get('id');
      await this.getShifts(this.viewDate)

        //create breadcrumbs
        let current: Breadcrumb = new Breadcrumb();
        current.label = 'Beschikbaarheid opgeven';
        let previous: Breadcrumb = new Breadcrumb();
        previous.label = this.shifts[0].project.name
        previous.url = "/project/" + this.shifts[0].project.id
        let array: Breadcrumb[] = [this.breadcrumbService.dashboardcrumb, previous, current];
        this.breadcrumbService.replace(array);

        //get participation including project
        this.participationService.getParticipation(this.userId, this.projectId)
          .then(async res => {
            if (res) {
              this.participation = res
              console.log(res)
              this.minDate = this.participation.project.participationStartDate;
              this.maxDate = this.participation.project.participationEndDate;
            }
          })
        //get basic data
        await this.availabilityService.getAvailabilityData(this.projectId, this.userId)
          .then(res => {
            this.availabilityData = res;
            console.log(res)
            this.displayedProjectTasks = this.availabilityData.projectTasks;
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
    await this.getShifts(this.viewDate);

    this.prevBtnDisabled = moment(this.viewDate).subtract(1, "day") < moment(this.minDate);
    this.nextBtnDisabled = moment(this.viewDate).add(1, "day") > moment(this.maxDate)

    if (this.viewDate < this.minDate) {
      this.changeDate(this.minDate);
    } else if (this.viewDate > this.maxDate) {
      this.changeDate(this.maxDate);
    }
  }

  async handleEvent(action: string, event: CalendarEvent): Promise<void> {
    console.log(event)
    //check if availibility existes
    //find shift
    let shift = this.findShift(event) //is dit reference? denk value based todo testen
    let availability :Availability= null;
    if(shift.availabilities)
      availability = shift.availabilities[0];
    else
      shift.availabilities=[];

    //yes? mod
    if (availability) {
      if (action !== "preference")
        availability.type = this.getAvailabilityType(action)
      else if (action === "preference")
        availability.preference = !availability.preference

      await this.availabilityService.updateAvailability(availability).then(res => {
        if (res) {
          shift.availabilities[0] = res;
        }

      })
    }
    //no? create
    else {
      availability = new Availability();
      availability.participation = this.participation;
      availability.participationId = this.participation.id;
      availability.shift = shift;
      availability.shiftId = shift.id;

      if (action !== "preference")
        availability.type = this.getAvailabilityType(action)
      else if (action === "preference")
        availability.preference = !availability.preference

      await this.availabilityService.postAvailability(availability).then(res => {
        if (res) {
          //add to list of availabilities
          shift.availabilities.push(res)
        }
      })
    }


    //color in calender
    let counter: number = 0;
    let color: string = "Red"
    this.shifts.forEach(s => {
      if (s.availabilities[0]?.type == 3)
        color = "Blue"
      else
        counter++;
    });
    if (counter === this.shifts.length)
      color = "Green";

    this.colorInCalendar(this.viewDate, color)

    //change border
    this.changeBorders(event, action)
  }

  getAvailabilityType(action: string): number {
    let number = 0;
    if (action === "Yes")
      number = 2
    else if (action === "No")
      number = 0
    else if (action === "Maybe")
      number = 1
    return number;
  }

  findShift(event: CalendarEvent): Shift {
    return this.shifts.find(s => s.task.name == event.title && s.endTime == moment(event.end).format("HH:mm") && s.startTime == moment(event.start).format("HH:mm"))
  }


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
        this.handleEvent("Yes", event);
      },
    },
    {
      label: this.maybeLabel,
      a11yLabel: 'Maybe',
      onClick: ({event}: { event: CalendarEvent }): void => {
        this.handleEvent("Maybe", event);
      },
    },
    {
      label: this.noLabel,
      a11yLabel: 'No',
      onClick: ({event}: { event: CalendarEvent }): void => {
        this.handleEvent("No", event);
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


  colorInCalendar(date: Date, color: string) {
    let label = moment(date).format("DD MMMM YYYY").toLowerCase()
    let element: HTMLElement = document.querySelector("[aria-label=" + CSS.escape(label) + "]");
    let child: any = element.children[0]
    child.style.background = color;
    child.style.color = 'white';
  }

  async getShifts(date: Date) {
    date = DateConverter.addOffset(date);
    await this.shiftService.getAllShiftsOnDate(this.projectId, this.userId, date).then(async res => {
      console.log(res)
      this.shifts = res;
    });
    this.addEvents();

  }

  addEvents() {

    this.allEvents = [];
    this.shifts.forEach(s => {
      console.log(s)
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

      this.allEvents.push(event)
    })
    this.filterEvents();
    this.refresh.next();
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
    this.refresh.next();
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
