import {Component, OnInit} from '@angular/core';
import {User} from "../../models/user";
import {UserService} from "../../services/user.service";
import {JwtHelper} from "../../helpers/jwt-helper";
import {MsalService} from "../../msal";
import {DateConverter} from "../../helpers/date-converter";
import {MatDialog} from '@angular/material/dialog';
import {ChangeProfileComponent} from "../../components/change-profile/change-profile.component";
import {ActivatedRoute, ParamMap} from "@angular/router";


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

  constructor(private route: ActivatedRoute,
              private userService: UserService,
              private authenticationService: MsalService,
              private dialog: MatDialog,) {
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

  }

  async loadUserProfile() {

    await this.userService.getUser(this.guid).then(res => {
      if (res.id) {
        this.user = res;
        console.log(this.user)
        this.age = DateConverter.calculateAge(this.user.dateOfBirth);
        this.loaded = true;
      }
    });
  }

  logout() {
    this.authenticationService.logout();
  }

  edit() {
    const dialogRef = this.dialog.open(ChangeProfileComponent, {
      width: '500px',
      data: this.user
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
          console.log(res)
          if (res)
            this.user = res;
        })
      }
    }
  }
}
