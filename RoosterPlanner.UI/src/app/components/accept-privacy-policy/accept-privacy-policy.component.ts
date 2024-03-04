import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { NgxDocViewerModule } from 'ngx-doc-viewer';
import { MatCheckbox } from '@angular/material/checkbox';
import { MaterialModule } from '../../modules/material/material.module';

@Component({
	selector: 'app-accept-privacy-policy',
	templateUrl: './accept-privacy-policy.component.html',
	standalone: true,
	imports: [NgxDocViewerModule, MatCheckbox, MaterialModule],
	styleUrls: ['./accept-privacy-policy.component.scss'],
})
export class AcceptPrivacyPolicyComponent implements OnInit {
	checked: any;

	constructor(
		public dialogRef: MatDialogRef<AcceptPrivacyPolicyComponent>,
		@Inject(MAT_DIALOG_DATA) public data: any
	) {}

	async ngOnInit(): Promise<void> {}

	save() {
		this.dialogRef.close('true');
	}
}
