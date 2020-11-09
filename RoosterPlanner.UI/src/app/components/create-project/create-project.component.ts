import {Component, Inject, OnInit} from '@angular/core';
import {Project} from "../../models/project";
import {FormBuilder, Validators} from "@angular/forms";
import {ProjectService} from "../../services/project.service";
import {Validator} from "../../helpers/validators"
import {ToastrService} from "ngx-toastr";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {DateConverter} from "../../helpers/date-converter";

@Component({
  selector: 'app-create-project',
  templateUrl: './create-project.component.html',
  styleUrls: ['./create-project.component.scss']
})
export class CreateProjectComponent implements OnInit {
  project: Project = new Project();
  checkoutForm;
  title: string;


  constructor(private formBuilder: FormBuilder,
              private projectService: ProjectService,
              private toastr: ToastrService,
              public dialogRef: MatDialogRef<CreateProjectComponent>,
              @Inject(MAT_DIALOG_DATA) public data: any) {

    let today = DateConverter.todayString()
    if (!this.data.createProject) {
      this.project = this.data.project;
    }
    this.checkoutForm = this.formBuilder.group({
      id: this.project.id != null ? this.project.id : '',
      name: [this.project.name != null ? this.project.name : '', Validators.required],
      address: [this.project.address != null ? this.project.address : '', Validators.required],
      city: [this.project.city != null ? this.project.city : '', Validators.required],
      description: [this.project.description != null ? this.project.description : '', Validators.required],
      startDate: [this.project.startDate != null ? this.project.startDate : today, Validator.date],
      endDate: [this.project.endDate != null ? this.project.endDate : '', [Validator.dateOrNull]],
      pictureUri: this.project.pictureUri != null ? this.project.pictureUri : '',
      websiteUrl: this.project.websiteUrl != null ? this.project.websiteUrl : ''
    })

  }

  ngOnInit(): void {
  }


  saveProject(value: Project) {
    this.project = value
    if (this.checkoutForm.status === 'INVALID') {
      this.toastr.error("Niet alle velden zijn correct ingevuld");
    } else {
      //create new project
      if (this.data.createProject) {
        this.projectService.postProject(this.project).then(response=>this.project=response.body);
      }
      //edit project
      else {
        this.projectService.updateProject(this.project).then(response=>{
          this.project=response.body
        });

      }
      this.dialogRef.close(this.project);
    }
  }
}
