import {
  DateAdapter,
  CalendarDateFormatter,
  CalendarModule,
  CalendarMomentDateFormatter,
} from 'angular-calendar';
import { adapterFactory } from 'angular-calendar/date-adapters/moment';
import moment from 'moment';

export function momentAdapterFactory() {
  return adapterFactory(moment);
}

export const calender = CalendarModule.forRoot(
  {
    provide: DateAdapter,
    useFactory: momentAdapterFactory,
  },
  {
    dateFormatter: {
      provide: CalendarDateFormatter,
      useClass: CalendarMomentDateFormatter,
    },
  },
);
