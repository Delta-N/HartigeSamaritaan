import {Component, OnInit, ViewChild} from '@angular/core';
import {MatTableDataSource} from "@angular/material/table";
import {MatPaginator} from "@angular/material/paginator";
import {MatSort} from "@angular/material/sort";
import {ActivatedRoute, ParamMap, Router} from "@angular/router";
import {ShiftService} from "../../services/shift.service";
import {BreadcrumbService} from "../../services/breadcrumb.service";
import {Breadcrumb} from "../../models/breadcrumb";
import {TextInjectorService} from "../../services/text-injector.service";
import {SelectionModel} from "@angular/cdk/collections";
import {Schedule} from "../../models/schedule";
import {Scheduledata} from "../../models/scheduledata";
import {ConfirmDialogComponent, ConfirmDialogModel} from "../../components/confirm-dialog/confirm-dialog.component";
import {MatDialog} from "@angular/material/dialog";
import {DateConverter} from "../../helpers/date-converter";
import {AvailabilityService} from "../../services/availability.service";
import {ToastrService} from "ngx-toastr";

@Component({
  selector: 'app-plan-shift',
  templateUrl: './plan-shift.component.html',
  styleUrls: ['./plan-shift.component.scss']
})
export class PlanShiftComponent implements OnInit {
  guid: string;
  loaded: boolean = false;
  title: string = "Plannen";
  dataSource: MatTableDataSource<Schedule> = new MatTableDataSource<Schedule>();
  displayedColumns: string[] = [];
  paginator: MatPaginator;
  sort: MatSort;
  selection = new SelectionModel<Schedule>(true, []);

  scheduledata: Scheduledata;
  addedPersons: Schedule[] = [];
  removedPersons: Schedule[] = [];

  @ViewChild(MatSort) set matSort(ms: MatSort) {
    this.sort = ms;
    this.setDataSourceAttributes()
  }

  @ViewChild(MatPaginator) set matPaginator(mp: MatPaginator) {
    this.paginator = mp;
    this.setDataSourceAttributes()
  }

  constructor(private route: ActivatedRoute,
              private router: Router,
              private shiftService: ShiftService,
              private breadcrumbService: BreadcrumbService,
              private availabilityService: AvailabilityService,
              public dialog: MatDialog,
              private toastr: ToastrService) {
  }

  async ngOnInit(): Promise<void> {
    this.route.paramMap.subscribe((params: ParamMap) => {
      this.guid = params.get('id');
    });

    let previous: Breadcrumb = new Breadcrumb();
    previous.label = "Plannen - Overzicht"
    previous.url = this.breadcrumbService.previousUrl;
    let temp: Breadcrumb = new Breadcrumb();
    temp.label = "Plannen"
    let breadcrumbs: Breadcrumb[] = [this.breadcrumbService.dashboardcrumb, this.breadcrumbService.managecrumb, previous, temp]
    this.breadcrumbService.replace(breadcrumbs);

    this.displayedColumns = TextInjectorService.planShiftTableColumnNames;

    await this.shiftService.getScheduleData(this.guid).then(res => {
      if (res)
        this.scheduledata = res;

      if (this.scheduledata)
        this.dataSource = new MatTableDataSource<Schedule>(this.scheduledata.schedules)
      this.loaded = true;
    })

    this.scheduledata.schedules.forEach(s => {
      if (s.scheduledThisShift)
        this.selection.select(s)
    })

    this.dataSource.filterPredicate = (data, filter) => {
      return DateConverter.calculateAge(data.person.dateOfBirth).toLocaleLowerCase().includes(filter) ||
        (data.person != null && (data.person.firstName + " " + data.person.lastName).toLocaleLowerCase().includes(filter) ||
          (data.person.country == null && 'Onbekend'.toLocaleLowerCase().includes(filter)) ||
          (data.person.country != null && data.person.country.toLocaleLowerCase().includes(filter)) ||
          data.numberOfTimesScheduledThisProject.toString().toLocaleLowerCase().includes(filter))
    }

    this.dataSource.sortingDataAccessor = (item, property) => {
      switch (property) {
        case 'Naam':
          return item.person != null ? (item.person.firstName + " " + item.person.lastName) : null;
        case 'Leeftijd':
          return DateConverter.calculateAge(item.person.dateOfBirth);
        case 'Nationaliteit':
          return item.person != null ? item.person.country : null;
        case '#Ingeroosterd':
          return item.numberOfTimesScheduledThisProject;
        default:
          return item[property];
      }
    };
  }

