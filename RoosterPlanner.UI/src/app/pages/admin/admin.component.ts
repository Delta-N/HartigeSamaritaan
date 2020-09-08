import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.scss']
})
export class AdminComponent implements OnInit {
  data= [
    { name: "Restaurant Delft 2019", GUID:"1" },
    { name: "Restaurant Delft 2016", GUID:"2" },
    { name: "Restaurant Den Haag 2019", GUID:"3" },
    { name: "Restaurant Almere 2078", GUID:"4" }
    ];

  administrators=[
    {name: "Corné"},
    {name: "JW"},
    {name: "Yannick"},
    {name: "Joanne"},
  ]

  constructor() { }
  addProject() {
    window.alert("Deze functie moet nog geschreven worden...")
  }
  ngOnInit(): void {
  }

  addAdministrator() {
    window.alert("Deze functie moet nog geschreven worden...")
  }
}
