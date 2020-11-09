import {Pipe, PipeTransform} from '@angular/core';
import {User} from "../models/user";

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
