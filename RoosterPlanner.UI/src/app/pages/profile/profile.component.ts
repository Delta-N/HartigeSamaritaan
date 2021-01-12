import {Component, OnInit} from '@angular/core';
import {User} from "../../models/user";
import {UserService} from "../../services/user.service";
import {JwtHelper} from "../../helpers/jwt-helper";
import {MsalService} from "../../msal";
import {DateConverter} from "../../helpers/date-converter";
import {MatDialog} from '@angular/material/dialog';
import {ChangeProfileComponent} from "../../components/change-profile/change-profile.component";
import {ActivatedRoute, ParamMap} from "@angular/router";
import {Certificate} from "../../models/Certificate";
import {AddCertificateComponent} from "../../components/add-certificate/add-certificate.component";
import {ToastrService} from "ngx-toastr";


@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {
  user: User;
  age: string;
  loaded: boolean;
  guid: string;
  isStaff: boolean;
  isAdmin: boolean;

  certificates: Certificate[] = [];
  certificateStyle = 'card';
  itemsPerCard = 5;
  reasonableMaxInteger = 10000;
  certificateElementHeight: number;
  CertificateExpandbtnDisabled: boolean = true;

  constructor(private route: ActivatedRoute,
              private userService: UserService,
              private authenticationService: MsalService,
              private dialog: MatDialog,
              private toastr: ToastrService) {
  }

  async ngOnInit(): Promise<void> {
    this.route.paramMap.subscribe((params: ParamMap) => {
      this.guid = params.get('id');
    });
    if (!this.guid) {
      const idToken = JwtHelper.decodeToken(sessionStorage.getItem('msal.idtoken'));
      this.guid = idToken.oid;
    }
    this.loadUserProfile().then();
    this.isStaff = this.userService.userIsProjectAdminFrontEnd();
    this.isAdmin = this.userService.userIsAdminFrontEnd()

  }

  async loadUserProfile() {

    await this.userService.getUser(this.guid).then(res => {
      if (res) {
        this.user = res;
        this.certificates = this.user.certificates.slice(0, this.itemsPerCard)
        if (this.user.certificates.length > 5)
          this.CertificateExpandbtnDisabled = false;
        this.age = DateConverter.calculateAge(this.user.dateOfBirth);
        this.loaded = true;
      }
    });
  }

  edit() {
    const dialogRef = this.dialog.open(ChangeProfileComponent, {
      width: '500px',
      data: {
        user: this.user,
        title: "Profiel wijzigen"
      }
    });
    dialogRef.disableClose = true;
    dialogRef.afterClosed().subscribe(result => {
      if (result != null) {
        this.user = result;
        this.age = DateConverter.calculateAge(this.user.dateOfBirth)
      }
    })
  }

  editRemark(id: string) {
    let element = document.getElementById(id);
    let textAreaId = id === 'personalbutton' ? 'personalremark' : 'staffremark'
    let originalText = id === 'personalbutton' ? this.user.personalRemark : this.user.staffRemark
    let textareaElement = document.getElementById(textAreaId) as HTMLInputElement

    if (element.innerText === 'Aanpassen') {
      element.innerText = 'Opslaan'
      textareaElement.disabled = false;
    } else {
      element.innerText = 'Aanpassen'
      textareaElement.disabled = true;
      if (textareaElement.value !== originalText) {
        id === 'personalbutton' ? this.user.personalRemark = textareaElement.value : this.user.staffRemark = textareaElement.value
        this.userService.updatePerson(this.user).then(res => {
          if (res)
            this.loadUserProfile()
        })
      }
    }
  }

  expandCertificateCard() {

    let leftElement = document.getElementById("left")
    let rightElement = document.getElementById("right")
    let expendedCardElement = document.getElementById("expanded-card")
    let remarkElement = document.getElementById("remark")

    if (this.certificateStyle === 'expanded-card') {
      if (leftElement)
        leftElement.hidden = false;
      if (rightElement)
        rightElement.hidden = false;
      if (remarkElement)
        remarkElement.hidden = false;
      if(expendedCardElement)
        expendedCardElement.hidden=true;


      this.certificateStyle = 'card';
      this.itemsPerCard = 5;
      this.certificates = this.user.certificates.slice(0, this.itemsPerCard);
    } else if (this.certificateStyle === 'card') {
      if (leftElement)
        leftElement.hidden = true;
      if (rightElement)
        rightElement.hidden = true;
      if (remarkElement)
        remarkElement.hidden = true;
      if (expendedCardElement)
        expendedCardElement.hidden = false;

      this.certificateStyle = 'expanded-card';
      this.itemsPerCard = this.reasonableMaxInteger;
      this.certificates = this.user.certificates;
      this.certificateElementHeight = this.certificates.length * 48;
    }
  }

  modCertificate(modifier: string) {
    const dialogRef = this.dialog.open(AddCertificateComponent, {
      width: '500px',
      data: {
        modifier: modifier,
        person: this.user,
      }
    });
    dialogRef.disableClose = true;
    dialogRef.afterClosed().subscribe(async res => {
      if (res) {
        await this.loadUserProfile()
      }
    })
  }

  async push() {
    this.user.pushDisabled = !this.user.pushDisabled
    await this.userService.updatePerson(this.user).then(res => {
      if (res) {
        this.loadUserProfile()
        let message = "Push berichten zijn ";
        message += this.user.pushDisabled ? "uitgeschakeld" : "ingeschakeld"
        this.toastr.success(message)
      }
    })
  }
}
