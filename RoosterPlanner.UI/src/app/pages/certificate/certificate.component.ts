import {Component, OnInit} from '@angular/core';
import {Certificate} from "../../models/Certificate";
import {ActivatedRoute, ParamMap, Router} from "@angular/router";
import {CertificateService} from "../../services/certificate.service";
import {UserService} from "../../services/user.service";
import {AddCertificateComponent} from "../../components/add-certificate/add-certificate.component";
import {MatDialog} from "@angular/material/dialog";
import {ToastrService} from "ngx-toastr";
import {BreadcrumbService} from "../../services/breadcrumb.service";
import {ConfirmDialogComponent, ConfirmDialogModel} from "../../components/confirm-dialog/confirm-dialog.component";

@Component({
  selector: 'app-certificate',
  templateUrl: './certificate.component.html',
  styleUrls: ['./certificate.component.scss']
})
export class CertificateComponent implements OnInit {
  guid: string;
  certificate: Certificate
  isAdmin: boolean = false;
  loaded: boolean = false;

  constructor(private route: ActivatedRoute,
              private certificateService: CertificateService,
              private userService: UserService,
              public dialog: MatDialog,
              private toastr: ToastrService,
              private router: Router,
              private breadcrumbService: BreadcrumbService,) {
    this.breadcrumbService.backcrumb();
  }

  ngOnInit(): void {
    this.isAdmin = this.userService.userIsAdminFrontEnd()
    this.route.paramMap.subscribe((params: ParamMap) => {
      this.guid = params.get('id');
      this.getCertificate()
    });
  }

  getCertificate() {
    this.certificateService.getCertificate(this.guid).then(res => {
      if (res)
        this.certificate = res;
      this.loaded = true;
    });
  }


  edit() {
    const dialogRef = this.dialog.open(AddCertificateComponent, {
      width: '400px',
      data: {
        modifier: 'wijzigen',
        certificate: this.certificate
      }
    });
    dialogRef.disableClose = true;
    dialogRef.afterClosed().subscribe(async result => {
      if (result && result !== 'false') {
        this.getCertificate()
        this.toastr.success("Certificaat is gewijzigd")
      }
    });
  }

  delete() {
    const message = "Weet je zeker dat je dit certificaat wilt verwijderen?"
    const dialogData = new ConfirmDialogModel("Bevestig verwijderen", message, "ConfirmationInput", null);
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: "400px",
      data: dialogData
    });
    dialogRef.afterClosed().subscribe(async dialogResult => {
      if (dialogResult === true) {
        await this.certificateService.deleteCertificate(this.guid).then(async response => {
          if (response) {
            this.router.navigateByUrl(this.breadcrumbService.previousUrl)
          }
        })
      }
    })
  }
}
