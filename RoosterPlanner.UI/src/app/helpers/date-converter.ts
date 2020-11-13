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
    project.participationStartDate = DateConverter.toReadableString(project.participationStartDate);
    project.participationEndDate != null ? project.participationEndDate = DateConverter.toReadableString(project.participationEndDate) : project.participationEndDate = null;
    project.projectEndDate = DateConverter.toReadableString(project.projectEndDate);
    project.projectStartDate = DateConverter.toReadableString(project.projectStartDate);
    return project
  }

  static todayString():String{
    return moment().format("DD-MM-YYYY")
  }
}
