import {Component, OnInit, ViewChild} from '@angular/core';
import {MatTableDataSource} from "@angular/material/table";
import {MatPaginator} from "@angular/material/paginator";
import {MatSort} from "@angular/material/sort";
import {User} from "../../models/user";
import {ActivatedRoute, ParamMap, Router} from "@angular/router";
import {ParticipationService} from "../../services/participation.service";
import {UserService} from "../../services/user.service";
import {TextInjectorService} from "../../services/text-injector.service";
import {Breadcrumb} from "../../models/breadcrumb";
import {BreadcrumbService} from "../../services/breadcrumb.service";
import {DateConverter} from "../../helpers/date-converter";
import {AgePipe} from "../../helpers/filter.pipe";
import {CsvService} from "../../services/csv.service";

@Component({
  selector: 'app-employee',
  templateUrl: './employee.component.html',
  styleUrls: ['./employee.component.scss']
})
export class EmployeeComponent implements OnInit {
  guid: string;
  loaded: boolean = false;
  title: string = "Medewerker overzicht";

  displayedColumns: string[] = [];
  dataSource: MatTableDataSource<User> = new MatTableDataSource<User>();
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
              private router: Router,
              private participationService: ParticipationService,
              private userService: UserService,
              private breadcrumbService: BreadcrumbService,
              private csvService: CsvService
  ) {
    this.breadcrumbService.backcrumb()
    this.displayedColumns = TextInjectorService.employeeTableColumnNames;
  }


  async ngOnInit(): Promise<void> {
    this.route.paramMap.subscribe((params: ParamMap) => {
      this.guid = params.get('id');
    });

    if (this.guid)
      await this.userService.getAllParticipants(this.guid).then(res => {
        if (res) {
          this.dataSource = new MatTableDataSource<User>(res)
          this.loaded = true;
        }
      })
    else
      await this.userService.getAllUsers().then(res => {
        if (res)
          this.dataSource = new MatTableDataSource<User>(res)
        this.loaded = true;
      })

    this.dataSource.filterPredicate = (data, filter) => {
      return (data != null && (data.firstName + " " + data.lastName).toLocaleLowerCase().includes(filter) ||
        (!data.nationality && 'Onbekend'.toLocaleLowerCase().includes(filter)) ||
        (data.nationality && data.nationality.toLocaleLowerCase().includes(filter)) ||
        DateConverter.calculateAge(data.dateOfBirth).toLocaleLowerCase().includes(filter)
      )
    }

    this.dataSource.sortingDataAccessor = (item, property) => {
      switch (property) {
        case 'Naam':
          return item != null ? (item.firstName + " " + item.lastName) : null;
        case 'Leeftijd':
          return DateConverter.calculateAge(item.dateOfBirth);
        case 'Nationaliteit':
          return item != null ? item.nationality : null;
        default:
          return item[property];
      }
    };

  }

  setDataSourceAttributes() {

    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  applyFilter($event: KeyboardEvent) {
    const filterValue = ($event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  details(id: string) {

    this.guid ? this.router.navigate(['manage/profile', id]).then() : this.router.navigate(['admin/profile', id]).then()
  }

  async exportUsers() {
    let pipe: AgePipe = new AgePipe();
    let headers = TextInjectorService.employeeExportHeaders;
    let users: User[] = []
    this.loaded = false;
    if (this.guid) {
      await this.userService.getAllParticipants(this.guid).then(res => {
        users = res;
      })
    } else {
      await this.userService.getAllUsers().then(res => {
        if (res)
          users = res;
      })
    }

    let statistics: any[] = []
    users.forEach(u => {
      let statistic = {
        NaamMedewerker: u.firstName && u.lastName ? u.firstName?.replace(",", ".") + " " + u.lastName?.replace(",", ".") : "Onbekend",
        Leeftijd: u.dateOfBirth ? pipe.transform(u.dateOfBirth) : "Onbekend",
        Email: u.email ? u.email.replace(",", ".") : "Onbekend",
        Telefoonnummer: u.phoneNumber ? u.phoneNumber.replace(",", ".") : "Onbekend",

        Adres: u.streetAddress ? u.streetAddress.replace(",", ".") : "Onbekend",
        Postcode: u.postalCode ? u.postalCode.replace(",", ".") : "Onbekend",
        Woonplaats: u.city ? u.city.replace(",", ".") : "Onbekend",
        Nationaliteit: u.nationality ? u.nationality.replace(",", ".") : "Onbekend",
        Moedertaal: u.nativeLanguage ? u.nativeLanguage.replace(",", ".") : "Onbekend",
        NLtaalniveau: u.dutchProficiency ? u.dutchProficiency.replace(",", ".") : "Onbekend",

        PushBerichten: u.pushDisabled ? "Uitgeschakeld" : "Ingeschakeld",
        DatumAkkoordPrivacyPolicy: u.termsOfUseConsented ? DateConverter.toReadableStringFromString(u.termsOfUseConsented) : "Onbekend",
      }
      statistics.push(statistic)
    })

    this.csvService.downloadFile(statistics, headers, "Employee Export")
    this.loaded = true;
  }
}

