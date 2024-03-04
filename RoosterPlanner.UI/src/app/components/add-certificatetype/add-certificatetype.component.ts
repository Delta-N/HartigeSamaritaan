import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { FormBuilder, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { CertificateService } from '../../services/certificate.service';
import { CertificateType } from '../../models/CertificateType';
import { EntityHelper } from '../../helpers/entity-helper';

@Component({
	selector: 'app-add-certificatetype',
	templateUrl: './add-certificatetype.component.html',
	styleUrls: ['./add-certificatetype.component.scss'],
})
export class AddCertificatetypeComponent {
	modifier: string = 'toevoegen';
	checkoutForm;
	certificateType: CertificateType;
	updatedCertificateType: CertificateType;

	constructor(
		@Inject(MAT_DIALOG_DATA) public data: any,
		private formBuilder: FormBuilder,
		private toastr: ToastrService,
		public dialogRef: MatDialogRef<AddCertificatetypeComponent>,
		private certificateService: CertificateService
	) {
		data.certificateType !== null
			? (this.certificateType = data.certificateType)
			: (this.certificateType = new CertificateType());
		this.modifier = data.modifier;

		this.checkoutForm = this.formBuilder.group({
			id: [
				this.certificateType.id !== null
					? this.certificateType.id
					: EntityHelper.returnEmptyGuid(),
			],
			name: [
				this.certificateType.name !== null ? this.certificateType.name : '',
				Validators.required,
			],
			level: [
				this.certificateType.level !== null ? this.certificateType.level : '',
			],
		});
	}

	close() {
		this.dialogRef.close('false');
	}

	async saveCertificate(value: CertificateType) {
		this.updatedCertificateType = value;
		if (this.checkoutForm.status === 'INVALID') {
			this.toastr.error('Niet alle velden zijn correct ingevuld');
		} else {
			if (this.modifier === 'toevoegen') {
				await this.certificateService
					.postCertificateType(this.updatedCertificateType)
					.then(async (response) => {
						this.dialogRef.close(response);
					});
			} else if (this.modifier === 'wijzigen') {
				this.updatedCertificateType.lastEditBy =
					this.certificateType.lastEditBy;
				this.updatedCertificateType.lastEditDate =
					this.certificateType.lastEditDate;
				this.updatedCertificateType.rowVersion =
					this.certificateType.rowVersion;
				await this.certificateService
					.updateCertificateType(this.updatedCertificateType)
					.then(async (response) => {
						this.dialogRef.close(response);
					});
			}
		}
	}
}
