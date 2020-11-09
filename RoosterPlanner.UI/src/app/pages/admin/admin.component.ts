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
  listOfProjects: any = []
  listOfAdmins: any = []
  tempListProjects: Project[] = [];
  tempListAdmins: User[] = [];
  loaded: boolean = false;
  administrators: User[] = []
  itemsPerCard: number = 5;

  constructor(public dialog: MatDialog,
              private projectService: ProjectService,
              private router: Router,
              private userService: UserService,
              private toastr: ToastrService) {
  }

  ngOnInit(): void {
    this.getProjects().then()
    this.getAdministrators().then(() => this.loaded = true)
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
    });
  }

  spitAdministrators() {
    this.administrators.sort((a, b) => a.firstName > b.firstName ? 1 : -1);
    for (let i = 0; i < this.administrators.length; i++) {
      this.tempListAdmins.push(this.administrators[i])
      if (this.tempListAdmins.length === this.itemsPerCard || i === this.administrators.length - 1) {
        this.listOfAdmins.push(this.tempListAdmins);
        this.tempListAdmins = [];
      }
    }
  }

  splitProjects() {
    this.projects.sort((a, b) => a.startDate < b.startDate ? 1 : -1);
    for (let i = 0; i < this.projects.length; i++) {
      this.tempListProjects.push(this.projects[i])
      if (this.tempListProjects.length === this.itemsPerCard || i === this.projects.length - 1) {
        this.listOfProjects.push(this.tempListProjects);
        this.tempListProjects = [];
      }
    }
  }

  addProject() {
    const dialogRef = this.dialog.open(CreateProjectComponent, {
      width: '500px',
      data: {
        createProject: true,
      }
    });
    dialogRef.disableClose = true;
    dialogRef.afterClosed().subscribe(result => {
      if (result !== 'false') {
        setTimeout(() => {
          this.listOfProjects = []
          this.getProjects().then(() => {
            if (result != null) {
              this.toastr.success(result.name + " is toegevoegd als nieuw project")
            }
          })
        }, 1000);
      }
    });
  }
  modAdmin(modifier: string) {
    let toastrString: string;
    let dataModifier: boolean;
    if (modifier === 'add') {
      toastrString = 'toegevoegd';
      dataModifier = true;
    } else if (modifier === 'remove') {
      toastrString = 'verwijderd';
      dataModifier = false;
    }

    const dialogRef = this.dialog.open(AddAdminComponent, {
      width: '500px',
      data: {
        addAdminType: dataModifier,
        administrators: this.administrators
      }
    });
    dialogRef.disableClose = true;
    dialogRef.afterClosed().subscribe(async result => {
      if (result != null) {
        this.listOfAdmins = [];
        setTimeout(() => {
          this.getAdministrators().then()
          this.toastr.success(result + " is " + toastrString + " als administrator")
        }, 500);
      }
    });
  }

  todo() {
    this.toastr.warning("Deze functie moet nog geschreven worden")
  }
}
