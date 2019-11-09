import { Component, OnInit } from '@angular/core';
import { Project } from 'src/app/models/project';
import { ProjectService } from 'src/app/core/project/project.service';

@Component({
  selector: 'app-project',
  templateUrl: './project.component.html',
  styleUrls: ['./project.component.less']
})
export class ProjectComponent implements OnInit {
  projects: any;

  constructor(private projectService: ProjectService) { }

  ngOnInit() {
    this.projectService.getAllProjects().subscribe((response) => {
      console.log(response);
      if (response) {
        this.projects = response;
        console.log(this.projects);
      }
    });
  }

}
