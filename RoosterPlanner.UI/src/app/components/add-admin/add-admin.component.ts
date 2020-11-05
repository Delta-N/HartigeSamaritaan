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
  displayUsers: User[] = []
  loaded: boolean;
  searchText: string = '';
  title: string;


  constructor(private userService: UserService, @Inject(MAT_DIALOG_DATA) public data: any, public dialogRef: MatDialogRef<AddAdminComponent>) {
  }

  async ngOnInit(): Promise<void> {
    if (this.data.administrators != null) {
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
    this.fillDisplayUsers();
    this.data.addAdminType ? this.title = 'toevoegen' : this.title = 'verwijderen';
  }

  fillDisplayUsers() {
    this.displayUsers = [];
    for (let i = this.pageSize * (this.currentPage - 1); i < this.pageSize * this.currentPage; i++) {
      this.displayUsers.push(this.users[i])
    }
    this.displayUsers = this.displayUsers.filter(f => {
      return f != null;
    })

  }

  nextPage() {

    if (this.currentPage != Math.ceil(this.users.length / this.pageSize)) {
      this.currentPage++;
      this.fillDisplayUsers();
    }
  }

  prevPage() {
    if (this.currentPage != 1) {
      this.currentPage--;
      this.fillDisplayUsers();
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
  close(){
    this.dialogRef.close()
  }

}
