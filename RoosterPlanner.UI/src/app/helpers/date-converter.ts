export class DateConverter {
  static date: Date

  static toDate(str) {
    const [day, month, year] = str.split("-")

    this.date = new Date();
    const offset = this.date.getTimezoneOffset();
    return new Date(year, month - 1, day,offset*-1/60);
  }
}
