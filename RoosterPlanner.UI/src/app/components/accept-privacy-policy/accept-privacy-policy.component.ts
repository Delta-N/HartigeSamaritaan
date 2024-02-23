import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog';


@Component({
  selector: 'app-accept-privacy-policy',
  templateUrl: './accept-privacy-policy.component.html',
  styleUrls: ['./accept-privacy-policy.component.scss']
})
export class AcceptPrivacyPolicyComponent implements OnInit {

  checked: any;

  constructor(public dialogRef: MatDialogRef<AcceptPrivacyPolicyComponent>,
              @Inject(MAT_DIALOG_DATA) public data: any) {
  }

  async ngOnInit(): Promise<void> {
  }

  save() {
    this.dialogRef.close('true');
  }
}
