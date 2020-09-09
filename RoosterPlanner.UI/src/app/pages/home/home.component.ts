import { Component, OnInit } from '@angular/core';
import {MatDialog} from "@angular/material/dialog";
import {AddProjectComponent} from "../../components/add-project/add-project.component";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  constructor(public dialog:MatDialog) { }

  ngOnInit(): void {
  }
  projects= [
    { name: "Restaurant Delft 2019",GUID:1 },
    { name: "Restaurant Delft 2016",GUID:2 },
    { name: "Restaurant Den Haag 2019",GUID:3 },
  ];

  addProject() {
    let dialogRef = this.dialog.open(AddProjectComponent,{data: this.projects,panelClass:'custom-dialog-container'});

    dialogRef.afterClosed().subscribe(result=>{
      console.log(result)
    })
  }
}
