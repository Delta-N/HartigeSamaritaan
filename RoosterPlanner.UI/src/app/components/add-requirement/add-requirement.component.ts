import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog';
import {FormBuilder, FormControl, Validators} from '@angular/forms';
import {ToastrService} from 'ngx-toastr';
import {CertificateService} from '../../services/certificate.service';
import {EntityHelper} from '../../helpers/entity-helper';
import {Requirement} from '../../models/requirement';
import {CertificateType} from '../../models/CertificateType';
import {RequirementService} from '../../services/requirement.service';

@Component({
  selector: 'app-add-requirement',
  templateUrl: './add-requirement.component.html',
  styleUrls: ['./add-requirement.component.scss']
})
export class AddRequirementComponent implements OnInit {
  modifier: string;
  checkoutForm: any;
  certificateTypeControl: FormControl;


  requirement: Requirement;
  updatedRequirement: Requirement;
  allCertificateTypes: CertificateType[];



  constructor(@Inject(MAT_DIALOG_DATA) public data: any,
              private formBuilder: FormBuilder,
              private toastr: ToastrService,
              public dialogRef: MatDialogRef<AddRequirementComponent>,
              private certificateService: CertificateService,
              private requirementService: RequirementService) {
    this.modifier = data.modifier;
    this.requirement = data.requirement;

    this.certificateTypeControl = new FormControl('', Validators.required);

    this.checkoutForm = this.formBuilder.group({
      id: [this.requirement.id != null ? this.requirement.id : EntityHelper.returnEmptyGuid()],
      certificateType: this.certificateTypeControl
    });
  }

  ngOnInit(): void {
    this.certificateService.getAllCertificateTypes().then(res => {
      if (res) {
        this.allCertificateTypes = res;
        if (this.requirement?.certificateType) {
          this.certificateTypeControl.setValue(this.allCertificateTypes.find(c => c.name === this.requirement.certificateType.name));
        }
      }
    });
  }

  async addRequirement(value: Requirement) {
    this.updatedRequirement = value;

    if (this.checkoutForm.status === 'INVALID') {
      this.toastr.error('Niet alle velden zijn correct ingevuld');
    } else {

      this.updatedRequirement.task = this.requirement.task;
      if (this.modifier === 'toevoegen') {
        await this.requirementService.postRequirement(this.updatedRequirement).then(async response => {
          this.dialogRef.close(response);
        });
      } else if (this.modifier === 'wijzigen') {
        this.updatedRequirement.lastEditBy = this.requirement.lastEditBy;
        this.updatedRequirement.lastEditDate = this.requirement.lastEditDate;
        this.updatedRequirement.rowVersion = this.requirement.rowVersion;
        await this.requirementService.updateRequirement(this.updatedRequirement).then(async response => {
          this.dialogRef.close(response);
        });
      }
    }
  }


  close() {
    this.dialogRef.close('false');
  }
}
