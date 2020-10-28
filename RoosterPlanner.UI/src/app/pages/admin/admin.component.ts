import {Component, OnInit} from '@angular/core';
import {ProjectService} from "../../services/project.service";
import {Project} from "../../models/project";
import {Router} from "@angular/router"
import {MatDialog} from '@angular/material/dialog';
import {CreateProjectComponent} from "../../components/create-project/create-project.component";
import {User} from "../../models/user";
import {UserService} from "../../services/user.service";
import {AddAdminComponent} from "../../components/add-admin/add-admin.component";
import {ToastrService} from "ngx-toastr";


@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.scss']
})
export class AdminComponent implements OnInit {
  projects: Project[] = [];
  listOfProjects: Project[][] = []
  listOfAdmins: User[][] = []
  tempListProjects: Project[] = [];
  tempListAdmins: User[] = [];
  loaded: boolean = false;

  administrators: User[] = []

  constructor(public dialog: MatDialog, private projectService: ProjectService, private router: Router, private userService: UserService, private toastr: ToastrService) {
  }

  ngOnInit(): void {
    this.getProjects().then()
    this.getAdministrators().then()

  }

  async getProjects() {
    await this.projectService.getProject().then(x => {
      this.projects = x;
      this.splitProjects();
    });
  }

  async getAdministrators() {
    await this.userService.getAdministrators().then(x => {
      this.administrators = x;
      this.spitAdministrators();
      this.loaded = true;
    });
  }

  spitAdministrators() {
    for (let i = 0; i < this.administrators.length; i++) {
      this.tempListAdmins.push(this.administrators[i])
      if (this.tempListAdmins.length === 5 || i === this.administrators.length - 1) {
        this.listOfAdmins.push(this.tempListAdmins);
        this.tempListAdmins = [];
      }
    }
  }

  splitProjects() {
    for (let i = 0; i < this.projects.length; i++) {
      this.tempListProjects.push(this.projects[i])
      if (this.tempListProjects.length === 5 || i === this.projects.length - 1) {
        this.listOfProjects.push(this.tempListProjects);
        this.tempListProjects = [];
      }
    }
  }

  addProject() {
    const dialogRef = this.dialog.open(CreateProjectComponent, {
      width: '500px',
      data: {createProject: true}
    });
    dialogRef.disableClose = true;
    dialogRef.afterClosed().subscribe(result => {
      if (result !== 'false') {
        setTimeout(x => {
          this.listOfProjects = []
          this.getProjects().then()
          if (result != null) {
            this.toastr.success(result + " is toegevoegd als nieuw project")
          }
        }, 500);
      }
    });
  }

  async addAdministrator() {
    const dialogRef = this.dialog.open(AddAdminComponent, {
      width: '500px',
      data: {addAdminType: true}
    });
    dialogRef.afterClosed().subscribe(async result => {
      this.listOfAdmins = [];
      setTimeout(x => {
        this.getAdministrators().then()
        this.toastr.success(result + " is toegevoegd als administrator")
      }, 500);
    })
  }

  removeAdministrator() {
    const dialogRef = this.dialog.open(AddAdminComponent, {
      width: '500px',
      data: {addAdminType: false}
    });
    dialogRef.afterClosed().subscribe(async result => {
      this.listOfAdmins = [];
      setTimeout(x => {
        this.getAdministrators().then()
        this.toastr.success(result + " is verwijderd als administrator")
      }, 500);
    });
  }
}
