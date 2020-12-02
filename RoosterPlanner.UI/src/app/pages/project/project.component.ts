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
  projectTasks: Task[];
  taskCardStyle = 'card';
  itemsPerCard = 5;
  reasonableMaxInteger = 10000;
  tasksElementHeight: number;


  constructor(private userService: UserService,
              private projectService: ProjectService,
              private participationService: ParticipationService,
              private route: ActivatedRoute,
              private toastr: ToastrService,
              public dialog: MatDialog,
              public taskService: TaskService) {
  }

  async ngOnInit(): Promise<void> {
    this.isAdmin = this.userService.userIsAdminFrontEnd();
    this.route.paramMap.subscribe((params: ParamMap) => {
      this.guid = params.get('id');
    });
    await this.getParticipation().then(() => {
        if (this.project != null) {
          this.project.pictureUri = "../assets/Logo.png"
        }
        if (this.participation != null) {
          this.participation.project.pictureUri = "../assets/Logo.png";
        }
      }
    );

    await this.getProjectTasks().then();

  }

  async getProject() {
    await this.projectService.getProject(this.guid).then(project => {
      if (project != null) {
        this.project = project;
        this.displayProject(project);
      }
    })
  }

  async getProjectTasks() {
    this.taskService.getAllProjectTasks(this.guid).then(tasks => {
      this.projectTasks = tasks.filter(t => t != null);
      this.projectTasks = this.projectTasks.slice(0, this.itemsPerCard);
      this.projectTasks.sort((a, b) => a.name > b.name ? 1 : -1);

    })
  }

  async getParticipation() {
    await this.participationService.getParticipation(this.userService.getCurrentUserId(), this.guid).then(async par => {
      if (par != null) {
        this.participation = par;
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
    this.loaded = false;
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
      this.loaded = false;
      if (dialogResult != null && dialogResult !== this.participation.maxWorkingHoursPerWeek) {
        this.participation.maxWorkingHoursPerWeek = dialogResult;
        this.participationService.updateParticipation(this.participation).then(response => {
          this.displayProject(response.body.project)
        })
      }
    });
  }

  expandTaskCard() {
    if (this.taskCardStyle == 'expanded-card') {
      document.getElementById("left").hidden = false;
      document.getElementById("pictureFrame").hidden = false;
      this.taskCardStyle = 'card';
      this.itemsPerCard = 5;
      this.projectTasks = this.projectTasks.slice(0, this.itemsPerCard);
    } else {
      document.getElementById("left").hidden = true;
      document.getElementById("pictureFrame").hidden = true;
      this.taskCardStyle = 'expanded-card';
      this.itemsPerCard = this.reasonableMaxInteger;
      this.getProjectTasks().then(() => {
        this.tasksElementHeight = (this.projectTasks.length * 48);
      })
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
      this.getProjectTasks();
    })
  }

  modManager() {
    const dialogRef = this.dialog.open(AddManagerComponent, {
      width: '750px',
      data: {
        projectId: this.project.id
      }
    });
    dialogRef.disableClose = true;
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.toastr.success("De rol van "+ result+" is succesvol gewijzigd.")
      }
    });
  }
}
