import {Component, OnInit} from '@angular/core';
import {UploadService} from "../../services/upload.service";
import {Document} from "../../models/document";

@Component({
  selector: 'app-privacy',
  templateUrl: './privacy.component.html',
  styleUrls: ['./privacy.component.scss']
})
export class PrivacyComponent implements OnInit {

  constructor(private uploadService: UploadService) {
  }

  PP: Document;

  async ngOnInit(): Promise<void> {
    await this.uploadService.getPP().then(res => {
      if (res)
        this.PP = res;
    })
  }
}
