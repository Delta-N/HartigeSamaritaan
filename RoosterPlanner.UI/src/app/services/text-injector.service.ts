import {Injectable} from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class TextInjectorService {
  public static calenderSelectionOptions: string[] = ['Elke dag', 'Elke maandag', 'Elke dinsdag', 'Elke woensdag', 'Elke donderdag', 'Elke vrijdag', 'Elke zaterdag', 'Elke zondag', 'Verwijder selectie'];
  public static shiftTableColumnNames: string[] = ['Taak', 'Datum', 'Vanaf', 'Tot', '#Benodigde vrijwilligers'];

  constructor() {
  }
}
