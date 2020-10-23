import {Component, OnInit} from '@angular/core';
import {User} from "../../models/user";
import {UserService} from "../../services/user.service";
import {JwtHelper} from "../../helpers/jwt-helper";
import {AuthenticationService} from "../../services/authentication.service";
import {DateConverter} from "../../helpers/date-converter";

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {
  user: User;
  age:number;
  loaded:boolean=false;

  constructor(private userService: UserService, private authenticationService: AuthenticationService) {
  }

  async ngOnInit(): Promise<void> {
    const idToken = JwtHelper.decodeToken(sessionStorage.getItem('msal.idtoken'));
    await this.userService.getUser(idToken.oid).then(x=>{
      this.user=x;
      this.age=this.calculateAge(this.user.dateOfBirth);
      this.loaded=true;
    });

  }

  logout() {
    this.authenticationService.logout();
  }

  edit(GUID: any) {
    window.alert("Deze functie moet nog geschreven worden")
  }

  calculateAge(dateOfBirth:string): number {
    if(dateOfBirth==null)
      return 0;

    const today = new Date();
    const birthDate = DateConverter.toDate(dateOfBirth);
    let age = today.getFullYear() - birthDate.getFullYear();
    const m = today.getMonth() - birthDate.getMonth();

    if (m < 0 || (m === 0 && today.getDate() < birthDate.getDate())) {
      age--;
    }

    return age;
  }

}
