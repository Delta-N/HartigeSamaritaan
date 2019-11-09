import { Component, OnInit } from '@angular/core';
import { Project } from '../../models/project';
import { ProjectService } from '../../core/project/project.service';

@Component({
  selector: 'app-project-overview',
  templateUrl: './project-overview.component.html',
  styleUrls: ['./project-overview.component.less']
})
export class ProjectOverviewComponent implements OnInit {
  projects = new Array<Project>();
  constructor(private projectService: ProjectService) {}

  ngOnInit() {
    this.getProjects();
  }

  private getProjects() {
    this.projectService.getAllProjects().subscribe(response => {
        if (response) {
          this.projects = response;
        }
    });
  }
}
