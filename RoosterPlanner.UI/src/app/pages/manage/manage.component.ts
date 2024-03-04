import { Component, OnInit } from '@angular/core';
import { Project } from '../../models/project';
import { UserService } from '../../services/user.service';
import { ToastrService } from 'ngx-toastr';
import { EmailService } from '../../services/email.service';
import { MatDialog } from '@angular/material/dialog';
import { EmailDialogComponent } from '../../components/email-dialog/email-dialog.component';
import {
	ConfirmDialogComponent,
	ConfirmDialogModel,
} from '../../components/confirm-dialog/confirm-dialog.component';

@Component({
	selector: 'app-manage',
	templateUrl: './manage.component.html',
	styleUrls: ['./manage.component.scss'],
})
export class ManageComponent implements OnInit {
	loaded: boolean = false;
	projects: Project[] = [];

	constructor(
		private userService: UserService,
		private toastr: ToastrService,
		private emailService: EmailService,
		public dialog: MatDialog
	) {}

	async ngOnInit(): Promise<void> {
		const userId: string = this.userService.getCurrentUserId();
		await this.userService.getProjectsManagedBy(userId).then((res) => {
			if (res != null) {
				res.forEach((m) => this.projects.push(m.project));
			}
			this.loaded = true;
		});
	}

	todo() {
		this.toastr.warning('Deze functie moet nog geschreven worden');
	}

	async sendEmail(id: string) {
		if (id) {
			const dialogRef = this.dialog.open(EmailDialogComponent, {
				width: '750px',
			});
			dialogRef.afterClosed().subscribe((result) => {
				if (result && result !== 'false') {
					this.toastr.warning('Berichten aan het versturen...');
					this.emailService.sendEmail(id, result).then((res) => {
						if (res) {
							this.toastr.success('Berichten verzonden');
						}
					});
				}
			});
		}
	}

	async sendSchedule(id: string) {
		if (id) {
			const message = 'Weet je zeker dat je het rooster wilt versturen?';
			const dialogData = new ConfirmDialogModel(
				'Bevestig e-mail',
				message,
				'ConfirmationInput',
				null
			);
			const dialogRef = this.dialog.open(ConfirmDialogComponent, {
				maxWidth: '400px',
				data: dialogData,
			});

			dialogRef.afterClosed().subscribe(async (dialogResult) => {
				if (dialogResult === true) {
					this.toastr.warning('Berichten aan het versturen...');
					await this.emailService.sendSchedule(id).then((res) => {
						if (res) {
							this.toastr.success('Rooster verzonden');
						}
					});
				}
			});
		}
	}
}
