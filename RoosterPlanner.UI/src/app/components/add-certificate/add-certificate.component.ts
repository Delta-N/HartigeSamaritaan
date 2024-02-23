import {Component, Inject, OnInit} from '@angular/core';
import {CertificateType} from '../../models/CertificateType';
import {Certificate} from '../../models/Certificate';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog';
import {FormBuilder, FormControl, Validators} from '@angular/forms';
import {ToastrService} from 'ngx-toastr';
import {CertificateService} from '../../services/certificate.service';
import {EntityHelper} from '../../helpers/entity-helper';
import {Validator} from '../../helpers/validators';
import {User} from '../../models/user';
import {DateConverter} from '../../helpers/date-converter';

@Component({
  selector: 'app-add-certificate',
  templateUrl: './add-certificate.component.html',
  styleUrls: ['./add-certificate.component.scss']
})
export class AddCertificateComponent implements OnInit {
  modifier: string;
  checkoutForm: any;
  certificateTypeControl: FormControl;

  person: User;
  allCertificateTypes: CertificateType[];
  certificate: Certificate;
  updatedCertificate: Certificate;


  constructor(@Inject(MAT_DIALOG_DATA) public data: any,
              private formBuilder: FormBuilder,
              private toastr: ToastrService,
              public dialogRef: MatDialogRef<AddCertificateComponent>,
              private certificateService: CertificateService) {

    data.person != null ? this.person = data.person : null;
    data.certificate != null ? this.certificate = data.certificate : this.certificate = new Certificate();
    this.modifier = data.modifier;

    this.certificateTypeControl = new FormControl('', Validators.required);
    const today = DateConverter.todayString();

    this.checkoutForm = this.formBuilder.group({
      id: [this.certificate.id != null ? this.certificate.id : EntityHelper.returnEmptyGuid()],
      dateIssued: [this.certificate.dateIssued != null ? DateConverter.toReadableStringFromDate(this.certificate.dateIssued) : today, Validator.date],
      dateExpired: [this.certificate.dateExpired != null ? DateConverter.toReadableStringFromDate(this.certificate.dateExpired) : '', [Validator.dateOrNull]],
      certificateType: this.certificateTypeControl
    });

  }

  ngOnInit(): void {
    this.certificateService.getAllCertificateTypes().then(res => {
      if (res) {
        this.allCertificateTypes = res;
        if (this.certificate?.certificateType) {
          this.certificateTypeControl.setValue(this.allCertificateTypes.find(c => c.name === this.certificate.certificateType.name));
        }

      }
    });
  }


  async addCertificate(value: Certificate) {
    this.updatedCertificate = value;
    if (this.checkoutForm.status === 'INVALID') {
      this.toastr.error('Niet alle velden zijn correct ingevuld');
    } else {
      const iss = DateConverter.toDate(this.updatedCertificate.dateIssued);
      const ex = DateConverter.toDate(this.updatedCertificate.dateExpired);
      if (ex <= iss) {
        this.toastr.error('Verloop datum mag niet voor uitgifte datum liggen');
        return;
      }
      this.updatedCertificate.person = this.person;

      if (this.modifier === 'toevoegen') {
        await this.certificateService.postCertificate(this.updatedCertificate).then(async response => {
          this.dialogRef.close(response);
        });
      } else if (this.modifier === 'wijzigen') {
        this.updatedCertificate.person = this.certificate.person;
        this.updatedCertificate.lastEditBy = this.certificate.lastEditBy;
        this.updatedCertificate.lastEditDate = this.certificate.lastEditDate;
        this.updatedCertificate.rowVersion = this.certificate.rowVersion;
        await this.certificateService.updateCertificate(this.updatedCertificate).then(async response => {
          this.dialogRef.close(response);
        });
      }
    }
  }

  close() {
    this.dialogRef.close('false');
  }
}
