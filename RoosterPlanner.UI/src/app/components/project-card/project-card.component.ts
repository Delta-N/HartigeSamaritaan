import { Component, OnInit, Input } from '@angular/core';
import { UserService } from '../../services/user.service';
import { ParticipationService } from '../../services/participation.service';
import { Participation } from '../../models/participation';
import { ToastrService } from 'ngx-toastr';
import {
	ConfirmDialogComponent,
	ConfirmDialogModel,
} from '../confirm-dialog/confirm-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import {
	faCalendarAlt,
	faFileAlt,
	faList,
} from '@fortawesome/free-solid-svg-icons';
import { faTimesCircle } from '@fortawesome/free-regular-svg-icons';
import { RouterLink } from '@angular/router';
import { MaterialModule } from '../../modules/material/material.module';

@Component({
	selector: 'app-project-card',
	templateUrl: './project-card.component.html',
	standalone: true,
	imports: [RouterLink, MaterialModule],
	styleUrls: ['./project-card.component.scss'],
})
export class ProjectCardComponent implements OnInit {
	calendarIcon = faCalendarAlt;
	listIcon = faList;
	signOut = faTimesCircle;
	detailsIcon = faFileAlt;
	isAdmin: boolean = false;

	constructor(
		private toastr: ToastrService,
		private userService: UserService,
		private participationService: ParticipationService,
		public dialog: MatDialog
	) {}

	@Input() participation: Participation;

	ngOnInit(): void {
		this.isAdmin = this.userService.userIsAdminFrontEnd();
	}

	removeParticipation(participation: Participation) {
		const message = 'Weet je zeker dat je wilt uitschrijven voor dit project?';
		const dialogData = new ConfirmDialogModel(
			'Bevestig uitschrijving',
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
				await this.participationService
					.deleteParticipation(participation)
					.then(async (response) => {
						if (response) {
							window.location.reload();
						}
					});
			}
		});
	}
}
