import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Component, OnInit, Inject } from '@angular/core';

@Component({
	selector: 'app-confirm-dialog',
	templateUrl: './confirm-dialog.component.html',
	styleUrls: ['./confirm-dialog.component.scss'],
})
export class ConfirmDialogComponent implements OnInit {
	title: string;
	message: string;
	type: string;
	value: any;

	constructor(
		public dialogRef: MatDialogRef<ConfirmDialogComponent>,
		@Inject(MAT_DIALOG_DATA) public data: ConfirmDialogModel
	) {
		this.title = data.title;
		this.message = data.message;
		this.type = data.type;
		this.value = data.inputvalue;
	}

	ngOnInit() {}

	onConfirm(): void {
		this.dialogRef.close(true);
	}

	onDismiss(): void {
		this.dialogRef.close(false);
	}

	sendInput(input: any) {
		this.dialogRef.close(input);
	}
}

export class ConfirmDialogModel {
	constructor(
		public title: string,
		public message: string,
		public type: string,
		public inputvalue: any
	) {}
}
