import {Component, OnInit} from '@angular/core';
import {Project} from "../../models/project";
import {FormBuilder} from "@angular/forms";
import {Router} from "@angular/router";
import {ProjectService} from "../../services/project.service";
import {Validators} from '@angular/forms';
import {DateValidator} from "../../helpers/datevalidator"

@Component({
  selector: 'app-create-project',
  templateUrl: './create-project.component.html',
  styleUrls: ['./create-project.component.scss']
})
export class CreateProjectComponent implements OnInit {
  project: Project = new Project('');
  checkoutForm;

  constructor(private formBuilder: FormBuilder, private router: Router, private projectService: ProjectService) {
    this.checkoutForm = this.formBuilder.group({
      id: '',
      name: ['', Validators.required],
      address: ['', Validators.required],
      city: ['', Validators.required],
      description: ['', Validators.required],
      startDate: ['', Validators.compose([Validators.required, DateValidator.date])],
      endDate: ['', [DateValidator.dateOrNull]],
      pictureUri: '',
      websiteUrl: ''
    })
  }

  ngOnInit(): void {
  }


  saveProject(value: Project) {
    this.project = value
    if (this.checkoutForm.status == 'INVALID') {
      window.alert("Not all fields are correct");
    } else {
      this.projectService.postProject(this.project).then();
    }
  }
}
