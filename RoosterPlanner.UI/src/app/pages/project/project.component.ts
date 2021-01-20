import {Component, OnInit} from '@angular/core';
import {ProjectService} from "../../services/project.service";
import {ActivatedRoute, ParamMap} from "@angular/router";
import {Project} from "../../models/project";
import {DateConverter} from "../../helpers/date-converter";
import {CreateProjectComponent} from "../../components/create-project/create-project.component";
import {ToastrService} from "ngx-toastr";
import {MatDialog} from "@angular/material/dialog";
import {UserService} from "../../services/user.service";
import {Participation} from "../../models/participation";
import {ParticipationService} from "../../services/participation.service";
import {ConfirmDialogComponent, ConfirmDialogModel} from "../../components/confirm-dialog/confirm-dialog.component";
import {TaskService} from "../../services/task.service";
import {AddProjectTaskComponent} from "../../components/add-project-task/add-project-task.component";
import {Task} from 'src/app/models/task';
import {AddManagerComponent} from "../../components/add-manager/add-manager.component";
import {BreadcrumbService} from "../../services/breadcrumb.service";
import {CsvService} from "../../services/csv.service";
import {ShiftService} from "../../services/shift.service";
import {AgePipe} from "../../helpers/filter.pipe";
import {TextInjectorService} from "../../services/text-injector.service";
import {User} from "../../models/user";
import {
  faClipboardList,
  faHistory,
  faInfoCircle,
  faPlusCircle,
  faTrashAlt,
  faUserFriends
} from '@fortawesome/free-solid-svg-icons';


@Component({
  selector: 'app-project',
  templateUrl: './project.component.html',
  styleUrls: ['./project.component.scss']
})
export class ProjectComponent implements OnInit {
  historyIcon = faHistory;
  friendsIcon = faUserFriends;
  infoIcon = faInfoCircle;
  clipboard = faClipboardList;
  circle = faPlusCircle
  faTrash = faTrashAlt

  guid: string;
  project: Project;
  viewProject: Project;
  loaded: boolean = false;
  title: string;
  closeButtonText: string;
  isAdmin: boolean = false;
  participation: Participation;
  projectTasks: Task[];
  taskCardStyle = 'card';
  itemsPerCard = 5;
  reasonableMaxInteger = 10000;
  projectTasksExpandbtnDisabled: boolean = true;
  isManager: boolean = false;


  constructor(private userService: UserService,
              private projectService: ProjectService,
              private participationService: ParticipationService,
              private route: ActivatedRoute,
              private toastr: ToastrService,
              public dialog: MatDialog,
              private taskService: TaskService,
              private breadcrumbService: BreadcrumbService,
              private csvService: CsvService,
              private shiftService: ShiftService) {
    this.breadcrumbService.backcrumb();

  }

  async ngOnInit(): Promise<void> {
    this.isAdmin = this.userService.userIsAdminFrontEnd();
    this.isManager = this.userService.userIsProjectAdminFrontEnd();
    this.route.paramMap.subscribe((params: ParamMap) => {
      this.guid = params.get('id');
    });
    await this.getParticipation().then();
    await this.getProjectTasks().then();
  }

  async getProject() {
    await this.projectService.getProject(this.guid).then(res => {
      if (res) {
        this.project = res;
        this.displayProject(res);
      }
    })
  }

  async getProjectTasks() {
    this.taskService.getAllProjectTasks(this.guid).then(res => {
      this.projectTasks = res.filter(t => t != null);
      this.projectTasks = this.projectTasks.slice(0, this.itemsPerCard);
      if (this.projectTasks.length >= 5) {
        this.projectTasksExpandbtnDisabled = false;
      }
      this.projectTasks.sort((a, b) => a.name > b.name ? 1 : -1);

    })
  }

  async getParticipation() {
    await this.participationService.getParticipation(this.userService.getCurrentUserId(), this.guid).then(async res => {
      if (res) {
        this.participation = res;
        this.displayProject(this.participation.project)
      } else {
        await this.getProject().then();
      }
    })
  }

  displayProject(project: Project) {
    this.project = project
    this.viewProject = DateConverter.formatProjectDateReadable(this.project)
    this.title = this.viewProject.name;
    this.viewProject.closed ? this.closeButtonText = "Project openen" : this.closeButtonText = "Project afsluiten";
    if (this.viewProject.closed) {
      this.title += " DIT PROJECT IS GESLOTEN"
    }
    this.loaded = true;
  }


  editProject() {
    const dialogRef = this.dialog.open(CreateProjectComponent, {
      width: '500px',
      data: {
        createProject: false,
        project: this.project,
        title: "Project wijzigen",
      }
    });
    dialogRef.disableClose = true;
    dialogRef.afterClosed().subscribe(result => {
      if (result && result !== 'false') {
        setTimeout(() => {
          this.toastr.success(result.name + " is gewijzigd")

        }, 500);
      }
      this.getProject();
    });
  }

