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
  @ViewChild('calendarView', { static: true })
  exampleCalendar: NgxTuiCalendarComponent;
  showNewUser = false;

  schedules: Schedule[];

  constructor() {}

  ngOnInit() {
    const idToken = JwtHelper.decodeToken(
      sessionStorage.getItem('msal.idtoken')
    );
    if (idToken && idToken.newUser) {
      this.showNewUser = true;
    }
    this.exampleCalendar.changeView('day');

  }

  // onTuiCalendarCreate($event) {


  //   this.exampleCalendar.createSchedules([
  //     /* populated schedules array goes here*/
  //   ]);

  //   this.exampleCalendar.changeView('day');

  //   this.exampleCalendar.setOptions(
  //     { taskView: true, defaultView: 'day', scheduleView: true },
  //     false
  //   );
  // }

  // public setView(): void {
  //   this.exampleCalendar.changeView('day');

  //   this.exampleCalendar.setOptions(
  //     { taskView: true, defaultView: 'day', scheduleView: true },
  //     false
  //   );
  // }

  // onSchedule(schedule) {
  //   console.log('schedule', schedule);
  // }
}
