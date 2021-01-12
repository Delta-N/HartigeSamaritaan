import {Pipe, PipeTransform} from '@angular/core';
import {User} from "../models/user";
import {Task} from "../models/task";
import {DateConverter} from "./date-converter";
import {Manager} from "../models/manager";
import {Shift} from "../models/shift";
import {Schedule} from "../models/schedule";
import {CalendarEvent} from "angular-calendar";
import {Availability} from "../models/availability";
import {Project} from "../models/project";

@Pipe({name: 'userFilter'})
export class FilterPipe implements PipeTransform {

  transform(items: User[], searchText: string): any[] {
    if (!items) {
      return [];
    }
    if (!searchText) {
      return items;
    }
    searchText = searchText.toLowerCase()

    return items.filter(function (item) {
      return JSON.stringify(item.firstName + " " + item.lastName).toLowerCase().includes(searchText);
    });
  }
}

@Pipe({name: 'managerFilter'})
export class ManagerFilterPipe implements PipeTransform {

  transform(items: Manager[], searchText: string): any[] {
    if (!items) {
      return [];
    }
    if (!searchText) {
      return items;
    }
    searchText = searchText.toLowerCase()

    return items.filter(function (item) {
      return JSON.stringify(item.person.firstName + " " + item.person.lastName).toLowerCase().includes(searchText);
    });
  }
}

@Pipe({name: 'taskFilter'})
export class TaskFilterPipe implements PipeTransform {

  transform(items: Task[], searchText: string): any[] {
    if (!items) {
      return []
    }
    if (!searchText) {
      return items;
    }
    searchText = searchText.toLowerCase()

    return items.filter(function (item) {
      return JSON.stringify(item.name).toLowerCase().includes(searchText);
    });
  }
}

@Pipe({name: 'datePipe'})
export class DatePipe implements PipeTransform {
  transform(value: Date): string {
    return DateConverter.dateToString(value);
  }
}

@Pipe({name: 'scheduledPipe'})
export class ScheduledPipe implements PipeTransform {
  transform(value: Shift): number {
    let result: number = 0;
    if (value.availabilities && value.availabilities.length > 0) {
      value.availabilities.forEach(a => {
        if (a.type === 3)
          result += 1;
      })
    }
    return result;
  }
}

@Pipe({name: 'agePipe'})
export class AgePipe implements PipeTransform {
  transform(value: number): number {
    if (value) {
      const today = new Date();
      const birthDate = DateConverter.toDate(value);
      let age = today.getFullYear() - birthDate.getFullYear();
      const m = today.getMonth() - birthDate.getMonth();

      if (m < 0 || (m === 0 && today.getDate() < birthDate.getDate())) {
        age--;
      }
      return age;
    }
  }
}

@Pipe({name: 'scheduledCount'})
export class ScheduledCount implements PipeTransform {
  transform(value: Schedule[]): number {
    let result: number = 0;
    if (value) {
      value.forEach(v => {
        if (v.scheduledThisDay)
          result++
      })
    }
    return result;
  }
}

@Pipe({name: 'checkboxFilter'})
export class CheckboxFilter implements PipeTransform {
  transform(listOfTasks: Task[], id: string): boolean {
    let result = listOfTasks.find(t => t.id === id)
    return !result;
  }
}

@Pipe({name: 'calendarTooltip'})
export class CalendarTooltip implements PipeTransform {
  transform(listOfTasks: Task[], title: string): string {
    let result = listOfTasks.find(t => t.name === title)
    return result.description;
  }
}

@Pipe({name: 'planTooltip'})
export class PlanTooltip implements PipeTransform {
  transform(listOfShifts: Shift[], event: CalendarEvent): string {
    let shift = listOfShifts.find(s => s.id == event.id)

    if ((event.end.getTime() - event.start.getTime()) / 3600000 <= 2) {
      let result: string = shift.participantsRequired + " Nodig "

      let availableNumber = shift.availabilities ? shift.availabilities.filter(a => a.type === 2).length : 0;
      result += availableNumber + " Beschikbaar ";

      let scheduledNumber = shift.availabilities ? shift.availabilities.filter(a => a.type === 3).length : 0
      result += scheduledNumber + " Ingeroosterd ";
      return result;
    }
  }
}

@Pipe({name: 'availabilityPipe'})
export class AvailabilityPipe implements PipeTransform {
  transform(listOfAvailabilities: Availability[]): string {
    let result: string = "Is vandaag beschikbaar voor: \n";
    listOfAvailabilities.forEach(a => {
      if (a.preference === true && a.type === 2)
        result += "☆ "
      if (a.type === 2)
        result += a.shift.task.name + " " + a.shift.startTime + "-" + a.shift.endTime + "\n"

    })
    let found: boolean = false
    let append: string = "Is vandaag ingeroosterd op: \n";

    listOfAvailabilities.forEach(a => {

      if (a.type === 3) {
        found = true
        append += a.shift.task.name + " " + a.shift.startTime + " - " + a.shift.endTime + "\n"
      }


    })
    if (found) {
      result += "\n";
      result += append
    }

    if (listOfAvailabilities[0] && listOfAvailabilities[0].participation && listOfAvailabilities[0].participation.remark)
      result += "\n Werkt graag samen met:\n" + listOfAvailabilities[0].participation.remark;

    return result;

  }

}

@Pipe({name: 'colorPipe'})
export class ColorPipe implements PipeTransform {
  transform(color: string): string {
    switch (color) {
      case "Red":
        return "Rood";
      case "Blue":
        return "Blauw";
      case "Yellow":
        return "Geel";
      case "Green":
        return "Groen";
      case "Orange":
        return "Oranje";
      case "Pink":
        return "Roze";
      case "Rood":
        return "Red";
      case "Blauw":
        return "Blue";
      case "Geel":
        return "Yellow";
      case "Groen":
        return "Green";
      case "Oranje":
        return "Orange";
      case "Roze":
        return "Pink";
      default:
        return "Gray";
    }
  }
}

@Pipe({name: 'projectClosed'})
export class ProjectClosedPipe implements PipeTransform {
  transform(project: Project): boolean {
    return project.closed || new Date(project.projectEndDate) < new Date();

  }
}

