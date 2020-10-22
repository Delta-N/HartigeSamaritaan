import {Component, OnInit} from '@angular/core';
import {User} from "../../models/user";
import {UserService} from "../../services/user.service";
import {JwtHelper} from "../../helpers/jwt-helper";

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {
  user: User;
  age:number;
  loaded:boolean=false;

  constructor(private userService: UserService) {
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
    window.alert("Deze functie moet nog geschreven worden")
  }

  edit(GUID: any) {
    window.alert("Deze functie moet nog geschreven worden")
  }

  calculateAge(dateOfBirth:string): number {
    const toDate = (str) => {
      const [day, month, year] = str.split("-")
      return new Date(year, month - 1, day)
    }
    const today = new Date();
    const birthDate = toDate(dateOfBirth);
    let age = today.getFullYear() - birthDate.getFullYear();
    const m = today.getMonth() - birthDate.getMonth();

    if (m < 0 || (m === 0 && today.getDate() < birthDate.getDate())) {
      age--;
    }

    return age;
  }

}
