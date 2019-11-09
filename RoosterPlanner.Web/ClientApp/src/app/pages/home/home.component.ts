import { Component, OnInit, ViewChild } from '@angular/core';
import { NgxTuiCalendarComponent } from 'ngx-tui-calendar';
import { JwtHelper } from 'src/app/utilities/jwt-helper';
import { Schedule } from 'ngx-tui-calendar/lib/Models/Schedule';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.less']
})
export class HomeComponent implements OnInit {
  @ViewChild('calendarView', { static: false })
  exampleCalendar: NgxTuiCalendarComponent;
  showNewUser = false;

  schedules: Schedule[];

  constructor() { }

  ngOnInit() {
    const idToken = JwtHelper.decodeToken(
      sessionStorage.getItem('msal.idtoken')
    );
    if (idToken && idToken.newUser) {
      this.showNewUser = true;
    }

    this.schedules = [
      {
        id: '1',
        calendarId: '1',
        title: 'my schedule1',
        category: 'time',
        dueDateClass: '',
        start: (new Date()),
        end: (new Date())
      },
      {
        id: '2',
        calendarId: '1',
        title: 'my schedule2',
        category: 'time',
        dueDateClass: '',
        start: (new Date()),
        end: (new Date())
      },
      {
        id: '3',
        calendarId: '1',
        title: 'my schedule3',
        category: 'time',
        dueDateClass: '',
        start: (new Date(2019, 11, 9, 3, 12)),
        end: (new Date(2019, 11, 9, 3, 42))
      },
      {
        id: '4',
        calendarId: '2',
        title: 'my schedule4',
        category: 'time',
        dueDateClass: '',
        start: (new Date(2019, 11, 9, 1, 12)),
        end: (new Date(2019, 11, 9, 14, 12))
      },
    ];
    console.log(this.schedules);
  }

  onTuiCalendarCreate($event) {
    console.log($event);
    this.exampleCalendar.changeView('month');
    /* at this point the calendar has been created and it's methods are available via the ViewChild defined above, so for example you can do: */
    this.exampleCalendar.createSchedules([
      /* populated schedules array goes here*/
    ]);
  }

  public setView(viewName: string): void {
    console.log(this.exampleCalendar);
    this.exampleCalendar.changeView(viewName);
    this.exampleCalendar.setOptions({ taskView: true, defaultView: 'week', scheduleView: true}, false);
  }

  onSchedule(schedule) {
    console.log('schedule', schedule);
  }

}
