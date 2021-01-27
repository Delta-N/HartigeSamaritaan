import {FormControl} from '@angular/forms';


export class Validator {

  static date(control: FormControl): { [key: string]: any } {
    if(control==null||control.value===null||control.value.match===null){
      return {"date":true};
    }
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
    if(control==null||control.value===null||control.value.match===null){
      return {"phoneNumber":true};
    }
    let numberPattern = "^((\\+|00(\\s|\\s?\\-\\s?)?)31(\\s|\\s?\\-\\s?)?(\\(0\\)[\\-\\s]?)?|0)[1-9]((\\s|\\s?\\-\\s?)?[0-9])((\\s|\\s?-\\s?)?[0-9])((\\s|\\s?-\\s?)?[0-9])\\s?[0-9]\\s?[0-9]\\s?[0-9]\\s?[0-9]\\s?[0-9]$";

    if (!control.value.match(numberPattern))
      return {"phoneNumber": true};

    return null;
  }

  static postalCode(control: FormControl): { [key: string]: any } {
    if(control==null||control.value===null||control.value.match===null){
      return {"postalCode":true};
    }
    let postalCodePattern = /^[1-9][0-9]{3} ?(?!sa|sd|ss)[a-z]{2}$/i;

    if (!control.value.match(postalCodePattern))
      return {"postalCode": true};

    return null;
  }

  static email(control: FormControl): { [key: string]: any } {
    if (!(control && control.value)) {
      return null;
    }
    let emailPattern = /(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|"(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])/;

    if (!control.value.match(emailPattern))
      return {"email": true};

    return null;
  }
}
