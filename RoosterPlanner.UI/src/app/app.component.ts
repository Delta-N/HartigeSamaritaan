import {Component, Inject, OnDestroy, OnInit} from '@angular/core';
import {MatDialog} from "@angular/material/dialog";
import {AddProjectComponent} from "./components/add-project/add-project.component";
import {UserService} from "./services/user.service";
import {User} from "./models/user";
import {MSAL_GUARD_CONFIG} from "./msal/constants";
import {MsalGuardConfiguration} from "./msal/msal.guard.config";
import {MsalBroadcastService, MsalService} from "./msal";
import {EventMessage, EventType, InteractionType} from "@azure/msal-browser";
import {filter, takeUntil} from "rxjs/operators";
import {Subject} from "rxjs";
import {environment} from "../environments/environment";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit, OnDestroy {
//todo reset password link
// todo remove #state=... from link after redirect

  public hasUser = false;

  title = 'Hartige Samaritaan';
  isIframe = false;
  loggedIn = false;
  isAdmin = false;
  user: User = new User('');
  private readonly _destroying$ = new Subject<void>();


  constructor(public dialog: MatDialog,
              private userService: UserService,
              @Inject(MSAL_GUARD_CONFIG) private msalGuardConfig: MsalGuardConfiguration,
              private authService: MsalService,
              private msalBroadcastService: MsalBroadcastService) {
  }

  ngOnInit() {
    this.isIframe = window !== window.parent && !window.opener;
    //this.checkTokenInCache();
    this.authService.handleRedirectObservable().subscribe({
      next: (result) => console.log("redirect obs " + result),
      error: (error) => console.log("redirect obs err " + error)
    });
    this.isAuthenticated();
    this.checkAccount();


    this.msalBroadcastService.msalSubject$
      .pipe(
        filter((msg: EventMessage) => msg.eventType === EventType.LOGIN_SUCCESS || msg.eventType === EventType.ACQUIRE_TOKEN_SUCCESS),
        takeUntil(this._destroying$)
      )
      .subscribe((result) => {
        console.log(result)
        this.checkAccount();
      });


  }

  checkTokenInCache() {
    let request = {
      account: this.authService.getAllAccounts()[0],
      scopes: environment.scopes
    }
    if (sessionStorage.getItem("msal.idtoken") == null) {
      this.authService.acquireTokenSilent(request).toPromise().then(token => {
        sessionStorage.setItem("msal.idtoken", token.idToken)
      }, Error => {
        this.authService.acquireTokenRedirect(request).toPromise().then(token => {
        })
      })
    }
  }

  async checkAccount() {
    this.loggedIn = this.authService.getAllAccounts().length > 0;
    this.isAdmin = this.userService.userIsAdminFrontEnd();
  }

  openDialog() {
    this.dialog.open(AddProjectComponent);
  }

  logout() {
    this.authService.logout();
  }

  login() {
    if (this.msalGuardConfig.interactionType === InteractionType.Popup) {
      this.authService.loginPopup({...this.msalGuardConfig.authRequest})
        .subscribe(() => this.checkAccount());
    } else {
      this.authService.loginRedirect({...this.msalGuardConfig.authRequest});
    }
  }

  ngOnDestroy(): void {
    this._destroying$.next(null);
    this._destroying$.complete();
  }

  private isAuthenticated(): void {
    const account = this.authService.getAllAccounts()[0];
    this.hasUser = !!account;
  }
}

