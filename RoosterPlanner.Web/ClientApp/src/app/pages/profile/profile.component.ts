import { Component, OnInit } from '@angular/core';
import { JwtHelper } from '../../utilities/jwt-helper';
import { User } from '../../models/user';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.less']
})
export class ProfileComponent implements OnInit {
  userInfo: User;
  disabled = true;

  constructor() {}

  ngOnInit() {
    const idToken = JwtHelper.decodeToken(
      sessionStorage.getItem('msal.idtoken')
    );
    this.userInfo = idToken;
  }
}
