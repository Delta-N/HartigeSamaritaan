import {Injectable} from '@angular/core';
import {ToastrService} from "ngx-toastr";
import {HttpErrorResponse,} from "@angular/common/http";
import {ErrorMessage} from "../models/Error";

@Injectable({
  providedIn: 'root'
})
export class ErrorService {

  constructor(private toastr: ToastrService) {

  }

  httpError(httpResponse: HttpErrorResponse) {
    if (httpResponse.status === 422) {
      let error: ErrorMessage = httpResponse.error
      if (error.type === 1)
        this.toastr.warning(error.message)

      if (error.type === 2)
        this.toastr.error(error.message)
    } else {
      this.error(httpResponse.error)
    }
  }

  error(error: string) {
    this.toastr.error(error);
  }
}
