import {Component, OnInit} from '@angular/core';
import {User} from "../../models/user";
import {UserService} from "../../services/user.service";
import {JwtHelper} from "../../helpers/jwt-helper";
import {AuthenticationService} from "../../services/authentication.service";
import {DateConverter} from "../../helpers/date-converter";
import {MatDialog} from '@angular/material/dialog';
import {ChangeProfileComponent} from "../../components/change-profile/change-profile.component";


@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {
  user: User;
  age: number;
  loaded: boolean = false;

  constructor(private userService: UserService, private authenticationService: AuthenticationService, private dialog: MatDialog) {
  }

  async ngOnInit(): Promise<void> {
    this.loadUserProfile().then();
  }

  async loadUserProfile() {
    const idToken = JwtHelper.decodeToken(sessionStorage.getItem('msal.idtoken'));
    await this.userService.getUser(idToken.oid).then(x => {
      this.user = x;
      this.age = this.calculateAge(this.user.dateOfBirth);
      this.loaded = true;
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
    dialogRef.afterClosed().subscribe(async result => {
      this.loadUserProfile().then();
    })
  }

  calculateAge(dateOfBirth: string): number {
    if (dateOfBirth === null || dateOfBirth === undefined)
      return 0;

    const today = new Date();
    const birthDate = DateConverter.toDate(dateOfBirth);
    let age = today.getFullYear() - birthDate.getFullYear();
    const m = today.getMonth() - birthDate.getMonth();

    if (m < 0 || (m === 0 && today.getDate() < birthDate.getDate())) {
      age--;
    }

    return age;
  }

}
