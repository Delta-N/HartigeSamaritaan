import {Injectable} from '@angular/core';
import {HttpClient, HttpErrorResponse} from '@angular/common/http';
import {Observable, throwError} from 'rxjs';
import {catchError, map} from 'rxjs/operators';

import {JwtHelper} from '../helpers/jwt-helper';
import {MsalService} from '../msal';
import {environment} from '../../environments/environment';
import {SilentRequest} from '@azure/msal-browser';

export interface Options {
  headers?: {
    [header: string]: string | Array<string>;
  };
  params?: {
    [param: string]: string | Array<string>;
  };
  withCredentials?: boolean;
  responseType?: 'json';
  observe?: any;
}

const DEFAULT_HEADERS = {
  'content-type': 'application/json',
  'Cache-Control': 'no-cache',
  Pragma: 'no-cache'
};
const DEFAULT_HEADERS_MULTIPARTFORMDATA = {
  'Cache-Control': 'no-cache',
  Pragma: 'no-cache'
};

/**
 * This service is used to communicate with a Restful API
 * It is an extention on the already existing HttpClient and
 * exist to provide additional functionality like proper error
 * handling and tracking if there is still any requests pending
 */
@Injectable({
  providedIn: 'root'
})
export class ApiService {

  // Constructor
  constructor(private readonly http: HttpClient, private msalService: MsalService) {
    this.checkTokenExpired();
  }

  /* GET request on a Restful API
   * Used to retreive data from rest-API
   * @param url the url endpoint
   * @param options the http options such as headers and params
  */
  get<T>(url: string, options?: Options) {
    return this.http.get<T>(url, this.createHttpOptions(options))
      .pipe(map(this.onSuccessResponse), catchError(this.onErrorResponse));
  }

  /* POST request on a Restful API
   * Used when trying to create a new record
   * @param url the url endpoint
   * @param body the content which has to be sent along the request
   * @param options the http options such as headers and params
  */
  post<T>(url: string, body?: any, options?: Options): Observable<T> {
    return this.http.post<T>(url, body, this.createHttpOptions(options))
      .pipe(map(this.onSuccessResponse), catchError(this.onErrorResponse));
  }

  /* POST request with content type multipart/form-data on a Restful API
   * Used when trying to create a new record
   * @param url the url endpoint
   * @param body the content which has to be sent along the request
   * @param options the http options such as headers and params
  */
  postMultiPartFormData<T>(url: string, body?: any, options?: Options): Observable<T> {
    return this.http.post<T>(url, body, this.createHttpOptionsMultiPartFormData(options))
      .pipe(map(this.onSuccessResponse), catchError(this.onErrorResponse));
  }

  /* PUT request on a Restful API
   * Used when trying to replace an ENTIRE record
   * @param url the url endpoint
   * @param body the content which has to be sent along the request
   * @param options the http options such as headers and params
  */
  put<T>(url: string, body?: any, options?: Options): Observable<T> {
    return this.http.put<T>(url, body, this.createHttpOptions(options))
      .pipe(map(this.onSuccessResponse), catchError(this.onErrorResponse));
  }

  /* PATCH request on a Restful API
   * Used when trying to update some properties of a record
   * hence, only PARTIALLY replacing the record
   * @param url the url endpoint
   * @param body the content which has to be sent along the request
   * @param options the http options such as headers and params
  */
  patch<T>(url: string, body?: any, options?: Options): Observable<T> {
    return this.http.patch<T>(url, body, this.createHttpOptions(options))
      .pipe(map(this.onSuccessResponse), catchError(this.onErrorResponse));
  }

  /* DELETE request on a Restful API
   * Used when trying to (soft-) delete a record.
   * @param url the url endpoint
   * @param options the http options such as headers and params
  */
  delete<T>(url: string, options?: Options): Observable<T> {
    return this.http.delete<T>(url, this.createHttpOptions(options))
      .pipe(map(this.onSuccessResponse), catchError(this.onErrorResponse));
  }

  /* Validate if the token is expired
  */
  private async checkTokenExpired(): Promise<any> {

    const idToken = JwtHelper.decodeToken(sessionStorage.getItem('msal.idtoken'));
    if (idToken) {
      const now = new Date();
      // Add offset so token can be refreshed just before current is expired.
      now.setMinutes(now.getMinutes() + 4);

      const tokenDate = new Date(0);
      tokenDate.setUTCSeconds(idToken.exp);
      if (tokenDate.valueOf() < now.valueOf()) {
        await this.refreshToken();
      }
    } else {
      await Promise.resolve(true);
    }
  }

  /* Will extend the provided options with the default headers provided
   * @param options the http options such as headers and params
  */
  private createHttpOptions(options: Options = {}): Options {
    const {headers = {}, params, observe = 'response' as 'response'} = options;
    return {
      headers: {
        Authorization: `Bearer ${sessionStorage.getItem('msal.idtoken')}`,
        ...DEFAULT_HEADERS,
        ...headers
      },
      params,
      observe
    };
  }

  /* Will extend the provided options with the default headers, for a multipart/form-data request, provided
   * @param options the http options such as headers and params
  */
  private createHttpOptionsMultiPartFormData(options: Options = {}): Options {
    const {headers = {}, params} = options;
    return {
      headers: {
        Authorization: `Bearer ${sessionStorage.getItem('msal.idtoken')}`,
        ...DEFAULT_HEADERS_MULTIPARTFORMDATA,
        ...headers
      },
      params
    };
  }

  /* Whenever a request has resulted in an error it will be
   * rethrown so the error-handler can pick it up later.
   * @param e the error from the restful api
   * @param observable$ the response associated
   */
  private onSuccessResponse<T>(e: T): T {
    return e;
  }

  /* Whenever a request has resulted in an error it will be
   * rethrown so the error-handler can pick it up later.
   * @param e the error from the restful api
   * @param observable$ the with response associated
  */
  private onErrorResponse(e: HttpErrorResponse): Observable<never> {
    return throwError(e);
  }

  /* Perform a silent token refresh.
  */
  private async refreshToken(): Promise<any> {
    const authParameters = {
      account: this.msalService.getAllAccounts()[0],
      authority: environment.auth.authority,
      redirectUri: environment.auth.redirectUri,
      scopes: environment.scopes
    } as SilentRequest;
    await this.msalService.acquireTokenSilent(authParameters);
  }
}
