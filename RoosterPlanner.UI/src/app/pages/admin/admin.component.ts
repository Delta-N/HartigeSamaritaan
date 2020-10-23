import {Component, OnInit, Inject} from '@angular/core';
import {ProjectService} from "../../services/project.service";
import {Project} from "../../models/project";
import {Router} from "@angular/router"
import {MatDialog} from '@angular/material/dialog';
import{CreateProjectComponent} from "../../components/create-project/create-project.component";


@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.scss']
})
export class AdminComponent implements OnInit {
  projects: Project[] = [];
  loaded: boolean = false;

  administrators = [
    {name: "Corné"},
    {name: "JW"},
    {name: "Yannick"},
    {name: "Joanne"},
  ]

  constructor(public dialog:MatDialog,private projectService: ProjectService, private router: Router) {
  }

  async ngOnInit(): Promise<void> {
    await this.projectService.getProject().then(x => {
      this.projects = x;
      this.loaded = true;
    });
  }

  addProject() {
    const dialogRef = this.dialog.open(CreateProjectComponent, {
      width: '500px',
    });
    dialogRef.afterClosed().subscribe(result => {
      this.router.navigateByUrl('/admin').then();
    });

  }


  addAdministrator() {
    window.alert("Deze functie moet nog geschreven worden...")
  }
}
