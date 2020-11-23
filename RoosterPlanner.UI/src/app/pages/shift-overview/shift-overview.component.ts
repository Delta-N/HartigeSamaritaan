import {Component, OnInit, ViewChild} from '@angular/core';
import {Project} from "../../models/project";
import {ActivatedRoute, ParamMap, Router} from "@angular/router";
import {ProjectService} from "../../services/project.service";
import {Shift} from "../../models/shift";
import {Task} from "../../models/task";
import {MatPaginator} from "@angular/material/paginator";
import {MatSort} from '@angular/material/sort';
import {MatTableDataSource} from "@angular/material/table";
import {ShiftService} from "../../services/shift.service";

@Component({
  selector: 'app-shift-overview',
  templateUrl: './shift-overview.component.html',
  styleUrls: ['./shift-overview.component.scss']
})
export class ShiftOverviewComponent implements OnInit {
  guid: string;
  loaded: boolean = false;
  title: string = "Shift overzicht";

  project: Project;

  displayedColumns: string[] = ['Taak', 'Datum', 'Vanaf', 'Tot', '#Benodigde vrijwilligers'];
  dataSource: MatTableDataSource<Shift>;
  paginator: MatPaginator;
  sort: MatSort;

  @ViewChild(MatSort) set matSort(ms: MatSort) {
    this.sort = ms;
    this.setDataSourceAttributes();
  }

  @ViewChild(MatPaginator) set matPaginator(mp: MatPaginator) {
    this.paginator = mp;
    this.setDataSourceAttributes();
  }

  constructor(private route: ActivatedRoute,
              private projectService: ProjectService,
              private router: Router,
              private shiftService: ShiftService) {
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe((params: ParamMap) => {
      this.guid = params.get('id');
    });
    this.projectService.getProject(this.guid).then(project => {
      this.project = project;
      this.title += ": " + this.project.name
      this.loaded = true;
    })
    //PLACEHOLDERS REPLACE WHEN BACKEND IS DONE
    let testShift: Shift = new Shift();
    testShift.date = new Date("11/23/2020");
    testShift.endTime = "15:00";
    testShift.id = "00000000-0000-0000-0000-000000000010";
    testShift.participantsRequired = 1;
    testShift.project = new Project();
    testShift.startTime = "13:00";
    testShift.task = new Task();
    testShift.task.color = "Blue";
    testShift.task.name = "Chef";

    let testShift2: Shift = new Shift();
    testShift2.date = new Date("11/25/2020");
    testShift2.endTime = "15:00";
    testShift2.id = "00000000-0000-0000-0000-000000000020";
    testShift2.participantsRequired = 5;
    testShift2.project = new Project();
    testShift2.startTime = "12:00";
    testShift2.task = new Task();
    testShift2.task.color = "Blue";
    testShift2.task.name = "Kok";


    let shifts: Shift[] = [];
    shifts.push(testShift);
    shifts.push(testShift2);
    shifts.push(testShift2);
    shifts.push(testShift2);
    shifts.push(testShift2);
    shifts.push(testShift2);
    //end of placeholders

    /*  this.shiftService.getAllShifts(this.guid).then(res => {
      shifts = res;
    })*/


    this.dataSource = new MatTableDataSource<Shift>(shifts)

    this.dataSource.filterPredicate = (data, filter) => {
      return data.task.name.toLocaleLowerCase().includes(filter) ||
        data.date.toLocaleDateString().toLocaleLowerCase().includes(filter) ||
        data.startTime.toLocaleLowerCase().includes(filter) ||
        data.endTime.toLocaleLowerCase().includes(filter) ||
        data.participantsRequired.toLocaleString().includes(filter);
    }

    // @ts-ignore
    this.dataSource.sortingDataAccessor = (item, property) => {
      switch (property) {
        case 'Taak':
          return item.task.name;
        case 'Datum':
          return item.date;
        case 'Vanaf':
          return item.startTime;
        case 'Tot':
          return item.endTime;
        case '#Benodigde vrijwilligers':
          return item.participantsRequired;
        default:
          return item[property];
      }
    };
  }

  setDataSourceAttributes() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;

  }

  applyFilter(event: Event) {
    console.log(event)
    console.log(this.dataSource)
    console.log(this.sort)
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();


    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }


  details(id: string) {
    this.router.navigate(['/shift', id]);
  }

  onMatSortChange() {

  }
}
