import {Component, OnInit} from '@angular/core';
import {MatDialog} from "@angular/material/dialog";
import {AddProjectComponent} from "../../components/add-project/add-project.component";
import {Project} from "../../models/project";
import {ProjectService} from "../../services/project.service";
import {Participation} from "../../models/participation";
import {UserService} from "../../services/user.service";
import {JwtHelper} from "../../helpers/jwt-helper";
import {User} from "../../models/user";
import {ParticipationService} from "../../services/participation.service";
import {DateConverter} from "../../helpers/date-converter";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  projects: Project[] = [];
  loaded: boolean = false;
  participation:Participation;
  currentUser:User;
  selectedProject:Project;


  constructor(public dialog: MatDialog, private projectService: ProjectService, private userService:UserService, private participationService:ParticipationService) {
  }

  async ngOnInit(): Promise<void> {
    await this.projectService.getProject().then(response => {
      this.projects = response.filter(x=>!x.closed);
      this.loaded = true;
    });
  }

  async addProject() {
    await this.projectService.getProject("68a122f7-2467-4a11-a61f-4dcbbfd57f2b").then(response => {
      this.selectedProject = response[0];
      this.selectedProject.startDate=DateConverter.toDate(this.selectedProject.startDate);
      this.selectedProject.endDate=DateConverter.toDate(this.selectedProject.endDate);
      console.log(this.selectedProject);
    });
    await this.userService.getCurrentUser().then(user => {
      if (user !== false) {
        this.currentUser = user
        console.log(this.currentUser);
        this.participation = new Participation("00000000-0000-0000-0000-000000000000", this.currentUser, this.selectedProject, 12)
        console.log(JSON.stringify(this.participation));
      }
    })


    await this.participationService.postParticipation(this.participation).then(response => {
      console.log(response)
    }, reject => {
      console.log(reject)
    })


    /*let dialogRef = this.dialog.open(AddProjectComponent, {data: this.projects, panelClass: 'custom-dialog-container'});

    dialogRef.afterClosed().subscribe(result => {
    })*/
  }
}
