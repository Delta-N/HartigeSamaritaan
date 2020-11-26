import {Component, OnInit} from '@angular/core';
import {MatDialog} from "@angular/material/dialog";
import {AddProjectComponent} from "../../components/add-project/add-project.component";
import {Project} from "../../models/project";
import {ProjectService} from "../../services/project.service";
import {Participation} from "../../models/participation";
import {UserService} from "../../services/user.service";
import {User} from "../../models/user";
import {ParticipationService} from "../../services/participation.service";
import {ToastrService} from "ngx-toastr";
import {EntityHelper} from "../../helpers/entity-helper";
import {ChangeProfileComponent} from "../../components/change-profile/change-profile.component";

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
              private participationService: ParticipationService,
              private toastr: ToastrService,
              ) {
  }

  async ngOnInit(): Promise<void> {
    await this.userService.getCurrentUser().then(async user => {
      if (user !== false) {
        this.currentUser = user
        this.getParticipations().then(() => this.loaded = true)
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
    let projects: Project[] = [];
    let userCheckedProfile: boolean = false;

    const dialogRef = this.dialog.open(ChangeProfileComponent, {
      width: '500px',
      data: this.currentUser
    });
    dialogRef.disableClose = true;
    dialogRef.afterClosed().subscribe(result => {
      if (result != null) {
        userCheckedProfile = true;
      }

      if (userCheckedProfile) {
        this.projectService.getActiveProjects().then(response => {
            projects = response;

            projects.forEach(pro => {
              this.participations.forEach(par => {
                if (pro.id == par.project.id) {
                  projects = projects.filter(obj => obj !== pro);
                }
              })
            })

            let dialogRef = this.dialog.open(AddProjectComponent, {
              data: projects,
              width: '350px',
            });
            dialogRef.disableClose = true;
            dialogRef.afterClosed().subscribe(result => {
              if (result !== 'false') {
                this.selectedProjects = result;
                this.selectedProjects.forEach(project => {
                  let participation: Participation = new Participation();
                  participation.id = EntityHelper.returnEmptyGuid();
                  participation.person = this.currentUser;
                  participation.project = project
                  this.participationService.postParticipation(participation);
                })
                setTimeout(() => this.getParticipations(), 1000)
              }
            })
          }
        )
      } else {
        this.toastr.warning("Controleer eerst je profiel")
      }
    })
  }
}
