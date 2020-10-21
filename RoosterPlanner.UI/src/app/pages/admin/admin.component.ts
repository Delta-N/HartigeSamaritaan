import {Component, OnInit} from '@angular/core';
import {ProjectService} from "../../services/project.service";
import {Project} from "../../models/project";
import {Router} from "@angular/router";

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

  constructor(private projectService: ProjectService, private router: Router) {
  }

  async ngOnInit(): Promise<void> {
    await this.projectService.getProject().then(x => {
      this.projects = x;
      this.loaded = true;
    });
  }

  addProject() {
    this.router.navigateByUrl('create-project').then();
  }


  addAdministrator() {
    window.alert("Deze functie moet nog geschreven worden...")
  }
}
