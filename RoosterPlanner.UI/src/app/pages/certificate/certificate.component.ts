import {Component, ElementRef, OnInit, ViewChild} from '@angular/core';
import {Certificate} from "../../models/Certificate";
import {ActivatedRoute, ParamMap, Router} from "@angular/router";
import {CertificateService} from "../../services/certificate.service";
import {UserService} from "../../services/user.service";
import {AddCertificateComponent} from "../../components/add-certificate/add-certificate.component";
import {MatDialog} from "@angular/material/dialog";
import {ToastrService} from "ngx-toastr";
import {BreadcrumbService} from "../../services/breadcrumb.service";
import {ConfirmDialogComponent, ConfirmDialogModel} from "../../components/confirm-dialog/confirm-dialog.component";
import jsPDF from "jspdf";
import {DateConverter} from "../../helpers/date-converter";
import {Breadcrumb} from "../../models/breadcrumb";

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

  @ViewChild('pdfTable', {static: false}) pdfTable: ElementRef;


  constructor(private route: ActivatedRoute,
              private certificateService: CertificateService,
              private userService: UserService,
              public dialog: MatDialog,
              private toastr: ToastrService,
              private router: Router,
              private breadcrumbService: BreadcrumbService,) {

  }

  ngOnInit(): void {
    this.isAdmin = this.userService.userIsAdminFrontEnd()
    this.route.paramMap.subscribe(async (params: ParamMap) => {
      this.guid = params.get('id');
      await this.getCertificate()

      let previousUrl = this.isAdmin ? 'admin/profile/' + this.certificate.person.id : "/profile";
      let previous: Breadcrumb = new Breadcrumb('Profiel', previousUrl);
      let current: Breadcrumb = new Breadcrumb('Certificaat', null);

      let breadcrumbs: Breadcrumb[] = [this.breadcrumbService.dashboardcrumb, previous, current]
      this.breadcrumbService.replace(breadcrumbs);

    });
  }

  async getCertificate() {
    await this.certificateService.getCertificate(this.guid).then(res => {
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
            window.history.back()
          }
        })
      }
    })
  }

  pdf() {
    const doc = new jsPDF({orientation: "l", unit: "mm", format: [210, 297]});


    let img = new Image()
    img.src = "../../../assets/Logo.png"

    let text: string;

    doc.rect(10, 10, doc.internal.pageSize.width - 20, doc.internal.pageSize.height - 20, 'S');

    let imgWidth: number = 100
    doc.addImage(img, 'png', this.getOffSet(doc, imgWidth), 20, imgWidth, 35)

    doc.setFont("Century Schoolbook")

    doc.setFontSize(40)
    text = "CERTIFICAAT"
    doc.text(text, this.getOffSet(doc, this.getTextWidth(doc, text)), 80)

    doc.setFontSize(16)
    text = "Hiermee bevestigen wij dat:"
    doc.text(text, this.getOffSet(doc, this.getTextWidth(doc, text)), 90)

    doc.setFontSize(24)
    text = this.certificate.person.firstName + " " + this.certificate.person.lastName
    doc.text(text, this.getOffSet(doc, this.getTextWidth(doc, text)), 100)

    doc.setFontSize(16)
    text = "Met goed resultaat kunde heeft laten zien op het gebied van"
    doc.text(text, this.getOffSet(doc, this.getTextWidth(doc, text)), 110)

    doc.setFontSize(24)
    text = this.certificate.certificateType ? this.certificate.certificateType.name : "Onbekend"
    doc.text(text, this.getOffSet(doc, this.getTextWidth(doc, text)), 120)

    doc.setFontSize(16)
    if (this.certificate.certificateType?.level) {
      text = "Niveau behaald: " + this.certificate.certificateType.level
      doc.text(text, this.getOffSet(doc, this.getTextWidth(doc, text)), 130)
    }

    text = "Afgiftedatum: " + DateConverter.toReadableStringFromDate(this.certificate.dateIssued)
    doc.text(text, this.getOffSet(doc, this.getTextWidth(doc, text)), 150)


    if (this.certificate.dateExpired) {
      text = "Verloopdatum: " + DateConverter.toReadableStringFromDate(this.certificate.dateExpired)
      doc.text(text, this.getOffSet(doc, this.getTextWidth(doc, text)), 160)
    }

    doc.save('Certificaat.pdf');
  }

  getTextWidth(doc: jsPDF, text: string) {
    return doc.getStringUnitWidth(text) * doc.getFontSize() / doc.internal.scaleFactor;
  }

  getOffSet(doc: jsPDF, textWidth: number) {
    return (doc.internal.pageSize.width - textWidth) / 2;
  }
}
