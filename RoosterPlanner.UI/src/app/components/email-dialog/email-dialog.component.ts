import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialog, MatDialogRef} from '@angular/material/dialog';
import {Message} from '../../models/message';
import {FormBuilder, Validators} from '@angular/forms';
import {ToastrService} from 'ngx-toastr';
import {ConfirmDialogComponent, ConfirmDialogModel} from '../confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'app-email-dialog',
  templateUrl: './email-dialog.component.html',
  styleUrls: ['./email-dialog.component.scss']
})
export class EmailDialogComponent implements OnInit {
  message: string;
  subject: string;
  checkoutForm;

  constructor(public dialogRef: MatDialogRef<EmailDialogComponent>,
              @Inject(MAT_DIALOG_DATA) public data,
              private formBuilder: FormBuilder,
              private toastr: ToastrService,
              public dialog: MatDialog) {

    this.checkoutForm = this.formBuilder.group({
      subject: ['', Validators.required],
      message: ['', Validators.required]
    });
  }

  ngOnInit(): void {
  }

  close() {
    this.dialogRef.close('false');
  }

  sendInput() {
    if (this.checkoutForm.status === 'INVALID') {
      this.toastr.error('Niet alle velden zijn correct ingevuld');
    } else {
      const message = 'Weet je zeker dat je dit bericht wilt versturen?';
      const dialogData = new ConfirmDialogModel('Bevestig e-mail', message, 'ConfirmationInput', null);
      const dialogRef = this.dialog.open(ConfirmDialogComponent, {
        maxWidth: '400px',
        data: dialogData
      });

      dialogRef.afterClosed().subscribe(async dialogResult => {
        if (dialogResult === true) {
          const email = new Message();
          email.subject = this.subject;
          email.body = this.message;
          this.dialogRef.close(email);
        }
      });
    }
  }
}
