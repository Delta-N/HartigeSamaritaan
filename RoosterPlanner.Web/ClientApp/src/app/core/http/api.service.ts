import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpResponse, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { catchError } from 'rxjs/operators';
import { map } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  constructor(private http: HttpClient) { }

  get<T>(url: string, httpHeaders?: Map<string, string>): Observable<T> {
    return this.http.get<T>(`${environment.baseUrl}/${url}`, this.createHttpOptions(httpHeaders))
      .pipe(map(this.onSuccessResponse), catchError(this.onErrorResponse));
  }

  post<T>(url: string, body: any, httpHeaders?: Map<string, string>): Observable<T> {
    return this.http.post<T>(`${environment.baseUrl}/${url}`, body, this.createHttpOptions(httpHeaders))
      .pipe(map(this.onSuccessResponse), catchError(this.onErrorResponse));
  }

  put<T>(url: string, body: any, httpHeaders?: Map<string, string>): Observable<T> {
    return this.http.put<T>(`${environment.baseUrl}/${url}`, body, this.createHttpOptions(httpHeaders))
      .pipe(map(this.onSuccessResponse), catchError(this.onErrorResponse));
  }

  delete<T>(url: string, httpHeaders?: Map<string, string>): Observable<T> {
    return this.http.delete<T>(`${environment.baseUrl}/${url}`, this.createHttpOptions(httpHeaders))
      .pipe(map(this.onSuccessResponse), catchError(this.onErrorResponse));
  }

  private createHttpOptions(dictionary: Map<string, string>): any {
    const requestOptions = {
      headers: new HttpHeaders({
        Authorization: `Bearer ${sessionStorage.getItem('msal.idtoken')}`,
        'Content-Type': 'application/json'
      })
    };
    if (dictionary) {
      dictionary.forEach((value, key) => {
        requestOptions.headers.set(key, value);
      });
    }
    return requestOptions;
  }

  private onSuccessResponse<T>(e: HttpResponse<T>): any {
    return e;
  }

  private onErrorResponse(e: HttpErrorResponse): Observable<never> {
    console.log('error has occured');
    console.log(e);
    return throwError(e);
  }
}
