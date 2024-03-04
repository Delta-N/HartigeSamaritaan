import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { User } from '../../models/user';
import { UserService } from '../../services/user.service';
import { MatTabChangeEvent } from '@angular/material/tabs';
import { ToastrService } from 'ngx-toastr';

@Component({
	selector: 'app-add-admin',
	templateUrl: './add-admin.component.html',
	styleUrls: ['./add-admin.component.scss'],
})
export class AddAdminComponent implements OnInit {
	loaded: boolean;
	searchText: string = '';
	users: User[] = [];
	admins: User[] = [];
	pageSize: number = 5;
	currentPage: number = 1;
	currentTabIndex: number = 0;

	addedAdmins: string[] = [];
	removedAdmins: string[] = [];

	constructor(
		private userService: UserService,
		public dialogRef: MatDialogRef<AddAdminComponent>,
		private toastr: ToastrService
	) {}

	async ngOnInit(): Promise<void> {
		await this.userService.getAllUsers().then((users) => {
			users.sort((a, b) => (a.firstName > b.firstName ? 1 : -1));
			users.forEach((user) => {
				if (user.userRole !== 'Boardmember') this.users.push(user);
				else this.admins.push(user);
			});
		});
		this.loaded = true;
	}

	nextPage() {
		if (this.currentTabIndex === 0) {
			if (this.currentPage !== Math.ceil(this.users.length / this.pageSize)) {
				this.currentPage++;
			}
		} else {
			if (this.currentPage !== Math.ceil(this.admins.length / this.pageSize)) {
				this.currentPage++;
			}
		}
		setTimeout(() => {
			this.changeBackground();
		}, 100);
	}

	prevPage() {
		if (this.currentPage !== 1) {
			this.currentPage--;
		}
		setTimeout(() => {
			this.changeBackground();
		}, 100);
	}

	resetPage() {
		this.currentPage = 1;
	}

	close() {
		this.dialogRef.close();
	}

	changeTab($event: MatTabChangeEvent) {
		this.searchText = '';
		this.currentPage = 1;
		this.currentTabIndex = $event.index;
	}

	async modAdmin(id: string, alreadyManager: boolean) {
		if (!alreadyManager) {
			const index = this.addedAdmins.indexOf(id);
			if (index > -1) this.addedAdmins.splice(index, 1);
			else this.addedAdmins.push(id);
		} else {
			const index = this.removedAdmins.indexOf(id);
			if (index > -1) this.removedAdmins.splice(index, 1);
			else this.removedAdmins.push(id);
		}
		this.changeBackground();
	}

	changeBackground() {
		this.users.forEach((u) => {
			const element = document.getElementById(u.id);
			const checkElement = document.getElementById('check' + u.id);
			if (element && checkElement) {
				const index = this.addedAdmins.find((m) => m === u.id);
				if (index) {
					element.style.background = 'whitesmoke';
					checkElement.hidden = false;
				} else {
					element.style.background = 'white';
					checkElement.hidden = true;
				}
			}
		});

		this.admins.forEach((u) => {
			const element = document.getElementById(u.id);
			const closeElement = document.getElementById('check' + u.id);
			if (element && closeElement) {
				const index = this.removedAdmins.find((m) => m === u.id);
				if (index) {
					element.style.background = 'whitesmoke';
					closeElement.hidden = false;
				} else {
					element.style.background = 'white';
					closeElement.hidden = true;
				}
			}
		});
	}

	async send() {
		for (const addedAdmin of this.addedAdmins) {
			this.userService.makeAdmin(addedAdmin).then((res) => {
				if (res)
					this.toastr.success(
						this.users.find((u) => u.id === addedAdmin)?.firstName +
							' is succesvol toegevoegd'
					);
			});
		}
		for (const removedAdmin of this.removedAdmins) {
			this.userService.removeAdmin(removedAdmin).then((res) => {
				if (res)
					this.toastr.success(
						this.admins.find((u) => u.id === removedAdmin)?.firstName +
							' is succesvol verwijderd'
					);
			});
		}
		this.dialogRef.close();
	}
}
