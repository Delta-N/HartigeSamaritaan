export class DateConverter {

  static toDate (str) {
    const [day, month, year] = str.split("-")
    return new Date(year, month - 1, day)
  }
}
