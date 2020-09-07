import { Component, OnInit } from '@angular/core';
@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }
  projects= [
    { name: "Restaurant Delft 2019" },
    { name: "Restaurant Delft 2016" },
    { name: "Restaurant Den Haag 2019" },

  ];

}
