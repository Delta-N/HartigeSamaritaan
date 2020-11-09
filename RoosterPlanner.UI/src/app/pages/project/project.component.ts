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

@Component({
  selector: 'app-project',
  templateUrl: './project.component.html',
  styleUrls: ['./project.component.scss']
})
export class ProjectComponent implements OnInit {
  guid: string;
  project: Project;
  viewProject: Project;
  loaded: boolean = false;
  title: string;
  closeButtonText: string;
  isAdmin: boolean = false;
  participation: Participation;


  constructor(private userService: UserService,
              private projectService: ProjectService,
              private participationService: ParticipationService,
              private route: ActivatedRoute,
              private toastr: ToastrService,
              public dialog: MatDialog) {
  }

  async ngOnInit(): Promise<void> {
    this.isAdmin = this.userService.userIsAdminFrontEnd();
    this.route.paramMap.subscribe((params: ParamMap) => {
      this.guid = params.get('id');
    });
      await this.getParticipation().then();
  }

  async getProject() {
    await this.projectService.getProject(this.guid).then(project => {
      if (project[0] != null) {
        this.displayProject(project[0]);
      }
    })
  }

  async getParticipation() {
    await this.participationService.getParticipation(this.userService.getCurrentUserId(), this.guid).then(par => {
      if (par != null) {
        this.participation = par;
        this.displayProject(this.participation.project)
      } else {
        this.getProject().then();
      }
    })
  }

  displayProject(project:Project){
    this.project = project
    this.viewProject = DateConverter.formatProjectDateReadable(this.project)
    this.title = this.viewProject.name;
    this.viewProject.closed ? this.closeButtonText = "Project openen" : this.closeButtonText = "Project sluiten";
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
        project: this.project
      }
    });
    dialogRef.disableClose = true;
    dialogRef.afterClosed().subscribe(result => {
      if (result !== 'false') {
          setTimeout(() => {
            this.toastr.success(result.name + " is gewijzigd")
            this.getProject();
          }, 500);
      }
    });
  }

  async closeProject() {
    this.project.closed = !this.project.closed;
    this.loaded=false;
    await this.projectService.updateProject(this.project).then(response => {
      this.displayProject(response.body)
      if (this.project.closed) {
        this.toastr.success("Het project is gesloten");
      } else {
        this.toastr.success("Het project is geopend");
      }
    }, () => this.toastr.error("Fout tijdens het sluiten van het project"));
  }

  editWorkingHours() {
    const message = "Hoeveel uur per week wil je maximaal meewerken aan dit project?"
    const dialogData = new ConfirmDialogModel("Maximale inzet ", message, "NumberInput");
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: "400px",
      data: dialogData
    });

    dialogRef.afterClosed().subscribe(dialogResult => {
      this.loaded=false;
      if (dialogResult != null && dialogResult !== this.participation.maxWorkingHoursPerWeek) {
        this.participation.maxWorkingHoursPerWeek = dialogResult;
        this.participationService.updateParticipation(this.participation).then(response => {
          this.displayProject(response.body.project)
        })
      }
    });
  }
}
