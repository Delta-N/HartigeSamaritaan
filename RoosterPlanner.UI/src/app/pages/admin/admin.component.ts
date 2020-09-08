import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.scss']
})
export class AdminComponent implements OnInit {
  data= [
    { name: "Restaurant Delft 2019" },
    { name: "Restaurant Delft 2016" },
    { name: "Restaurant Den Haag 2019" },
    { name: "Restaurant Almere 2078" }
    ];

  constructor() { }
  addProject() {
    window.alert("Deze functie moet nog geschreven worden...")
  }
  ngOnInit(): void {
  }

}
