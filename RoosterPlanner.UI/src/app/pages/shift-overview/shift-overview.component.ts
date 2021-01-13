import {Component, OnInit, ViewChild} from '@angular/core';
import {Project} from "../../models/project";
import {ActivatedRoute, ParamMap, Router} from "@angular/router";
import {ProjectService} from "../../services/project.service";
import {Shift} from "../../models/shift";
import {MatPaginator} from "@angular/material/paginator";
import {MatSort} from '@angular/material/sort';
import {MatTableDataSource} from "@angular/material/table";
import {ShiftService} from "../../services/shift.service";
import {TextInjectorService} from "../../services/text-injector.service";
import {BreadcrumbService} from "../../services/breadcrumb.service";
import {Breadcrumb} from "../../models/breadcrumb";

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

  displayedColumns: string[] = [];
  dataSource: MatTableDataSource<Shift> = new MatTableDataSource<Shift>();
  paginator: MatPaginator;
  sort: MatSort;

  @ViewChild(MatSort) set matSort(ms: MatSort) {
    this.sort = ms;
    this.setDataSourceAttributes()
  }

  @ViewChild(MatPaginator) set matPaginator(mp: MatPaginator) {
    this.paginator = mp;
    this.setDataSourceAttributes()
  }

  constructor(private route: ActivatedRoute,
              private projectService: ProjectService,
              private router: Router,
              private shiftService: ShiftService,
              private breadcrumbService: BreadcrumbService) {
    let current: Breadcrumb = new Breadcrumb("Shift overzicht",null);

    let breadcrumbs: Breadcrumb[] = [this.breadcrumbService.dashboardcrumb, this.breadcrumbService.managecrumb, current]
    this.breadcrumbService.replace(breadcrumbs);
    this.displayedColumns = TextInjectorService.shiftTableColumnNames;
  }

  async ngOnInit(): Promise<void> {
    this.route.paramMap.subscribe((params: ParamMap) => {
      this.guid = params.get('id');
    });
    await this.projectService.getProject(this.guid).then(async project => {
      this.project = project;
      this.title += ": " + this.project.name
    })
    let shifts: Shift[] = [];
    await this.shiftService.getAllShifts(this.guid).then(res => {
      if (res != null) {
        shifts = res;
        this.dataSource = new MatTableDataSource<Shift>(shifts)
      }
      this.loaded = true;
    })

    this.dataSource.filterPredicate = (data, filter) => {
      return (data.task != null && data.task.name.toLocaleLowerCase().includes(filter)) ||
        (data.task == null && "n.n.t.b.".toLocaleLowerCase().includes(filter)) ||
        data.date.toLocaleDateString().toLocaleLowerCase().includes(filter) ||
        data.startTime.toLocaleLowerCase().includes(filter) ||
        data.endTime.toLocaleLowerCase().includes(filter) ||
        data.participantsRequired.toLocaleString().includes(filter);
    }

    // @ts-ignore
    this.dataSource.sortingDataAccessor = (item, property) => {
      switch (property) {
        case 'Taak':
          return item.task != null ? item.task.name : null;
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
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  details(id: string) {
    this.router.navigate(['manage/shift', id]).then();
  }
}
