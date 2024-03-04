import { Component, Input, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { AddProjectComponent } from '../../components/add-project/add-project.component';
import { Project } from '../../models/project';
import { ProjectService } from '../../services/project.service';
import { Participation } from '../../models/participation';
import { UserService } from '../../services/user.service';
import { User } from '../../models/user';
import { ParticipationService } from '../../services/participation.service';
import { ToastrService } from 'ngx-toastr';
import { EntityHelper } from '../../helpers/entity-helper';
import { ChangeProfileComponent } from '../../components/change-profile/change-profile.component';
import { JwtHelper } from '../../helpers/jwt-helper';
import { faHome, faPlusCircle } from '@fortawesome/free-solid-svg-icons';
import { MatProgressSpinner } from '@angular/material/progress-spinner';
import { MaterialModule } from '../../modules/material/material.module';
import { ProjectCardComponent } from '../../components/project-card/project-card.component';

@Component({
	selector: 'app-home',
	templateUrl: './home.component.html',
	standalone: true,
	imports: [MaterialModule, ProjectCardComponent],
	styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit {
	homeIcon = faHome;
	circleIcon = faPlusCircle;
	loaded: boolean = false;
	participations: Participation[] | null = [];
	@Input() user: User;
	selectedProjects: Project[];

	constructor(
		public dialog: MatDialog,
		private projectService: ProjectService,
		private userService: UserService,
		private participationService: ParticipationService,
		private toastr: ToastrService
	) {}

	async ngOnInit(): Promise<void> {
		const idToken = JwtHelper.decodeToken(
			sessionStorage.getItem('msal.idtoken')
		);
		await this.userService.getUser(idToken.oid).then(async (res) => {
			if (res) {
				this.user = res;
				this.getParticipations().then(() => (this.loaded = true));
			}
		});
	}

	async getParticipations() {
		await this.participationService
			.getParticipations(this.user.id)
			.then((response) => {
				this.participations = response;
				this.participations?.sort((a, b) =>
					a.project.name.toLowerCase() > b.project.name.toLowerCase() ? 1 : -1
				);
			});
	}

	async addParticipation() {
		let projects: Project[] = [];
		let userCheckedProfile: boolean = false;
		this.toastr.warning('Controleer je profiel en vul deze eventueel aan.');

		const dialogRef = this.dialog.open(ChangeProfileComponent, {
			width: '500px',
			data: {
				user: this.user,
				title: 'Update je profiel voordat je je aanmeldt',
			},
		});
		dialogRef.disableClose = true;

		dialogRef.afterClosed().subscribe(async (result) => {
			if (result != null) {
				userCheckedProfile = true;
			}

			if (userCheckedProfile) {
				await this.projectService.getActiveProjects().then(async (response) => {
					projects = response;
					projects.forEach((pro) => {
						this.participations?.forEach((par) => {
							if (pro.id == par.project.id) {
								projects = projects.filter((obj) => obj !== pro);
							}
						});
					});
					let dialogRef: MatDialogRef<any> | null = null;
					if (projects.length > 0) {
						dialogRef = this.dialog.open(AddProjectComponent, {
							data: projects,
							width: '350px',
						});
					} else {
						this.toastr.warning('Geen (nieuwe) projecten gevonden.');
					}

					if (dialogRef) {
						dialogRef.disableClose = true;
						dialogRef.afterClosed().subscribe(async (result) => {
							if (result !== 'false') {
								this.selectedProjects = result;
								for (const project of this.selectedProjects) {
									const participation: Participation = new Participation();
									participation.id = EntityHelper.returnEmptyGuid();
									participation.person = this.user;
									participation.project = project;
									await this.participationService
										.postParticipation(participation)
										.then((res) => {
											if (res) {
												this.participations?.push(res);
												this.toastr.success(
													'Aangemeld voor project: ' + res.project.name
												);
											}
										});
								}
							}
						});
					}
				});
			}
		});
	}
}
