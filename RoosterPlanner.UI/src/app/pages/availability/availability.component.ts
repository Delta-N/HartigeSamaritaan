import {Component, OnInit} from '@angular/core';
import {BreadcrumbService} from "../../services/breadcrumb.service";
import {Breadcrumb} from "../../models/breadcrumb";
import {ActivatedRoute, ParamMap} from "@angular/router";
import {Shift} from "../../models/shift";
import {ShiftService} from "../../services/shift.service";

@Component({
  selector: 'app-availability',
  templateUrl: './availability.component.html',
  styleUrls: ['./availability.component.scss']
})
export class AvailabilityComponent implements OnInit {
  shifts: Shift[] = [];
  view: CalendarView = CalendarView.Day;

  viewDate: Date = new Date();

  handleEvent(action: string, event: CalendarEvent): void {
    /*this.modalData = {event, action};
    this.modal.open(this.modalContent, {size: 'lg'});*/
  }


  actions: CalendarEventAction[] = [
    {
      label: '<i class="fas fa-fw fa-pencil-alt"></i>',
      a11yLabel: 'Edit',
      onClick: ({event}: { event: CalendarEvent }): void => {
        this.handleEvent('Edited', event);
      },
    },
    {
      label: '<i class="fas fa-fw fa-trash-alt"></i>',
      a11yLabel: 'Delete',
      onClick: ({event}: { event: CalendarEvent }): void => {
        this.events = this.events.filter((iEvent) => iEvent !== event);
        this.handleEvent('Deleted', event);
      },
    },
  ];

  events: CalendarEvent[] = [

    {
      start: subDays(endOfMonth(new Date()), 3),
      end: addDays(endOfMonth(new Date()), 3),
      title: 'A long event that spans 2 months',
      color: colors.blue,
      allDay: true,
    },
    {
      start: addHours(startOfDay(new Date()), 2),
      end: addHours(new Date(), 2),
      title: 'A draggable and resizable event',
      color: colors.yellow,
      actions: this.actions,
      resizable: {
        beforeStart: true,
        afterEnd: true,
      },
      draggable: true,
    },
  ];

  constructor(private breadcrumbService: BreadcrumbService,
              private shiftService: ShiftService,
              private route: ActivatedRoute) {


  }

  ngOnInit(): void {
    this.route.paramMap.subscribe(async (params: ParamMap) => {
      await this.shiftService.getAllShifts(params.get('id')).then(async res => {
        this.shifts = res;
        //create breadcrumbs
        let current: Breadcrumb = new Breadcrumb();
        current.label = 'Beschikbaarheid opgeven';
        let previous: Breadcrumb = new Breadcrumb();
        previous.label = this.shifts[0].project.name
        previous.url = "/project/" + this.shifts[0].project.id

        let array: Breadcrumb[] = [this.breadcrumbService.dashboardcrumb, previous, current];
        this.breadcrumbService.replace(array);
      });
    });


  }

}
