import {Injectable} from '@angular/core';
import {BroadcastService, MsalService} from '@azure/msal-angular';
import {isIE, b2cPolicies} from '../app-config';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  isIframe = false;
  loggedIn = false;

  constructor(private broadcastService: BroadcastService, private authService: MsalService) {
  }

  ngOnInit(): void {
    this.isIframe = window !==window.parent && !window.opener;
    this.checkAccount();

  }

  // other methods
  checkAccount(): boolean {
    this.loggedIn = !!this.authService.getAccount();
    return this.loggedIn;
  }

  login() {
    if (isIE) {
      this.authService.loginRedirect();
    } else {
      this.authService.loginPopup().then();
    }
  }

  logout() {
    this.authService.logout();
  }

  editProfile() {
    if (isIE) {
      this.authService.loginRedirect(b2cPolicies.authorities.editProfile);
    } else {
      this.authService.loginPopup(b2cPolicies.authorities.editProfile).then();
    }
  }
}

