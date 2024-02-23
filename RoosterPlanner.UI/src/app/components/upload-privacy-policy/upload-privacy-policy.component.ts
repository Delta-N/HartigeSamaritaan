import {Component, OnInit} from '@angular/core';
import {Document} from '../../models/document';
import {MatDialogRef} from '@angular/material/dialog';
import {UploadService} from '../../services/upload.service';
import {EntityHelper} from '../../helpers/entity-helper';
import * as moment from 'moment';
import {ToastrService} from 'ngx-toastr';

@Component({
  selector: 'app-upload-privacy-policy',
  templateUrl: './upload-privacy-policy.component.html',
  styleUrls: ['./upload-privacy-policy.component.scss']
})
export class UploadPrivacyPolicyComponent implements OnInit {
  PP: Document;
  date: string;
  UpdatedPP: Document = new Document();
  files: FileList;


  constructor(public dialogRef: MatDialogRef<UploadPrivacyPolicyComponent>,
              public uploadService: UploadService,
              public toastr: ToastrService) {
  }

  ngOnInit(): void {
    this.getTOS();
  }

  getTOS() {
    this.uploadService.getPP().then(res => {
      if (res) {
        this.PP = res;
        this.date = moment(this.PP.lastEditDate).format('LLLL');
      }

    });
  }

  close() {
    this.dialogRef.close('false');
  }

  async save() {
    if (this.files && this.files[0]) {
      const formData = new FormData();
      formData.append(this.files[0].name, this.files[0]);

      let uri: string = null;
      await this.uploadService.uploadPP(formData).then(url => {
        if (url && url.path && url.path.trim().length > 0) {
          uri = url.path.trim();
        }
      });

      this.UpdatedPP.name = 'Privacy Policy';
      this.UpdatedPP.documentUri = uri;
      if (this.PP) {
        this.UpdatedPP.id = this.PP.id;
        this.UpdatedPP.rowVersion = this.PP.rowVersion;
        this.UpdatedPP.lastEditDate = this.PP.lastEditDate;
        this.UpdatedPP.lastEditBy = this.PP.lastEditBy;
        await this.uploadService.updateDocument(this.UpdatedPP).then(res => {
          if (res) {
            this.dialogRef.close(res);
          }
        });
      } else {
        this.UpdatedPP.id = EntityHelper.returnEmptyGuid();
        await this.uploadService.postDocument(this.UpdatedPP).then(res => {
          if (res) {
            this.dialogRef.close(res);
          }
        });
      }
    }
  }

  uploadPP(files: FileList) {
    let correctExtention = true;
    for (let i = 0; i < files.length; i++) {
      if (files[i].name.substring(files[i].name.lastIndexOf('.') + 1) !== 'pdf') {
        this.toastr.warning('Er mogen alleen PDF bestanden geupload worden');
        correctExtention = false;
      }
    }
    if (correctExtention) {
      this.files = files;
    }
  }
}

