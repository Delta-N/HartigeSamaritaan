import { Component, OnInit } from '@angular/core';
import { ProjectService } from '../../core/project/project.service';
import { Router } from '@angular/router';
import { Project } from '../../models/project';
import { JwtHelper } from '../../utilities/jwt-helper';

@Component({
  selector: 'app-add-project',
  templateUrl: './add-project.component.html',
  styleUrls: ['./add-project.component.less']
})
export class AddProjectComponent implements OnInit {
  projects = new Array<Project>();
  selectedOptions = [];

  private personId: string;
  constructor(private projectService: ProjectService, private router: Router) {}

  ngOnInit() {
    this.getProjects();
    const idToken = JwtHelper.decodeToken(
      sessionStorage.getItem('msal.idtoken')
    );
    if (idToken && idToken.oid) {
      this.personId = idToken.oid;
    }
  }

  addUserToProject() {
    this.selectedOptions.forEach(element => {
      // this.projectService
      //   .addPersonToProject(element.id, this.personId)
      //   .subscribe(response => {});
    });
    this.router.navigateByUrl('/');
  }

  private getProjects() {
    this.projectService.getAllProjects().subscribe(response => {
      if (response) {
        this.projects = response;
      }
    });
  }
}
