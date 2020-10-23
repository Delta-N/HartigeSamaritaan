import {Component, OnInit} from '@angular/core';
import {MatDialog} from "@angular/material/dialog";
import {AddProjectComponent} from "../../components/add-project/add-project.component";
import {Project} from "../../models/project";
import {ProjectService} from "../../services/project.service";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  projects: Project[] = [];
  loaded: boolean = false;


  constructor(public dialog: MatDialog, private projectService: ProjectService) {
  }

  async ngOnInit(): Promise<void> {
    await this.projectService.getProject().then(response => {
      this.projects = response;
      this.loaded = true;
    });
  }

  addProject() {
    let dialogRef = this.dialog.open(AddProjectComponent, {data: this.projects, panelClass: 'custom-dialog-container'});

    dialogRef.afterClosed().subscribe(result => {
    })
  }
}
