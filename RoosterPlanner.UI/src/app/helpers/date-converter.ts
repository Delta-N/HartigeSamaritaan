import {Project} from "../models/project";

export class DateConverter {
  static date: Date

  static toDate(str) {
    const [day, month, year] = str.split("-")

    this.date = new Date();
    const offset = this.date.getTimezoneOffset();
    return new Date(year, month - 1, day,(offset*-1/60)+1); //+1 to compensate daylight saving
  }
//alle dates worden alleen naar de gebruiker toe geconverteerd naar een leesbarevorm
  static toReadableString(date:string){
    const day = date.substring(8,10);
    const month = date.substring(5,7);
    const year = date.substring(0,4);

    return String(day+"-"+month+"-"+year);
  }
  static formatProjectDateReadable(project): Project {
    project.startDate = DateConverter.toReadableString(project.startDate);
    project.endDate != null ? project.endDate = DateConverter.toReadableString(project.endDate) : project.endDate = null;
    return project
  }
  static formatProjectDateSendable(project):Project{
    project.startDate = DateConverter.toDate(project.startDate);
    project.endDate != null ? project.endDate = DateConverter.toDate(project.endDate) : project.endDate = null;
    return project
  }
}
