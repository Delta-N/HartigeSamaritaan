import { Component, OnInit } from '@angular/core';
import { ProjectService } from '../../services/project.service';
import { Project } from '../../models/project';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { CreateProjectComponent } from '../../components/create-project/create-project.component';
import { User } from '../../models/user';
import { UserService } from '../../services/user.service';
import { AddAdminComponent } from '../../components/add-admin/add-admin.component';
import { ToastrService } from 'ngx-toastr';
import { UploadPrivacyPolicyComponent } from '../../components/upload-privacy-policy/upload-privacy-policy.component';
import { faPlusCircle, faEdit } from '@fortawesome/free-solid-svg-icons';

@Component({
	selector: 'app-admin',
	templateUrl: './admin.component.html',
	styleUrls: ['./admin.component.scss'],
})
export class AdminComponent implements OnInit {
	circleIcon = faPlusCircle;
	editIcon = faEdit;

	projects: Project[] = [];
	loaded: boolean = false;
	administrators: User[] = [];
	itemsPerCard: number = 5;

	projectCardStyle = 'card';
	projectsElementHeight: number;
	adminCardStyle = 'card';
	adminElementHeight: number;

	reasonableMaxInteger: number = 10000; //aanpassen na 10k projecten/admins ;)
	projectExpandbtnDisabled: boolean = true;
	adminExpandbtnDisabled: boolean = true;

	constructor(
		public dialog: MatDialog,
		private projectService: ProjectService,
		private router: Router,
		private userService: UserService,
		private toastr: ToastrService
	) {}

	ngOnInit(): void {
		this.getProjects(0, this.itemsPerCard).then();
		this.getAdministrators(0, this.itemsPerCard).then(
			() => (this.loaded = true)
		);
	}

	async getProjects(offset: number, pageSize: number) {
		await this.projectService
			.getAllProjects(offset, pageSize)
			.then(async (x) => {
				this.projects = x;
				if (this.projects.length >= 5) {
					this.projectExpandbtnDisabled = false;
				}
				this.projects.sort((a, b) =>
					a.participationStartDate < b.participationStartDate ? 1 : -1
				);
			});
	}

	async getAdministrators(offset: number, pageSize: number) {
		await this.userService.getAdministrators(offset, pageSize).then((res) => {
			this.administrators = res;
			if (this.administrators.length >= 5) {
				this.adminExpandbtnDisabled = false;
			}
			this.administrators.sort((a, b) => (a.firstName > b.firstName ? 1 : -1));
		});
	}

	addProject() {
		const dialogRef = this.dialog.open(CreateProjectComponent, {
			width: '500px',
			data: {
				createProject: true,
				title: 'Project toevoegen',
			},
		});
		dialogRef.disableClose = true;
		dialogRef.afterClosed().subscribe((result) => {
			if (result !== 'false') {
				setTimeout(() => {
					this.getProjects(0, this.itemsPerCard).then(() => {
						if (result != null) {
							this.toastr.success(
								result.name + ' is toegevoegd als nieuw project'
							);
						}
					});
				}, 1000);
			}
		});
	}

	async modAdmin() {
		const dialogRef = this.dialog.open(AddAdminComponent, {
			width: '500px',
		});

		dialogRef.disableClose = true;
		dialogRef.afterClosed().subscribe((_) => {
			setTimeout(() => {
				this.getAdministrators(0, this.itemsPerCard).then(
					() => (this.loaded = true)
				);
			}, 250);
		});
	}

	expandProjectCard() {
		const dataCardElement = document.getElementById('dataCard');
		const adminCardElement = document.getElementById('adminCard');
		const element = document.getElementById('projectIcon');
		if (element) {
			if (this.projectCardStyle === 'expanded-card')
				element.innerText = 'zoom_out_map';
			else element.innerText = 'fullscreen_exit';
		}
		if (this.projectCardStyle == 'expanded-card') {
			if (adminCardElement) adminCardElement.hidden = false;
			if (dataCardElement) dataCardElement.hidden = false;
			this.projectCardStyle = 'card';
			this.itemsPerCard = 5;
			this.projects = this.projects.slice(0, this.itemsPerCard);
		} else {
			if (adminCardElement) adminCardElement.hidden = true;
			if (dataCardElement) dataCardElement.hidden = true;
			this.projectCardStyle = 'expanded-card';
			this.itemsPerCard = this.reasonableMaxInteger;
			this.getProjects(0, this.itemsPerCard).then(() => {
				this.projectsElementHeight = this.projects.length * 48;
			});
		}
	}

	expandAdminCard() {
		const projectCardElement = document.getElementById('projectCard');
		const dataCardElement = document.getElementById('dataCard');
		const element = document.getElementById('adminIcon');
		if (element) {
			if (this.adminCardStyle === 'expanded-card')
				element.innerText = 'zoom_out_map';
			else element.innerText = 'fullscreen_exit';
		}

		if (this.adminCardStyle == 'expanded-card') {
			if (projectCardElement) projectCardElement.hidden = false;
			if (dataCardElement) dataCardElement.hidden = false;

			this.adminCardStyle = 'card';
			this.itemsPerCard = 5;
			this.administrators = this.administrators.slice(0, this.itemsPerCard);
		} else {
			if (projectCardElement) projectCardElement.hidden = true;
			if (dataCardElement) dataCardElement.hidden = true;
			this.adminCardStyle = 'expanded-card';
			this.itemsPerCard = this.reasonableMaxInteger;
			this.getAdministrators(0, this.itemsPerCard).then(() => {
				this.adminElementHeight = this.administrators.length * 48;
			});
		}
	}

	uploadPP() {
		const dialogRef = this.dialog.open(UploadPrivacyPolicyComponent, {
			width: '400px',
		});
		dialogRef.disableClose = true;
		dialogRef.afterClosed().subscribe((result) => {
			if (result && result !== 'false') {
				this.toastr.success('De privacy policy is gewijzigd');
			}
		});
	}
}
