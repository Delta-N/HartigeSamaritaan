import { Component, OnInit } from '@angular/core';
import { JwtHelper } from '../../utilities/jwt-helper';
import { User } from '../../models/user';
import { environment } from '../../../environments/environment';
import { Router, ActivatedRoute } from '@angular/router';
import { UserAgentApplication } from 'msal';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.less']
})
export class ProfileComponent implements OnInit {
  userInfo: User;
  disabled = true;
  redirectUri = `https://DeltanHackaton.b2clogin.com/DeltanHackaton.onmicrosoft.com/oauth2/v2.0/authorize?p=B2C_1_edit&client_id=${environment.options.appId}&nonce=defaultNonce&redirect_uri=${environment.options.redirectUri}profile&scope=openid&response_type=id_token&prompt=login`;
  constructor(private router: Router, private route: ActivatedRoute) {
  }

  ngOnInit() {
    if (this.route.snapshot.params.updated) {
      this.refreshTokenSilent();
    }

    // this.refreshTokenSilent();
    const idToken = JwtHelper.decodeToken(
      sessionStorage.getItem('msal.idtoken')
    );
    this.userInfo = idToken;
  }

  private refreshTokenSilent(): void {
    const authority =
      environment.options.authority;

    const clientApp = window.msal as UserAgentApplication;
    clientApp.authority = authority;
    clientApp.acquireTokenSilent(['user.read']);
  }
}
