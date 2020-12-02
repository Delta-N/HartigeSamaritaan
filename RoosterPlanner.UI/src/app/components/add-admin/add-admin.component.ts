import {Component, OnInit, Inject} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {User} from "../../models/user";
import {UserService} from "../../services/user.service";

@Component({
  selector: 'app-add-admin',
  templateUrl: './add-admin.component.html',
  styleUrls: ['./add-admin.component.scss']
})
export class AddAdminComponent implements OnInit {
  users: User[] = [];
  checkoutForm;
  pageSize: number = 5;
  currentPage: number = 1;
  loaded: boolean;
  searchText: string = '';
  title: string;


  constructor(private userService: UserService,
              @Inject(MAT_DIALOG_DATA) public data: any,
              public dialogRef: MatDialogRef<AddAdminComponent>) {
  }

  async ngOnInit(): Promise<void> {
    if (this.data.administrators != null && !this.data.addAdminType) {
      this.users = this.data.administrators;
      this.loaded = true;
    } else {
      await this.userService.getAllUsers().then(users => {
        users.forEach(user => {
          if (this.data.addAdminType) {
            if (user.userRole !== "Boardmember") {
              this.users.push(user);
            }
          }
        })
        this.loaded = true;
      })
    }

    this.users.sort((a, b) => a.firstName > b.firstName ? 1 : -1);

    this.data.addAdminType ? this.title = 'toevoegen' : this.title = 'verwijderen';
  }

  nextPage() {

    if (this.currentPage != Math.ceil(this.users.length / this.pageSize)) {
      this.currentPage++;
    }
  }

  prevPage() {
    if (this.currentPage != 1) {
      this.currentPage--;
    }
  }

  modAdmin(GUID: string) {
    if (this.data.addAdminType) {
      this.userService.makeAdmin(GUID).then();
    } else {
      this.userService.removeAdmin(GUID).then();
    }
  }

  resetPage() {
    this.currentPage = 1;
  }

  close() {
    this.dialogRef.close()
  }

}
