import {Component, Inject, OnInit} from '@angular/core';
import {User} from "../../models/user";
import {UserService} from "../../services/user.service";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {Manager} from "../../models/manager";
import {MatTabChangeEvent} from "@angular/material/tabs";

@Component({
  selector: 'app-add-manager',
  templateUrl: './add-manager.component.html',
  styleUrls: ['./add-manager.component.scss']
})
export class AddManagerComponent implements OnInit {
  loaded: boolean = false;
  searchText: string = '';
  users: User[] = [];
  managers: Manager[] = [];
  currentPage: number = 1;
  pageSize: number = 5;
  projectId: string;
  currentTabIndex: number = 0;

  constructor(private userService: UserService,
              @Inject(MAT_DIALOG_DATA) public data: any,
              public dialogRef: MatDialogRef<AddManagerComponent>,) {
  }

  async ngOnInit(): Promise<void> {
    this.data.projectId != null ? this.projectId = this.data.projectId : this.projectId = null;

    //get all current project managers
    await this.userService.getAllProjectManagers(this.projectId).then(res => {
      if (res) {
        this.managers = res.filter(m => m.person.userRole !== "Boardmember");
      }

    })
    //get all users
    await this.userService.getAllUsers().then(users => {
      users.forEach(user => {
        if (user.userRole !== "Boardmember" && !this.managers.find(m => m.personId == user.id)) { //and this.projectManagers does not contain (user.id)
          this.users.push(user);
        }
      })
      this.users.sort((a, b) => a.firstName > b.firstName ? 1 : -1);
      this.managers.sort((a, b) => a.person.firstName > b.person.firstName ? 1 : -1);
      this.loaded = true;
    })

  }

  resetPage() {
    this.currentPage = 1;
  }

  async modManager(id: string, alreadyManager: boolean) {
    if (!alreadyManager) {
      await this.userService.makeManager(this.projectId, id).then(res => {
        if (res) {
          this.dialogRef.close(this.users.find(u => u.id == id).firstName)
        }
      })

    } else {
      await this.userService.removeManager(this.projectId, id).then(res => {
        if (res) {
          this.dialogRef.close(this.managers.find(m => m.personId == id).person.firstName)
        }
      })
    }

  }

  prevPage() {
    if (this.currentPage != 1) {
      this.currentPage--;
    }
  }

  nextPage() {
    if (this.currentTabIndex === 0) {
      if (this.currentPage != Math.ceil(this.users.length / this.pageSize)) {
        this.currentPage++;
      }
    } else {
      if (this.currentPage != Math.ceil(this.managers.length / this.pageSize)) {
        this.currentPage++;
      }
    }
  }

  close() {
    this.dialogRef.close()
  }

  changeTab($event: MatTabChangeEvent) {
    this.searchText = '';
    this.currentPage = 1;
    this.currentTabIndex = $event.index;
  }
}