  async closeProject() {
    let messageVariable: string;
    this.project.closed ? messageVariable = "openen" : messageVariable = "sluiten";
    const message = "Weet je zeker dat je dit project wilt " + messageVariable + " ?"
    const dialogData = new ConfirmDialogModel("Bevestig wijziging", message, "ConfirmationInput", null);
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: "400px",
      data: dialogData
    });

    dialogRef.afterClosed().subscribe(async dialogResult => {
      if (dialogResult === true) {
        this.project.closed = !this.project.closed;
        this.loaded = false;
        await this.projectService.updateProject(this.project).then(response => {
          this.displayProject(response)
          if (this.project.closed) {
            this.toastr.success("Het project is gesloten");
          } else {
            this.toastr.success("Het project is geopend");
          }
        }, () => this.toastr.error("Fout tijdens het sluiten van het project"));
      }
    });


  }

  editWorkingHours() {
    const message = "Hoeveel uur per week wil je maximaal meewerken aan dit project?"
    const dialogData = new ConfirmDialogModel("Maximale inzet ", message, "NumberInput", this.participation.maxWorkingHoursPerWeek);
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: "400px",
      data: dialogData
    });

    dialogRef.afterClosed().subscribe(async dialogResult => {
      this.loaded = false;
      if (dialogResult != null &&
        dialogResult !== this.participation.maxWorkingHoursPerWeek &&
        dialogResult > 0 &&
        dialogResult <= 40) {
        this.participation.maxWorkingHoursPerWeek = dialogResult;
        await this.participationService.updateParticipation(this.participation).then(async response => {
          if (response) {
            this.participation = response
            this.displayProject(response.project);
          } else
            window.location.reload()
        });
      } else {
        this.loaded = true;
      }
    });
  }

  expandTaskCard() {
    let element = document.getElementById("icon")
    if (element) {
      if (this.taskCardStyle === 'expanded-card')
        element.innerText = "zoom_out_map"
      else
        element.innerText = "fullscreen_exit"
    }

    let pictureElement = document.getElementById("pictureFrame")
    let leftElement = document.getElementById("left")
    if (this.taskCardStyle === 'expanded-card') {
      if (leftElement)
        leftElement.hidden = false;

      if (pictureElement)
        pictureElement.hidden = false;
      this.taskCardStyle = 'card';
      this.itemsPerCard = 5;
      this.projectTasks = this.projectTasks.slice(0, this.itemsPerCard);
    } else if (this.taskCardStyle === 'card') {
      if (leftElement)
        leftElement.hidden = true;

      if (pictureElement)
        pictureElement.hidden = true;

      this.taskCardStyle = 'expanded-card';
      this.itemsPerCard = this.reasonableMaxInteger;
      this.getProjectTasks()
    }
  }

  modTask(modifier: string) {
    const dialogRef = this.dialog.open(AddProjectTaskComponent, {
      width: '500px',
      data: {
        modifier: modifier,
        project: this.project,
        currentProjectTasks: this.projectTasks
      }
    });
    dialogRef.disableClose = true;
    dialogRef.afterClosed().subscribe(() => {
      setTimeout(() => {
        this.getProjectTasks();
      }, 500);
    })
  }

  modManager() {
    const dialogRef = this.dialog.open(AddManagerComponent, {
      width: '500px',
      data: {
        projectId: this.project.id
      }
    });
    dialogRef.disableClose = true;
  }

  collaborate(participation: Participation) {
    const message = "Met wie wil je samenwerken?"
    const dialogData = new ConfirmDialogModel("Samenwerking", message, "TextInput", participation.remark);
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: "400px",
      data: dialogData
    });

    dialogRef.afterClosed().subscribe(async dialogResult => {
      this.loaded = false;
      if (dialogResult) {
        participation.remark = dialogResult.toString()
        await this.participationService.updateParticipation(participation).then(async response => {
            if (response) {
              this.toastr.success("Samenwerkingsvoorkeur is gewijzigd")
              this.participation = response;
              this.displayProject(response.project);
            } else
              window.location.reload()
          }
        );
      }
      this.loaded = true;
    });
  }

  async exportShifts() {
    let pipe: AgePipe = new AgePipe();
    let headers = TextInjectorService.shiftExportHeaders;

    let guid: string;
    if (this.participation)
      guid = this.participation.project.id
    else
      guid = this.project.id
    this.loaded = false;
    await this.shiftService.GetExportableData(guid).then(res => {
      let statistics: any[] = []
      res.forEach(shift => {
        shift.availabilities.forEach(avail => {
          let statistic = {
            Taaknaam: shift.task ? shift.task.name.replace(",", ".") : "Onbekend",
            Taakcategorie: shift.task && shift.task.category ? shift.task.category.name.replace(",", ".") : "Onbekend",
            Datum: DateConverter.toReadableStringFromDate(shift.date).replace(",", "."),
            Begintijd: shift.startTime.replace(",", "."),
            Endtijd: shift.endTime.replace(",", "."),
            NaamMedewerker: avail.participation.person.firstName.replace(",", ".") + " " + avail.participation.person.lastName.replace(",", "."),
            Leeftijd: pipe.transform(avail.participation.person.dateOfBirth),
            Woonplaats: avail.participation.person.city.replace(",", "."),
            Nationaliteit: avail.participation.person.nationality.replace(",", "."),
            Moedertaal: avail.participation.person.nativeLanguage.replace(",", "."),
            NLtaalniveau: avail.participation.person.dutchProficiency.replace(",", ".")
          }

          statistics.push(statistic);
        })
      })
      this.csvService.downloadFile(statistics, headers, "Shift export - " + this.project.name)
    })
    this.loaded = true;
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
