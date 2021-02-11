import {ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot} from '@angular/router';
import {MsalService} from './msal.service';
import {Inject, Injectable} from '@angular/core';
import {Location} from "@angular/common";
import {InteractionType} from "@azure/msal-browser";
import {MsalGuardConfiguration} from './msal.guard.config';
import {MSAL_GUARD_CONFIG} from './constants';
import {catchError, concatMap, map} from 'rxjs/operators';
import {Observable, of} from 'rxjs';
import {environment} from "../../environments/environment";


@Injectable()
export class MsalGuard implements CanActivate {
  constructor(
    @Inject(MSAL_GUARD_CONFIG) private msalGuardConfig: MsalGuardConfiguration,
    private authService: MsalService,
    private location: Location,
  ) {
  }

  /**
   * Builds the absolute url for the destination page
   * @param path Relative path of requested page
   * @returns Full destination url
   */
  getDestinationUrl(path: string): string {
    // Absolute base url for the application (default to origin if base element not present)
    const baseElements = document.getElementsByTagName("base");
    const baseUrl = this.location.normalize(baseElements.length ? baseElements[0].href : window.location.origin);

    // Path of page (including hash, if using hash routing)
    const pathUrl = this.location.prepareExternalUrl(path);

    // Hash location strategy
    if (pathUrl.startsWith("#")) {
      return `${baseUrl}/${pathUrl}`;
    }

    // If using path location strategy, pathUrl will include the relative portion of the base path (e.g. /base/page).
    // Since baseUrl also includes /base, can just concatentate baseUrl + path
    return `${baseUrl}${path}`;
  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | Observable<boolean> {
    return this.authService.handleRedirectObservable()
      .pipe(
        concatMap(() => {
          if (!this.authService.getAllAccounts().length) {
            let x = this.loginInteractively(state.url);
            this.checkTokenInCache()
            return x;
          } else {
            this.checkTokenInCache()
            return of(true);
          }

        }),
        catchError((err) => {
          if (err.errorMessage.indexOf('AADB2C90118') > -1) {
            window.location.href='https://roosterplanneridp.b2clogin.com/roosterplanneridp.onmicrosoft.com/oauth2/v2.0/authorize?p=B2C_1_reset&client_id=23cbcba3-683e-4fea-bf57-f25d3dc4f0fc&nonce=defaultNonce&redirect_uri=https%3A%2F%2Frooster.hartigesamaritaan.nl&scope=openid&response_type=id_token&prompt=login'
            this.authService.loginRedirect({
              authority: environment.authorities.resetPassword.authority,
              scopes: environment.scopes
            })
            this.authService.logout()
            return of(false);
          }
          return of(false);
        }))

  }

  checkTokenInCache() {
    const request = {
      account: this.authService.getAllAccounts()[0],
      scopes: environment.scopes
    };
    if (sessionStorage.getItem("msal.idtoken") == null) {
      this.authService.acquireTokenSilent(request).toPromise().then(token => {
        sessionStorage.setItem("msal.idtoken", token.idToken)
      }, Error => {
        this.authService.acquireTokenRedirect(request).toPromise().then(token => {
        })
      })
    }
  }

  private loginInteractively(url: string): Observable<boolean> {
    const redirectStartPage = this.getDestinationUrl(url);
    if (this.msalGuardConfig.interactionType === InteractionType.Popup) {
      return this.authService.loginPopup({
        scopes: environment.scopes,
        ...this.msalGuardConfig.authRequest,
      })
        .pipe(
          map(() => true),
          catchError(() => of(false))
        );
    }


    this.authService.loginRedirect({
      redirectStartPage,
      scopes: environment.scopes,
      ...this.msalGuardConfig.authRequest,
    });
    return of(false);
  }


}
