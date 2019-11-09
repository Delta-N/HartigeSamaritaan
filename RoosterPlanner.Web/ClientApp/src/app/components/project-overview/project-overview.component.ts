import { Component, OnInit } from '@angular/core';
import { Project } from '../../models/project';
import { ProjectService } from '../../core/project/project.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-project-overview',
  templateUrl: './project-overview.component.html',
  styleUrls: ['./project-overview.component.less']
})
export class ProjectOverviewComponent implements OnInit {
  projects = new Array<Project>();
  constructor(private projectService: ProjectService, private router: Router) {}

  ngOnInit() {
    this.getProjects();
  }

  addProject() {
    this.router.navigateByUrl('project/add');
  }

  private getProjects() {
    this.projectService.getAllProjects().subscribe(response => {
      if (response) {
        this.projects = response;
      }
    });
  }
}
