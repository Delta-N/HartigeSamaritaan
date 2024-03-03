import {Component, Inject, OnDestroy, OnInit} from '@angular/core';
import {RouterOutlet} from '@angular/router';
import {faHome, faUserLock, faUserCog, faSignOutAlt, faUser, faUserEdit} from '@fortawesome/free-solid-svg-icons';
import {User} from "./models/user";
import {Subject, takeUntil} from "rxjs";
import {MatDialog} from "@angular/material/dialog";
import {AddProjectComponent} from "./components/add-project/add-project.component";
import {MaterialModule} from "./modules/material/material.module";
import moment from "moment";
import {UploadService, DocumentViewModel, PersonsService, PersonViewModel} from "@RoosterPlanner/openapi";

import {MSAL_GUARD_CONFIG, MsalBroadcastService, MsalGuardConfiguration, MsalService} from "@azure/msal-angular";
import {ChangeProfilePictureComponent} from "./components/change-profile-picture/change-profile-picture.component";
import {AcceptPrivacyPolicyComponent} from "./components/accept-privacy-policy/accept-privacy-policy.component";
import {
  AuthenticationResult,
  EventMessage,
  EventType, IdTokenClaims, InteractionStatus,
  InteractionType,
  PopupRequest, RedirectRequest
} from "@azure/msal-browser";
import {BreadcrumbComponent} from "./components/breadcrumb/breadcrumb.component";
import {filter} from "rxjs/operators";
import {environment} from "../environments/environment";
import {tapResponse} from '@ngrx/component-store';
import {ErrorService} from "./services/error.service";
import {HttpErrorResponse} from "@angular/common/http";
import {UserService} from "./services/user.service";


@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, MaterialModule, BreadcrumbComponent,

  ],
  providers: [],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit, OnDestroy {
  homeIcon = faHome;
  adminIcon = faUserLock;
  managerIcon = faUserCog;
  signOutIcon = faSignOutAlt;
  profileIcon = faUser;
  editIcon = faUserEdit

  public hasUser = false;
  title = 'Hartige Samaritaan';
  isIframe = false;
  loggedIn = false;
  isAdmin = false;
  isManager: boolean;

  user: PersonViewModel;
  PP: DocumentViewModel;

  private readonly _destroying$ = new Subject<void>();

  constructor(public dialog: MatDialog,

              private personService: PersonsService,
              private userService:UserService,
              @Inject(MSAL_GUARD_CONFIG) private msalGuardConfig: MsalGuardConfiguration,
              private authService: MsalService,
              private msalBroadcastService: MsalBroadcastService,
              private uploadService: UploadService,
              private errorService: ErrorService
  ) {
  }

  async ngOnInit() {

    moment.locale('nl')
    this.isIframe = window !== window.parent && !window.opener;
    await this.checkAccount();

    this.authService.instance.enableAccountStorageEvents();


    this.msalBroadcastService.msalSubject$
      .pipe(
        filter((msg: EventMessage) => msg.eventType === EventType.ACCOUNT_ADDED || msg.eventType === EventType.ACCOUNT_REMOVED),
      )
      .subscribe(({eventType, payload}: EventMessage) => {
        switch (eventType) {
          case EventType.ACCOUNT_ADDED:
          case EventType.ACCOUNT_REMOVED:
            this.onAccountAddedOrRemoved();
            break;
          case EventType.LOGIN_SUCCESS:
            this.onLoginSuccess(payload as AuthenticationResult);
            break;
        }
      });

    this.msalBroadcastService.inProgress$
      .pipe(
        takeUntil(this._destroying$),
        filter((status: InteractionStatus) => status === InteractionStatus.None)
      )
      .subscribe(() => {
        this.checkAccount();

      });


    this.uploadService.apiUploadPrivacyPolicyGet().pipe(
      tapResponse(
        res => {
          this.PP = res;
        },
        (error: HttpErrorResponse) => {
          this.errorService.httpError(error);
        }
      )
    ).subscribe();

    // await this.uploadService.getPP().then(res => {
    //   if (res)
    //     this.PP = res;
    // })

    const idToken = this.authService.instance.getActiveAccount()?.localAccountId;
    console.log(this.authService.instance.getActiveAccount())
    this.personService.apiPersonsIdGet(idToken!).pipe(
      tapResponse(
        (res) => {
          this.user = res;
          if (this.PP && (!this.user.termsOfUseConsented || moment(this.PP.lastEditDate) > moment(this.user.termsOfUseConsented)))
            this.promptPPAccept();
        },
        (error: HttpErrorResponse) => {
          this.errorService.httpError(error);
        }

      )
    ).subscribe();

  }

  private onAccountAddedOrRemoved(): void {
    const countAllAccounts = this.authService.instance.getAllAccounts().length;
    if (!countAllAccounts) {
      window.location.pathname = '/';
    } else {
      this.checkAccount();
    }
  }

  private onLoginSuccess(payload: AuthenticationResult): void {

    if (!payload?.account) {
      return;
    }
    this.authService.instance.setActiveAccount(payload?.account);
  }

  async checkAccount() {
    let activeAccount = this.authService.instance.getActiveAccount();

    if (!activeAccount && this.authService.instance.getAllAccounts().length > 0) {
      let accounts = this.authService.instance.getAllAccounts();
      this.authService.instance.setActiveAccount(accounts[0]);
    }
    this.loggedIn = this.authService.instance.getAllAccounts().length > 0;
    this.isAdmin = this.userService.userIsAdminFrontEnd();
    this.isManager = this.userService.userIsProjectAdminFrontEnd();
  }

  openDialog() {
    this.dialog.open(AddProjectComponent);
  }

  login(userFlowRequest?: RedirectRequest | PopupRequest) {
    if (this.msalGuardConfig.interactionType === InteractionType.Popup) {
      if (this.msalGuardConfig.authRequest) {
        this.authService.loginPopup({...this.msalGuardConfig.authRequest, ...userFlowRequest} as PopupRequest)
          .subscribe((response: AuthenticationResult) => {
            this.authService.instance.setActiveAccount(response.account);
          });
      } else {
        this.authService.loginPopup(userFlowRequest)
          .subscribe((response: AuthenticationResult) => {
            this.authService.instance.setActiveAccount(response.account);
          });
      }
    } else {
      if (this.msalGuardConfig.authRequest) {
        this.authService.loginRedirect({...this.msalGuardConfig.authRequest, ...userFlowRequest} as RedirectRequest);
      } else {
        this.authService.loginRedirect(userFlowRequest);
      }
    }
  }

  logout() {
    if (this.msalGuardConfig.interactionType === InteractionType.Popup) {
      this.authService.logoutPopup({
        mainWindowRedirectUri: "/"
      });
    } else {
      this.authService.logoutRedirect();
    }
  }

  editProfile() {
    let editProfileFlowRequest: RedirectRequest | PopupRequest = {
      authority: environment.b2cPolicies.authorities.editProfile.authority,
      scopes: [],
    };

    this.login(editProfileFlowRequest);
  }

  ngOnDestroy(): void {
    this._destroying$.next(undefined);
    this._destroying$.complete();
  }


  private isAuthenticated(): void {
    this.hasUser = this.authService.instance.getAllAccounts().length > 0;
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
        this.personService.apiPersonsUpdatePersonPut(this.user)
          .pipe(tapResponse(
            res => {
              this.user = res;
            },
            (error: HttpErrorResponse) => {
              this.errorService.httpError(error);
            }
          )).subscribe()

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
        window.location.reload()
      }
    });
  }
}



