import {FormControl} from '@angular/forms';


export class Validator {

  static date(control: FormControl): { [key: string]: any } {
    let datePattern = /^(0[1-9]|[12][0-9]|3[01])[-](0[1-9]|1[012])[-](19|20)\d\d$/g;

    if (!control.value.match(datePattern))
      return {"date": true};

    return null;
  }

  static dateOrNull(control: FormControl): { [key: string]: any } {
    if (!(control && control.value)) {
      return null;
    }
    return Validator.date(control);
  }

  static phoneNumber(control: FormControl): { [key: string]: any } {
    let numberPattern = "^((\\+|00(\\s|\\s?\\-\\s?)?)31(\\s|\\s?\\-\\s?)?(\\(0\\)[\\-\\s]?)?|0)[1-9]((\\s|\\s?\\-\\s?)?[0-9])((\\s|\\s?-\\s?)?[0-9])((\\s|\\s?-\\s?)?[0-9])\\s?[0-9]\\s?[0-9]\\s?[0-9]\\s?[0-9]\\s?[0-9]$";

    if (!control.value.match(numberPattern))
      return {"phoneNumber": true};

    return null;
  }

  static postalCode(control: FormControl): { [key: string]: any } {
    let postalCodePattern = /^[1-9][0-9]{3} ?(?!sa|sd|ss)[a-z]{2}$/i;

    if (!control.value.match(postalCodePattern))
      return {"postalCode": true};

    return null;
  }
}
