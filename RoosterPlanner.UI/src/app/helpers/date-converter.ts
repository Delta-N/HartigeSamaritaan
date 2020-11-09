import {Project} from "../models/project";
import * as moment from "moment"

export class DateConverter {
  static date: Date

  static toDate(str) {
    const offset = moment().utcOffset()

    if(moment().isDST())
      return moment(str,"DD-MM-YYYY").add(offset,'minutes').toDate();
    return  moment(str,"DD-MM-YYYY").add(offset,'minutes').add(1,'hour').toDate();

  }
//alle dates worden alleen naar de gebruiker toe geconverteerd naar een leesbarevorm
  static toReadableString(date:string){
    return moment(date,"YYYY-MM-DDTHH:mm").format("DD-MM-YYYY")

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
  static todayString():String{
    return moment().format("DD-MM-YYYY")
  }
}
