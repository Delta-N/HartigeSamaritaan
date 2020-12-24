import {Component, OnInit} from '@angular/core';
import {User} from "../../models/user";
import {UserService} from "../../services/user.service";
import {JwtHelper} from "../../helpers/jwt-helper";
import {MsalService} from "../../msal";
import {DateConverter} from "../../helpers/date-converter";
import {MatDialog} from '@angular/material/dialog';
import {ChangeProfileComponent} from "../../components/change-profile/change-profile.component";
import {ActivatedRoute, ParamMap} from "@angular/router";
import {BreadcrumbService} from "../../services/breadcrumb.service";


@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {
  user: User;
  age: any;
  loaded: boolean = false;
  guid: string;

  constructor(private route: ActivatedRoute,
              private userService: UserService,
              private authenticationService: MsalService,
              private dialog: MatDialog,) {}

  async ngOnInit(): Promise<void> {
    this.route.paramMap.subscribe((params: ParamMap) => {
      this.guid = params.get('id');
    });
    if (this.guid == null) {
      const idToken = JwtHelper.decodeToken(sessionStorage.getItem('msal.idtoken'));
      this.guid = idToken.oid;
    }
    this.loadUserProfile().then();
  }

  async loadUserProfile() {

    await this.userService.getUser(this.guid).then(x => {
      if (x.id != "") {
        this.user = x;
        this.age = DateConverter.calculateAge(this.user.dateOfBirth);
        this.loaded = true;
      } else {
        this.loaded = false;
      }
    });
  }

  logout() {
    this.authenticationService.logout();
  }

  edit() {
    const dialogRef = this.dialog.open(ChangeProfileComponent, {
      width: '500px',
      data: this.user
    });
    dialogRef.disableClose=true;
    dialogRef.afterClosed().subscribe(result => {
      if (result != null) {
        this.user = result;
        this.age = DateConverter.calculateAge(this.user.dateOfBirth)
      }
    })
  }
}
