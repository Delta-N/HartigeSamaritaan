import { Component } from '@angular/core';
import {MatDialog} from "@angular/material/dialog";
import {AddProjectComponent} from "./components/add-project/add-project.component";


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  constructor(public dialog:MatDialog) {
  }
  title = 'Hartige Samaritaan';
  openDialog(){
    this.dialog.open(AddProjectComponent);
  }
}
