import {Component, OnInit, ViewChild} from '@angular/core';
import {Location} from '@angular/common';
import {Project} from "../../models/project";
import {Task} from "../../models/task";
import {FormBuilder, FormControl, Validators} from "@angular/forms";
import {MultipleDatesComponent} from "ngx-multiple-dates";
import {TaskService} from "../../services/task.service";
import {ProjectService} from "../../services/project.service";
import {ActivatedRoute, ParamMap} from "@angular/router";
import {ToastrService} from "ngx-toastr";
import {ShiftService} from "../../services/shift.service";
import {Moment} from "moment";
import {DateConverter} from "../../helpers/date-converter";
import {Shift} from "../../models/shift";
import {EntityHelper} from "../../helpers/entity-helper";
import * as moment from "moment";
import {TextInjectorService} from "../../services/text-injector.service";
import {BreadcrumbService} from "../../services/breadcrumb.service";
import {Breadcrumb} from "../../models/breadcrumb";

@Component({
  selector: 'app-add-shifts',
  templateUrl: './add-shifts.component.html',
  styleUrls: ['./add-shifts.component.scss']
})
export class AddShiftsComponent implements OnInit {
  guid: string;
  loaded: boolean = false;
  title: string = "Shift toevoegen";

  project: Project;
  tasks: Task[];
  requiredParticipants: number;

  checkoutForm;
  taskControl: FormControl;
  startTimeControl: FormControl;
  endTimeControl: FormControl;
  participantsRequiredControl: FormControl;
  daySelectionControl: FormControl;
  selectionOptions: string[];

  shiftDates: Date[] = [];
  min: Date;
  max: Date;
  removable = true;
  selectable = true;
  @ViewChild('days') datepicker: MultipleDatesComponent;

  constructor(
    private taskService: TaskService,
    private projectService: ProjectService,
    private route: ActivatedRoute,
    private formBuilder: FormBuilder,
    private toastr: ToastrService,
    private shiftService: ShiftService,
    private _location: Location,
    private breadcrumbService: BreadcrumbService) {

    this.selectionOptions = TextInjectorService.calenderSelectionOptions;

    this.taskControl = new FormControl('', Validators.required);
    this.startTimeControl = new FormControl('', Validators.required);
    this.endTimeControl = new FormControl('', Validators.required);
    this.participantsRequiredControl = new FormControl('', Validators.required);
    this.daySelectionControl = new FormControl('');

    this.checkoutForm = this.formBuilder.group({
      task: this.taskControl,
      start: this.startTimeControl,
      end: this.endTimeControl,
      participantsRequired: this.participantsRequiredControl,
    })
  }

  async ngOnInit(): Promise<void> {
    this.route.paramMap.subscribe((params: ParamMap) => {
      this.guid = params.get('id');

      let previous: Breadcrumb = new Breadcrumb("Shift overzicht", "/manage/shifts/" + this.guid);
      let current: Breadcrumb = new Breadcrumb("Shift toevoegen", null);

      let breadcrumbs: Breadcrumb[] = [this.breadcrumbService.dashboardcrumb, this.breadcrumbService.managecrumb, previous, current]
      this.breadcrumbService.replace(breadcrumbs);
    });

    this.taskService.getAllProjectTasks(this.guid).then(tasks => {
      this.tasks = tasks.filter(t => t != null);
    });

    await this.projectService.getProject(this.guid).then(async project => {
      this.project = project;
      this.min = new Date(this.project.participationStartDate) < new Date() ? new Date() : this.project.participationStartDate;
      this.max = this.project.participationEndDate;

      this.loaded = true;
    })
  }

  getDatesBetweenDates = (startDate, endDate) => {
    let dates = []
    const theDate = new Date(startDate)
    while (theDate <= endDate) {
      dates = [...dates, new Date(theDate)]
      theDate.setDate(theDate.getDate() + 1)
    }
    return dates
  }

  addAllDays() {
    let currentDates: any[] = this.shiftDates;
    let allDates: Date[] = this.getDatesBetweenDates(new Date(this.min), new Date(this.max));

    //filter alldates depending on action
    switch (this.daySelectionControl.value) {
      case this.selectionOptions[0]: {
        break;
      }
      case this.selectionOptions[1]: {
        allDates = allDates.filter(d => d.getDay() === 1)
        break;
      }
      case this.selectionOptions[2]: {
        allDates = allDates.filter(d => d.getDay() === 2)
        break;
      }
      case this.selectionOptions[3]: {
        allDates = allDates.filter(d => d.getDay() === 3)
        break;
      }
      case this.selectionOptions[4]: {
        allDates = allDates.filter(d => d.getDay() === 4)
        break;
      }
      case this.selectionOptions[5]: {
        allDates = allDates.filter(d => d.getDay() === 5)
        break;
      }
      case this.selectionOptions[6]: {
        allDates = allDates.filter(d => d.getDay() === 6)
        break;
      }
      case this.selectionOptions[7]: {
        allDates = allDates.filter(d => d.getDay() === 0)
        break;
      }
      case this.selectionOptions[8]: {
        allDates = [];
        currentDates = [];
        break;
      }
      default: {
        allDates = [];
        break;
      }
    }

    allDates.forEach(date => {
      let found: boolean = false;
      let dateMoment: Moment = moment([date.getFullYear(), date.getMonth(), date.getDate()])

      currentDates.forEach(storedDate => {
        if (dateMoment.toJSON() === storedDate.toJSON()) {
          found = true;
        }
      })
      if (!found) {
        currentDates.push(dateMoment)
      }
    })
    this.datepicker.writeValue(currentDates)
    this.shiftDates.sort((a, b) => a.valueOf() - b.valueOf());
  }


  remove(day: Date) {
    const index = this.shiftDates.indexOf(day)
    if (index >= 0) {
      this.datepicker.removeChip(day);
    }
  }

  timeBefore(startTime: string, endTime: string): boolean {
    let start: Date = new Date(null, null, null, parseInt(startTime.substring(0, 2)), parseInt(startTime.substring(3, 5)));
    let end: Date = new Date(null, null, null, parseInt(endTime.substring(0, 2)), parseInt(endTime.substring(3, 5)));
    return end.valueOf() - start.valueOf() >= 0
  }

  async save(value: any) {
    if (this.checkoutForm.status === 'INVALID' || !this.shiftDates.length) {
      this.toastr.error("Niet alle velden zijn correct ingevuld")
    } else {
      if (!this.timeBefore(value.start, value.end)) {
        this.toastr.error("Starttijd moet voor eindtijd komen")
        return;
      }
      if (value.par <= 0) {
        this.toastr.error("Een shift heeft minimaal 1 vrijwilliger nodig")
        return;
      }

      let shifts: Shift[] = []
      this.shiftDates.forEach(shiftdate => {
        let currentShift: Shift = new Shift();
        currentShift.id = EntityHelper.returnEmptyGuid();
        currentShift.project = this.project
        currentShift.task = value.task;
        currentShift.participantsRequired = value.participantsRequired;
        currentShift.startTime = value.start;
        currentShift.endTime = value.end;

        if (shiftdate instanceof moment) {
          currentShift.date = DateConverter.addOffset(new Date(shiftdate.valueOf()));
        } else {
          currentShift.date = DateConverter.addOffset(shiftdate);
        }
        shifts.push(currentShift);
      })
      if (shifts && shifts.length) {
        await this.shiftService.postShifts(shifts).then(res => {
          if (res && res.length) {
            this._location.back();
          }
        });
      }
    }
  }
}
