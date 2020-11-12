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
  loaded: boolean = false;
  administrators: User[] = []
  itemsPerCard: number = 5;

  projectCardStyle = 'card';
  projectsElementHeight: number;
  adminCardStyle = 'card';
  adminElementHeight: number;

  reasonableMaxInteger: number = 10000; //aanpassen na 10k projecten/admins ;)


  constructor(public dialog: MatDialog,
              private projectService: ProjectService,
              private router: Router,
              private userService: UserService,
              private toastr: ToastrService,
  ) {
  }

  ngOnInit(): void {
    this.getProjects(0, this.itemsPerCard).then()
    this.getAdministrators(0, this.itemsPerCard).then(() => this.loaded = true)
  }

  async getProjects(offset: number, pageSize: number) {
    await this.projectService.getAllProjects(offset, pageSize).then(x => {
      this.projects = x;
      this.projects.sort((a, b) => a.startDate < b.startDate ? 1 : -1);
    });
  }

  async getAdministrators(offset: number, pageSize: number) {
    await this.userService.getAdministrators(offset, pageSize).then(x => {
      this.administrators = x;
      this.administrators.sort((a, b) => a.firstName > b.firstName ? 1 : -1);
    });
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
          this.getProjects(0, this.itemsPerCard).then(() => {
            if (result != null) {
              this.toastr.success(result.name + " is toegevoegd als nieuw project")
            }
          })
        }, 1000);
      }
    });
  }

  async modAdmin(modifier: string) {
    let toastrString: string;
    let dataModifier: boolean;
    if (modifier === 'add') {
      toastrString = 'toegevoegd';
      dataModifier = true;
    } else if (modifier === 'remove') {
      await this.getAdministrators(0, this.reasonableMaxInteger)
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
        setTimeout(() => {
          this.getAdministrators(0, this.itemsPerCard).then()
          this.toastr.success(result + " is " + toastrString + " als administrator")
        }, 500);
      }
    });
  }

  todo() {
    this.toastr.warning("Deze functie moet nog geschreven worden")
  }

  expandProjectCard() {
    if (this.projectCardStyle == 'expanded-card') {
      document.getElementById("adminCard").hidden = false;
      document.getElementById("dataCard").hidden = false;
      document.getElementById("taskCard").hidden = false;
      this.projectCardStyle = 'card';
      this.itemsPerCard = 5;
      this.projects = this.projects.slice(0, this.itemsPerCard);
    } else {
      document.getElementById("adminCard").hidden = true;
      document.getElementById("dataCard").hidden = true;
      document.getElementById("taskCard").hidden = true;
      this.projectCardStyle = 'expanded-card';
      this.itemsPerCard = this.reasonableMaxInteger
      this.getProjects(0, this.itemsPerCard).then(() => {
        this.projectsElementHeight = (this.projects.length * 48);

      })
    }
  }

  expandAdminCard() {
    if (this.adminCardStyle == 'expanded-card') {
      document.getElementById("projectCard").hidden = false;
      document.getElementById("dataCard").hidden = false;
      document.getElementById("taskCard").hidden = false;
      this.adminCardStyle = 'card';
      this.itemsPerCard = 5;
      this.administrators = this.administrators.slice(0, this.itemsPerCard);
    } else {
      document.getElementById("projectCard").hidden = true;
      document.getElementById("dataCard").hidden = true;
      document.getElementById("taskCard").hidden = true;
      this.adminCardStyle = 'expanded-card';
      this.itemsPerCard = this.reasonableMaxInteger;
      this.getAdministrators(0, this.itemsPerCard).then(() => {
        this.adminElementHeight = (this.administrators.length * 48);
      })

    }
  }
}
