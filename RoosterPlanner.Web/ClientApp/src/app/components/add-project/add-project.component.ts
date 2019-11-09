import { Component, OnInit } from '@angular/core';
import { ProjectService } from '../../core/project/project.service';
import { Router } from '@angular/router';
import { Project } from '../../models/project';

@Component({
  selector: 'app-add-project',
  templateUrl: './add-project.component.html',
  styleUrls: ['./add-project.component.less']
})
export class AddProjectComponent implements OnInit {
  projects = new Array<Project>();
  constructor(private projectService: ProjectService, private router: Router) {}

  ngOnInit() {
    this.getProjects();
  }

  addUserToProject() {

  }

  private getProjects() {
    this.projectService.getAllProjects().subscribe(response => {
      if (response) {
        this.projects = response;
      }
    });
  }
}
