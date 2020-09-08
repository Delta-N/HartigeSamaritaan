import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

  logout() {
    window.alert("Deze functie moet nog geschreven worden")
  }
  user={name:"Corné van den Brink", age:24, city: "Delft"}


  edit(GUID: any) {
    window.alert("Deze functie moet nog geschreven worden")
  }
}
