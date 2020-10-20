import {Component, OnInit} from '@angular/core';
import {MatDialog} from "@angular/material/dialog";
import {AddProjectComponent} from "./components/add-project/add-project.component";
import {MsalService, BroadcastService} from '@azure/msal-angular';
import {AuthenticationService} from './services/authentication.service';
import {UserService} from "./services/user.service";
import {User} from "./models/user";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'Hartige Samaritaan';
  isIframe = false;
  loggedIn = false;
  isAdmin = false;
  user: User = new User('');

  constructor(public dialog: MatDialog,
              private broadcastService: BroadcastService,
              private authService: MsalService,
              private authenticationService: AuthenticationService,
              private userService: UserService) {

  }

  openDialog() {
    this.dialog.open(AddProjectComponent);
  }


  ngOnInit() {
    this.isIframe = window !== window.parent && !window.opener;
    this.checkAccount();
  }

  // other methods

  async checkAccount() {
    this.loggedIn = this.authenticationService.checkAccount();
    this.isAdmin = this.userService.userIsAdminFrontEnd();
  }

  async testButton() {
  }

  logout() {
    this.authenticationService.logout()
  }

  login() {
    this.authenticationService.logout()
  }
}

