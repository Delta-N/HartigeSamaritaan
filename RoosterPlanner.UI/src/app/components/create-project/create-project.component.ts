import {Component, OnInit} from '@angular/core';
import {Project} from "../../models/project";
import {FormBuilder, Validators} from "@angular/forms";
import {ProjectService} from "../../services/project.service";
import {Validator} from "../../helpers/validators"
import {ToastrService} from "ngx-toastr";
import {MatDialogRef} from "@angular/material/dialog";

@Component({
  selector: 'app-create-project',
  templateUrl: './create-project.component.html',
  styleUrls: ['./create-project.component.scss']
})
export class CreateProjectComponent implements OnInit {
  project: Project = new Project('');
  checkoutForm;

  constructor(private formBuilder: FormBuilder, private projectService: ProjectService, private toastr: ToastrService, public dialogRef: MatDialogRef<CreateProjectComponent>) {
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
      this.toastr.error("Niet alle velden zijn correct ingevuld");
    } else {
      this.projectService.postProject(this.project).then();
      this.dialogRef.close(value.name);
    }
  }
}
