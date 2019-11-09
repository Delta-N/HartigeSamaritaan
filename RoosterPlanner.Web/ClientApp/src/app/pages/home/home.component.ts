import { Component, OnInit, ViewChild } from '@angular/core';
import { NgxTuiCalendarComponent } from 'ngx-tui-calendar';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.less']
})
export class HomeComponent implements OnInit {
  @ViewChild('exampleCalendar', { static: false })
  exampleCalendar: NgxTuiCalendarComponent;

  constructor() {}

  ngOnInit() {}

  onTuiCalendarCreate($event) {
    /* at this point the calendar has been created and it's methods are available via the ViewChild defined above, so for example you can do: */
    this.exampleCalendar.createSchedules([
      /* populated schedules array goes here*/
    ]);
  }
}
