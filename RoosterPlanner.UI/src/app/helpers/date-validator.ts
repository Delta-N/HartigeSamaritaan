import {FormControl} from '@angular/forms';


export class DateValidator {

  static date(control: FormControl): { [key: string]: any } {
    let datePattern = /^(0[1-9]|[12][0-9]|3[01])[-](0[1-9]|1[012])[-](19|20)\d\d$/g;

    if (!control.value.match(datePattern))
      return {"date": true};

    return null;
  }
  //todo dit herschrijven zodat date() methode wordt gebuikt.
  static dateOrNull(control: FormControl): { [key: string]: any } {
    let datePattern = /^(0[1-9]|[12][0-9]|3[01])[-](0[1-9]|1[012])[-](19|20)\d\d$/g;

    if (!(control && control.value)) {
      return null;
    }

    if (!control.value.match(datePattern))
      return {"date": true};

    return null;
  }
}
