import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';

import {ManageRoutingModule} from './manage-routing.module';
import {ShiftOverviewComponent} from "../../pages/shift-overview/shift-overview.component";
import {AddShiftsComponent} from "../../components/add-shifts/add-shifts.component";
import {EditShiftComponent} from "../../components/edit-shift/edit-shift.component";
import {PlanComponent} from "../../pages/plan/plan.component";
import {PlanShiftComponent} from "../../pages/plan-shift/plan-shift.component";
import {
  AgePipe,
  AvailabilityPipe, CalendarTooltip, CheckboxFilter, DatePipe,
  PlanTooltip,
  ScheduledCount, ScheduledPipe, TaskFilterPipe
} from "../../helpers/filter.pipe";
import {ManageComponent} from "../../pages/manage/manage.component";
import {MaterialModule} from "../material/material.module";
import {CalendarDateFormatter, CalendarModule, CalendarMomentDateFormatter} from "angular-calendar";
import {DateAdapter as CalendarDateAdapter} from "angular-calendar/date-adapters/date-adapter";
import {NgxMaterialTimepickerModule} from "ngx-material-timepicker";
import {NgxMultipleDatesModule} from "ngx-multiple-dates";
import {AddProjectTaskComponent} from "../../components/add-project-task/add-project-task.component";
import {adapterFactory} from "angular-calendar/date-adapters/moment";
import * as moment from "moment";
import {DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE} from "@angular/material/core";
import {
  MAT_MOMENT_DATE_ADAPTER_OPTIONS,
  MAT_MOMENT_DATE_FORMATS,
  MomentDateAdapter
} from "@angular/material-moment-adapter";
import {ShiftComponent} from "../../pages/shift/shift.component";
import {EmailDialogComponent} from "../../components/email-dialog/email-dialog.component";

export function momentAdapterFactory() {
  return adapterFactory(moment);
}


@NgModule({
  declarations: [
    ManageComponent,
    ShiftOverviewComponent,
    AddShiftsComponent,
    EditShiftComponent,
    PlanComponent,
    PlanShiftComponent,
    ScheduledCount,
    DatePipe,
    CalendarTooltip,
    CheckboxFilter,
    AddProjectTaskComponent,
    ShiftComponent,
    PlanTooltip,
    AvailabilityPipe,
    TaskFilterPipe,
    EmailDialogComponent,

    ScheduledPipe,
    AgePipe,
  ],
  imports: [
    CommonModule,
    ManageRoutingModule,
    MaterialModule,
    NgxMaterialTimepickerModule,
    NgxMultipleDatesModule,
    CalendarModule.forRoot(
      {
        provide: CalendarDateAdapter,
        useFactory: momentAdapterFactory,
      },
      {
        dateFormatter: {
          provide: CalendarDateFormatter,
          useClass: CalendarMomentDateFormatter,
        },
      }
    ),

  ],
    exports: [
        DatePipe,
        CalendarTooltip,
        CheckboxFilter,
        AgePipe
    ],
  providers: [
    {
      provide: MAT_DATE_LOCALE,
      useValue: 'nl-NL'
    },
    {
      provide: DateAdapter,
      useClass: MomentDateAdapter,
      deps: [MAT_DATE_LOCALE, MAT_MOMENT_DATE_ADAPTER_OPTIONS]
    },
    {
      provide: MAT_DATE_FORMATS, useValue: MAT_MOMENT_DATE_FORMATS
    },
  ]
})
export class ManageModule {
}
