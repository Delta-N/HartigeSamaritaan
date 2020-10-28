export class DateConverter {
  static date: Date

  static toDate(str) {
    const [day, month, year] = str.split("-")

    this.date = new Date();
    const offset = this.date.getTimezoneOffset();
    return new Date(year, month - 1, day,(offset*-1/60)+1); //+1 to compensate daylight saving //todo prio low
  }

  static toReadableString(date:string){
    const day = date.substring(8,10);
    const month = date.substring(5,7);
    const year = date.substring(0,4);

    return String(day+"-"+month+"-"+year);
  }
}