  applyFilter($event: KeyboardEvent) {
    const filterValue = ($event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  setDataSourceAttributes() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource.data.length;
    return numSelected === numRows;
  }

  checkboxLabel(row?: Schedule): string {
    if (!row) {
      return `${this.isAllSelected() ? 'select' : 'deselect'} all`;
    }
    return `${this.selection.isSelected(row) ? 'deselect' : 'select'} row ${row.availabilityId}`;
  }

  select(row: Schedule) {
    if (this.scheduledata.schedules[this.scheduledata.schedules.indexOf(row)].scheduledThisDay === true) {
      this.confirmDeselect(row);
    } else
      this.addPerson(row)
  }

  confirmDeselect(row: Schedule) {
    const message = "Weet je zeker dat je deze persoon wilt uitroosteren?"
    const dialogData = new ConfirmDialogModel("Bevestig uitroostering", message, "ConfirmationInput",null);
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: "400px",
      data: dialogData
    });
    dialogRef.afterClosed().subscribe(dialogResult => {
      if (dialogResult) {
        row.scheduledThisDay = false;
        this.removedPersons.push(row)
        this.selection.toggle(row)
      }
    })
  }

  addPerson(row) {
    this.selection.toggle(row)
    let index = this.addedPersons.indexOf(row);
    if (index > -1)
      this.addedPersons.splice(index, 1)
    else
      this.addedPersons.push(row)
  }

  async save() {
    if (this.addedPersons.length === 0 && this.removedPersons.length === 0)
      this.toastr.warning("Er hebben geen wijzigingen plaats gevonden")
    else {
      const message = "Weet je zeker dat je deze je deze personen wilt in- en uitroosteren?"
      const dialogData = new ConfirmDialogModel("Bevestig roostering", message, "ConfirmationInput",null);
      const dialogRef = this.dialog.open(ConfirmDialogComponent, {
        maxWidth: "400px",
        data: dialogData
      });
      dialogRef.afterClosed().subscribe(async dialogResult => {
        if (dialogResult) {
          let schedule: Schedule[] = [];

          //check added persons for duplicates with removedPersons and add new schedule to list (to be added)
          if (this.addedPersons && this.addedPersons.length > 0) {
            this.addedPersons.forEach(ap => {
              let s: Schedule = new Schedule();
              s.availabilityId = ap.availabilityId;
              s.person = ap.person;
              s.scheduledThisDay = true;
              //if removed also contains person dont add it
              let index = this.removedPersons.indexOf(ap);
              if (index === -1)
                schedule.push(s)
            })
          }

          //add removed persons to list (to be removed)
          if (this.removedPersons && this.removedPersons.length > 0) {
            this.removedPersons.forEach(rp => {
              let index = this.addedPersons.indexOf(rp)
              if (index === -1) {
                let s: Schedule = new Schedule();
                s.availabilityId = rp.availabilityId;
                s.person = rp.person;
                s.scheduledThisDay = false;
                schedule.push(s)
              }
            })

          }
          if (schedule.length > 0)
            await this.availabilityService.changeAvailabilities(schedule).then(res => {
              if (res) {
                this.toastr.success("Diensten zijn succesvol ingepland")
                this.router.navigate(['manage/plan', this.scheduledata.shift.project.id, this.scheduledata.shift.date])
              } else
                this.toastr.error("Fout tijdens het plannen")
            })
        }
      })
    }
  }
}
