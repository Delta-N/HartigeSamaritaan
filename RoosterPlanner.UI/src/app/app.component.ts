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
import * as moment from "moment"
import {JwtHelper} from "./helpers/jwt-helper";
import {ChangeProfilePictureComponent} from "./components/change-profile-picture/change-profile-picture.component";
import {Document} from "./models/document";
import {UploadService} from "./services/upload.service";
import {AcceptPrivacyPolicyComponent} from "./components/accept-privacy-policy/accept-privacy-policy.component";


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit, OnDestroy {

  public hasUser = false;

  title = 'Hartige Samaritaan';
  isIframe = false;
  loggedIn = false;
  isAdmin = false;
  isManager: boolean;

  user: User = new User();
  PP: Document;

  private readonly _destroying$ = new Subject<void>();


  constructor(public dialog: MatDialog,
              private userService: UserService,
              @Inject(MSAL_GUARD_CONFIG) private msalGuardConfig: MsalGuardConfiguration,
              private authService: MsalService,
              private msalBroadcastService: MsalBroadcastService,
              private uploadService: UploadService) {
  }

  async ngOnInit() {
    moment.locale('nl')
    this.isIframe = window !== window.parent && !window.opener;
    this.isAuthenticated();
    this.checkAccount();


    this.msalBroadcastService.msalSubject$
      .pipe(
        filter((msg: EventMessage) => msg.eventType === EventType.LOGIN_SUCCESS || msg.eventType === EventType.ACQUIRE_TOKEN_SUCCESS),
        takeUntil(this._destroying$)
      )
      .subscribe((result) => {
        this.checkAccount();
      });

    await this.uploadService.getPP().then(res => {
      if (res)
        this.PP = res;
    })

    const idToken = JwtHelper.decodeToken(sessionStorage.getItem('msal.idtoken'));
    await this.userService.getUser(idToken.oid).then(async user => {
      if (user) {
        this.user = user

        if (this.PP && (!this.user.termsOfUseConsented || moment(this.PP.lastEditDate) > moment(this.user.termsOfUseConsented)))
          this.promptPPAccept();
      }
    });
  }

  async checkAccount() {
    this.loggedIn = this.authService.getAllAccounts().length > 0;
    this.isAdmin = this.userService.userIsAdminFrontEnd();
    this.isManager = this.userService.userIsProjectAdminFrontEnd();
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

  promptPPAccept() {
    const dialogRef = this.dialog.open(AcceptPrivacyPolicyComponent, {
      width: '95vw',
      height: '95vh',
      data: this.PP
    });
    dialogRef.disableClose = true;
    dialogRef.afterClosed().subscribe(async result => {
      if (result && result === 'true') {
        this.user.termsOfUseConsented = moment().subtract(moment().utcOffset(), "minutes").toDate().toISOString()
        await this.userService.updateUser(this.user).then(()=> window.location.reload())
      }
    });
  }

  changeProfilePicture() {
    const dialogRef = this.dialog.open(ChangeProfilePictureComponent, {
      width: '300px',
      data: this.user
    });
    dialogRef.disableClose = false;
    dialogRef.afterClosed().subscribe(res => {
      if (res) {
        if (res === 'removed')
          this.user.profilePicture = null
        else
          this.user = res
      }

    });
  }
}

