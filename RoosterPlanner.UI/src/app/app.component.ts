import {Component, OnDestroy, OnInit} from '@angular/core';
import {MatDialog} from "@angular/material/dialog";
import {AddProjectComponent} from "./components/add-project/add-project.component";
import {MsalService, BroadcastService} from '@azure/msal-angular';
import {AuthenticationService} from './services/authentication.service';
import {UserService} from "./services/user.service";
import {User} from "./models/user";
import {Subscription} from "rxjs";
import {environment} from "../environments/environment";
import {AuthError, AuthResponse, UserAgentApplication} from "msal";
import {b2cPolicies} from './app-config';
import {Router} from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit, OnDestroy {
  private failureSubscription: Subscription;
  private refreshTokenSubscription: Subscription;
  public hasUser = false;

  title = 'Hartige Samaritaan';
  isIframe = false;
  loggedIn = false;
  isAdmin = false;
  user: User = new User('');

  constructor(public dialog: MatDialog,
              private broadcastService: BroadcastService,
              private authService: MsalService,
              private authenticationService: AuthenticationService,
              private userService: UserService,
              private router: Router) {

    this.authService.handleRedirectCallback((authError: AuthError, response: AuthResponse) => {
      if (authError && authError.errorMessage.indexOf('AADB2C90118') > -1) {
        // change authority to password reset policy
        this.passwordRedirect(b2cPolicies.authorities.resetPassword.authority);
        return;
      }

      // We need to reject id tokens that were not issued with the default sign-in policy.
      // To learn more about b2c tokens, visit https://docs.microsoft.com/en-us/azure/active-directory-b2c/tokens-overview
      if (response && response.tokenType === 'id_token' && response.idToken.claims['tfp'] === b2cPolicies.authorities.resetPassword.authority) {
        this.authenticationService.logout();
        return;
      }
      if (response && response.idToken && response.idToken.objectId && response.expiresOn > new Date()) {
        this.hasUser = true;
      }

      if (authError && !environment.production) {
        console.error('Redirect Error: ', authError.errorMessage);
        return;
      }
    });
  }


  ngOnInit() {
    this.isIframe = window !== window.parent && !window.opener;
    this.subscribeMsalBroadcastEvents();
    this.isAuthenticated();
    this.checkAccount().then();
  }

  ngOnDestroy(): void {
    this.unsubscribeMsalBroadcastEvents();
  }

  private isAuthenticated(): void {
    const account = this.authService.getAccount();
    this.hasUser = !!account;
  }


  /* Subscribe on event for password reset and logout. */
  private subscribeMsalBroadcastEvents(): void {
    this.failureSubscription = this.broadcastService.subscribe(
      'msal:loginFailure', payload => {
        if (payload._errorDesc.indexOf('AADB2C90118') > -1) {
          this.passwordRedirect(b2cPolicies.authorities.resetPassword.authority);
        } else if (payload._errorDesc.indexOf('AADB2C90091') > -1) {
          this.router.navigateByUrl('/').then();
        }
      });
    this.refreshTokenSubscription = this.broadcastService.subscribe(
      'msal:acquireTokenFailure', payload => {
        this.logout();
      });
  }

  /* Unsubscribe from broadcast events when component is destoryed */
  private unsubscribeMsalBroadcastEvents(): void {
    this.broadcastService.getMSALSubject().next(1);
    if (this.failureSubscription) {
      this.failureSubscription.unsubscribe();
    }
    if (this.refreshTokenSubscription) {
      this.refreshTokenSubscription.unsubscribe();
    }
  }

  private passwordRedirect(policyURL: string): void {
    if (policyURL && policyURL.length > 0) {
      const clientApp = window.msal as UserAgentApplication;
      clientApp.authority = policyURL;
      clientApp.loginRedirect();
    }
  }

  openDialog() {
    this.dialog.open(AddProjectComponent);
  }

  // other methods

  async checkAccount() {
    this.loggedIn = this.authenticationService.checkAccount();
    this.isAdmin = this.userService.userIsAdminFrontEnd();
  }

  async testButton() {
  }

  logout() {
    this.authenticationService.logout()
  }

  login() {
    this.authenticationService.logout()
  }
}

