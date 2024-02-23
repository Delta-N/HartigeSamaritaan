import {Injectable} from '@angular/core';
import {ToastrService} from 'ngx-toastr';
import {HttpErrorResponse, } from '@angular/common/http';
import {ErrorMessage} from '../models/Error';

@Injectable({
  providedIn: 'root'
})
export class ErrorService {

  constructor(private toastr: ToastrService) {

  }

  httpError(httpResponse: HttpErrorResponse) {
    if (httpResponse.status === 422) {
      const error: ErrorMessage = httpResponse.error;
      if (error.type === 1) {
        this.toastr.warning(error.message);
      }

      if (error.type === 2) {
        this.toastr.error(error.message);
      }

    } else {
      if (httpResponse.error === 'Outdated entity received'){
        this.toastr.warning('Pagina word opnieuw geladen...');
        setTimeout(() => {
          window.location.reload();
        }, 1000);
      }
      this.error(httpResponse.error);
    }
  }

  error(error: string) {
    this.toastr.error(error);
  }
}
