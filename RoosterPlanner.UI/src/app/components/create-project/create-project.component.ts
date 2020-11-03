import {Component, OnInit} from '@angular/core';
import {Project} from "../../models/project";
import {FormBuilder, Validators} from "@angular/forms";
import {ProjectService} from "../../services/project.service";
import {Validator} from "../../helpers/validators"

@Component({
  selector: 'app-create-project',
  templateUrl: './create-project.component.html',
  styleUrls: ['./create-project.component.scss']
})
export class CreateProjectComponent implements OnInit {
  project: Project = new Project('');
  checkoutForm;

  constructor(private formBuilder: FormBuilder, private projectService: ProjectService) {
    this.checkoutForm = this.formBuilder.group({
      id: '',
      name: ['', Validators.required],
      address: ['', Validators.required],
      city: ['', Validators.required],
      description: ['', Validators.required],
      startDate: ['', Validators.compose([Validators.required, Validator.date])],
      endDate: ['', [Validator.dateOrNull]],
      pictureUri: '',
      websiteUrl: ''
    })
  }

  ngOnInit(): void {
  }


  saveProject(value: Project) {
    this.project = value
    if (this.checkoutForm.status === 'INVALID') {
      window.alert("Not all fields are correct");
    } else {
      this.projectService.postProject(this.project).then();
    }
  }
}
