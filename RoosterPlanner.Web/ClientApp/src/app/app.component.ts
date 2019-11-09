import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { MsalService, BroadcastService } from '@azure/msal-angular';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { UserAgentApplication } from 'msal';
import { ProjectService } from './core/project/project.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.less']
})
export class AppComponent implements OnInit, OnDestroy {
  constructor(
    private broadcastService: BroadcastService,
    private msalService: MsalService,
    private router: Router
  ) {}
  title = 'hartige-samaritaan-ui';
  projects: any;

  private failureSubscription: Subscription;
  private refreshTokenSubscription: Subscription;

  ngOnInit(): void {
    this.subscribeMsalBroadcastEvents();
  }
  delay(ms: number) {
    return new Promise(resolve => setTimeout(resolve, ms));
  }

  ngOnDestroy() {
    this.unsubscribeMsalBroadcastEvents();
  }

  private subscribeMsalBroadcastEvents(): void {
    this.failureSubscription = this.broadcastService.subscribe(
      'msal:loginFailure',
      payload => {
        console.log(payload);
        if (payload._errorDesc.indexOf('AADB2C90118') > -1) {
          this.redirectToPolicy(environment.options.passwordResetPolicy);
        } else if (payload._errorDesc.indexOf('AADB2C90091') > -1) {
          this.router.navigateByUrl('/');
        }
      }
    );

    this.refreshTokenSubscription = this.broadcastService.subscribe(
      'msal:acquireTokenFailure',
      payload => {
        this.msalService.logout();
      }
    );
  }

  /**
   * Unsubscribe from broadcast events when component is destoryed
   */
  private unsubscribeMsalBroadcastEvents(): void {
    this.broadcastService.getMSALSubject().next(1);
    if (this.failureSubscription) {
      this.failureSubscription.unsubscribe();
    }

    if (this.refreshTokenSubscription) {
      this.refreshTokenSubscription.unsubscribe();
    }
  }

  /**
   * Checks if a policy is present and redirects to password redirect page
   * @param policyName name of the active policy
   */
  private redirectToPolicy(policyName: string): void {
    if (policyName && policyName.length > 0) {
      const pos = environment.options.authority
        .toLowerCase()
        .indexOf('/b2c_1_');
      const authority =
        environment.options.authority.substring(0, pos) + `/${policyName}/`;

      const clientApp = window.msal as UserAgentApplication;
      clientApp.authority = authority;
      clientApp.loginRedirect(environment.options.consentScopes);
    }
  }
}
