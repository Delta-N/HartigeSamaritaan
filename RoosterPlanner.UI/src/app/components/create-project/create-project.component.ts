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
      participationStartDate: [this.project.participationStartDate != null ? this.project.participationStartDate : '', Validator.date],
      participationEndDate: [this.project.participationEndDate != null ? this.project.participationEndDate : '', [Validator.dateOrNull]],
      projectStartDate: [this.project.projectStartDate != null ? this.project.projectStartDate : today, Validator.date],
      projectEndDate: [this.project.projectEndDate != null ? this.project.projectEndDate : '', Validator.date],
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
      let prSdate = DateConverter.toDate(this.project.projectStartDate)
      let prEdate = DateConverter.toDate(this.project.projectEndDate)
      let parSdate = DateConverter.toDate(this.project.participationStartDate)
      //alleen participation end date is optioneel
      let parEdate = null;
      this.project.participationEndDate!=null?parEdate=DateConverter.toDate(this.project.participationEndDate):null;

      if(prEdate<prSdate){
        this.toastr.error("Project einddatum mag niet voor project startdatum liggen")
        return;
      }
      if(parSdate<prSdate || parSdate>prEdate){
        this.toastr.error("Participatie startdatum moet tussen projectstart en project einddatum liggen")
        return;
      }
      if (parEdate != null && parEdate < parSdate) {
        this.toastr.error("Participatie eindatum mag niet voor de participatie startdatum liggen")
        return;
      }
      if (parEdate != null && parEdate > (prEdate || parEdate<prSdate)) {
        this.toastr.error("Participatie einddatum moet tussen projectstart en project einddatum liggen")
        return;
      }

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
