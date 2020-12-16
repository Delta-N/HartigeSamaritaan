import {Pipe, PipeTransform} from '@angular/core';
import {User} from "../models/user";
import {Task} from "../models/task";
import {DateConverter} from "./date-converter";
import {Manager} from "../models/manager";
import {Shift} from "../models/shift";

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
    console.log("hier")
    let result: number = 0;
    if (value.availabilities && value.availabilities.length > 0) {
      value.availabilities.forEach(a => {
        if (a.type === 3)
          result+=1;
      })
    }
    return result;
  }
}
