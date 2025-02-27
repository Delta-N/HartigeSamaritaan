import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { FormsModule } from '@angular/forms';
import {
  MAT_MOMENT_DATE_ADAPTER_OPTIONS,
  MAT_MOMENT_DATE_FORMATS,
  MomentDateAdapter,
} from '@angular/material-moment-adapter';
import { MatChipsModule } from '@angular/material/chips';
import {
  DateAdapter,
  MAT_DATE_FORMATS,
  MAT_DATE_LOCALE,
} from '@angular/material/core';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { NgbPopoverModule } from '@ng-bootstrap/ng-bootstrap';
import {
  DateAdapter as CalendarDateAdapter,
  CalendarDateFormatter,
  CalendarModule,
  CalendarMomentDateFormatter,
} from 'angular-calendar';
import { adapterFactory } from 'angular-calendar/date-adapters/date-fns';

import * as moment from 'moment';
import { NgxMaterialTimepickerModule } from 'ngx-material-timepicker';
import { NgxMultipleDatesModule } from 'ngx-multiple-dates';
import { AddProjectTaskComponent } from '../../components/add-project-task/add-project-task.component';
import { AddShiftsComponent } from '../../components/add-shifts/add-shifts.component';
import { EditShiftComponent } from '../../components/edit-shift/edit-shift.component';
import { EmailDialogComponent } from '../../components/email-dialog/email-dialog.component';
import {
  AgePipe,
  AvailabilityPipe,
  CalendarTooltip,
  CheckboxFilter,
  DatePipe,
  PlanTooltip,
  ScheduledCount,
  ScheduledPipe,
  TaskFilterPipe,
} from '../../helpers/filter.pipe';
import { ManageComponent } from '../../pages/manage/manage.component';
import { PlanShiftComponent } from '../../pages/plan-shift/plan-shift.component';
import { PlanComponent } from '../../pages/plan/plan.component';
import { ShiftOverviewComponent } from '../../pages/shift-overview/shift-overview.component';
import { ShiftComponent } from '../../pages/shift/shift.component';
import { MaterialModule } from '../material/material.module';
import { ManageRoutingModule } from './manage-routing.module';
import { calender } from '../shared/calendar.module';

// export function momentAdapterFactory() {
//   return adapterFactory(moment);
// }

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
    MatChipsModule,
    NgbPopoverModule,
    FontAwesomeModule,
    calender,
  ],
  exports: [
    DatePipe,
    CalendarTooltip,
    CheckboxFilter,
    AgePipe,
    FontAwesomeModule,
    CalendarModule,
  ],
  providers: [
    {
      provide: MAT_DATE_LOCALE,
      useValue: 'nl-NL',
    },
    {
      provide: DateAdapter,
      useClass: MomentDateAdapter,
      deps: [MAT_DATE_LOCALE, MAT_MOMENT_DATE_ADAPTER_OPTIONS],
    },
    {
      provide: MAT_DATE_FORMATS,
      useValue: MAT_MOMENT_DATE_FORMATS,
    },
  ],
})
export class ManageModule {}
