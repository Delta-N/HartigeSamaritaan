import { Component, OnInit } from '@angular/core';
import {CertificateType} from "../../models/CertificateType";
import {ActivatedRoute, ParamMap, Router} from "@angular/router";
import {MatDialog} from "@angular/material/dialog";
import {ToastrService} from "ngx-toastr";
import {BreadcrumbService} from "../../services/breadcrumb.service";
import {CertificateService} from "../../services/certificate.service";
import {Breadcrumb} from "../../models/breadcrumb";
import {UserService} from "../../services/user.service";
import {AddCertificatetypeComponent} from "../../components/add-certificatetype/add-certificatetype.component";
import {ConfirmDialogComponent, ConfirmDialogModel} from "../../components/confirm-dialog/confirm-dialog.component";

@Component({
  selector: 'app-certificate-type',
  templateUrl: './certificate-type.component.html',
  styleUrls: ['./certificate-type.component.scss']
})
export class CertificateTypeComponent implements OnInit {
  loaded: boolean = false;
  certificateType: CertificateType;
  guid: string;
  isAdmin: boolean = false;


  constructor(private route: ActivatedRoute,
              public dialog: MatDialog,
              private toastr: ToastrService,
              private router: Router,
              private breadcrumbService: BreadcrumbService,
              private certificateService:CertificateService,
              private userService: UserService,
  ) {
    let breadcrumb: Breadcrumb = new Breadcrumb('Certificaattype',null);
    let takencrumb: Breadcrumb = new Breadcrumb("Taken", "admin/tasks");


    let array: Breadcrumb[] = [this.breadcrumbService.dashboardcrumb,
      this.breadcrumbService.admincrumb, takencrumb, breadcrumb];

    this.breadcrumbService.replace(array);
  }

  ngOnInit(): void {
    this.isAdmin = this.userService.userIsAdminFrontEnd();
    this.route.paramMap.subscribe((params: ParamMap) => {
      this.guid = params.get('id');
    });
    this.certificateService.getCertificateType(this.guid).then(response => {
      this.certificateType = response;
      this.loaded = true;
    })
  }

  edit() {
    const dialogRef = this.dialog.open(AddCertificatetypeComponent, {
      width: '400px',
      data: {
        modifier: 'wijzigen',
        certificateType: this.certificateType,
      }
    });
    dialogRef.disableClose = true;
    dialogRef.afterClosed().subscribe(async result => {
      if (result && result !== 'false') {
        this.certificateType = result;
        this.toastr.success(result.name + " is gewijzigd")
      }
    });
  }

  delete() {
    const message = "Weet je zeker dat je dit certificaattype wilt verwijderen?"
    const dialogData = new ConfirmDialogModel("Bevestig verwijderen", message, "ConfirmationInput", null);
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: "400px",
      data: dialogData
    });
    dialogRef.afterClosed().subscribe(async dialogResult => {
      if (dialogResult === true) {
        await this.certificateService.deleteCertificateType(this.guid).then(async response => {
          if (response) {
            this.router.navigate(['admin/tasks']).then();
          }
        })
      }
    })
  }
}
