import { Component, Inject, OnInit } from '@angular/core';
import { User } from '../../models/user';
import { UserService } from '../../services/user.service';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Manager } from '../../models/manager';
import { MatTabChangeEvent } from '@angular/material/tabs';
import { ToastrService } from 'ngx-toastr';

@Component({
	selector: 'app-add-manager',
	templateUrl: './add-manager.component.html',
	styleUrls: ['./add-manager.component.scss'],
})
export class AddManagerComponent implements OnInit {
	loaded = false;
	searchText = '';
	users: User[] = [];
	managers: Manager[] = [];
	currentPage = 1;
	pageSize = 5;
	projectId: string;
	currentTabIndex = 0;

	addedManagers: string[] = [];
	removedManagers: string[] = [];

	constructor(
		private userService: UserService,
		@Inject(MAT_DIALOG_DATA) public data: any,
		public dialogRef: MatDialogRef<AddManagerComponent>,
		private toastr: ToastrService
	) {}

	async ngOnInit(): Promise<void> {
		this.data.projectId != null
			? (this.projectId = this.data.projectId)
			: (this.projectId = null);

		//get all current project managers
		await this.userService.getAllProjectManagers(this.projectId).then((res) => {
			if (res) {
				this.managers = res;
			}
		});
		//get all users
		await this.userService.getAllUsers().then((users) => {
			users.forEach((user) => {
				if (!this.managers.find((m) => m.personId == user.id)) {
					this.users.push(user);
				}
			});

			this.users.sort((a, b) => (a.firstName > b.firstName ? 1 : -1));
			this.managers.sort((a, b) =>
				a.person.firstName > b.person.firstName ? 1 : -1
			);
			this.loaded = true;
		});
	}

	resetPage() {
		this.currentPage = 1;
	}

	async modManager(id: string, alreadyManager: boolean) {
		if (!alreadyManager) {
			const index = this.addedManagers.indexOf(id);
			if (index > -1) this.addedManagers.splice(index, 1);
			else this.addedManagers.push(id);
		} else {
			const index = this.removedManagers.indexOf(id);
			if (index > -1) this.removedManagers.splice(index, 1);
			else this.removedManagers.push(id);
		}
		this.changeBackground();
	}

	changeBackground() {
		this.users.forEach((u) => {
			const element = document.getElementById(u.id);
			const checkElement = document.getElementById('check' + u.id);
			if (element) {
				const index = this.addedManagers.find((m) => m === u.id);
				if (index) {
					element.style.background = 'whitesmoke';
					checkElement.hidden = false;
				} else {
					element.style.background = 'white';
					checkElement.hidden = true;
				}
			}
		});

		this.managers.forEach((u) => {
			const element = document.getElementById(u.personId);
			const closeElement = document.getElementById('check' + u.id);
			if (element) {
				const index = this.removedManagers.find((m) => m === u.personId);
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

	prevPage() {
		if (this.currentPage != 1) {
			this.currentPage--;
		}
		setTimeout(() => {
			this.changeBackground();
		}, 100);
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
		setTimeout(() => {
			this.changeBackground();
		}, 100);
	}

	close() {
		this.dialogRef.close();
	}

	changeTab($event: MatTabChangeEvent) {
		this.searchText = '';
		this.currentPage = 1;
		this.currentTabIndex = $event.index;
	}

	async send() {
		for (const am of this.addedManagers) {
			this.userService.makeManager(this.projectId, am).then((res) => {
				if (res)
					this.toastr.success(
						this.users.find((u) => u.id === am).firstName +
							' is succesvol toegevoegd'
					);
			});
		}
		for (const rm of this.removedManagers) {
			this.userService.removeManager(this.projectId, rm).then((res) => {
				if (res)
					this.toastr.success(
						this.managers.find((u) => u.personId === rm).person.firstName +
							' is succesvol verwijderd'
					);
			});
		}
		this.dialogRef.close();
	}
}
