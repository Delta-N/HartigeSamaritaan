import {Component, OnInit} from '@angular/core';
import {MatDialog} from "@angular/material/dialog";
import {AddProjectComponent} from "../../components/add-project/add-project.component";
import {Project} from "../../models/project";
import {ProjectService} from "../../services/project.service";
import {Participation} from "../../models/participation";
import {UserService} from "../../services/user.service";
import {User} from "../../models/user";
import {ParticipationService} from "../../services/participation.service";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  loaded: boolean = false;
  participations: Participation[] = [];
  currentUser: User;
  selectedProjects: Project[];


  constructor(public dialog: MatDialog,
              private projectService: ProjectService,
              private userService: UserService,
              private participationService: ParticipationService) {
  }

  async ngOnInit(): Promise<void> {
    await this.userService.getCurrentUser().then(async user => {
      if (user !== false) {
        this.currentUser = user
        this.getParticipations().then(x => this.loaded = true)
      }
    });
  }


  async getParticipations() {
    await this.participationService.getParticipations(this.currentUser.id).then(response => {
      this.participations = response;
      this.participations.sort((a, b) => a.project.name.toLowerCase() > b.project.name.toLowerCase() ? 1 : -1);
    })
  }

  async addParticipation() {
    await this.projectService.getActiveProjects().then(response => {
      let projects: Project[] = response;
      projects.forEach(pro => {
        this.participations.forEach(par => {
          if (pro.id == par.project.id) {
            projects = projects.filter(obj => obj !== pro);
          }
        })
      })
      let dialogRef = this.dialog.open(AddProjectComponent, {
        data: projects,
        panelClass: 'custom-dialog-container'
      });
      dialogRef.disableClose = true;
      dialogRef.afterClosed().subscribe(result => {
        if (result !== 'false') {
          this.selectedProjects = result;
          this.selectedProjects.forEach(project => {
            let participation: Participation = new Participation("00000000-0000-0000-0000-000000000000", this.currentUser, project)
            this.participationService.postParticipation(participation);
          })
          setTimeout(x => this.getParticipations(), 1000)
        }
      })
    })
  }
}
