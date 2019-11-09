import { Component, OnInit } from '@angular/core';
import { ProjectService } from '../../core/project/project.service';
import { Project } from '../../models/project.model';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.less']
})
export class AdminComponent implements OnInit {
  constructor(private projectService: ProjectService) {}

  ngOnInit() {
    const project = new Project();
    project.id = '00000000-0000-0000-0000-000000000000';
    project.name = 'John Test Project';
    project.address = 'Laan van Waalhaven 450';
    project.city = 'Den Haag';
    project.closed = false;
    project.description = 'This is a brief description';
    project.startDate = new Date();
    project.endDate = new Date();
    project.pictureUri = 'www.delta-n.nl';
    project.websiteUrl = 'www.delta-n.nl';
    this.projectService.createOrUpdateProject(project).subscribe(result => {
      if (result && result.status === 200) {
        console.log(result.body);
      }
    });

    this.projectService.getAllProjects().subscribe(result => {
      console.log(result);
    });
  }
}
