import {Pipe, PipeTransform} from '@angular/core';
import {User} from "../models/user";
import {Task} from "../models/task";

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

@Pipe({name: 'taskFilter'})
export class TaskFilterPipe implements PipeTransform {

  transform(items: Task[], searchText: string): any[] {
    if (!items) {
      return [];
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
