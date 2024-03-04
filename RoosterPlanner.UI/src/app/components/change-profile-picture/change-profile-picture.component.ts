import { Component, Inject, OnInit } from '@angular/core';
import { User } from '../../models/user';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { UserService } from '../../services/user.service';
import { UploadService } from '../../services/upload.service';
import { ToastrService } from 'ngx-toastr';
import { TextInjectorService } from '../../services/text-injector.service';
import { Document } from '../../models/document';

@Component({
	selector: 'app-change-profile-picture',
	templateUrl: './change-profile-picture.component.html',
	standalone: true,
	styleUrls: ['./change-profile-picture.component.scss'],
})
export class ChangeProfilePictureComponent implements OnInit {
	user: User;
	files: FileList;

	constructor(
		@Inject(MAT_DIALOG_DATA) public data: any,
		public userService: UserService,
		public uploadService: UploadService,
		public toastr: ToastrService,
		public dialogRef: MatDialogRef<ChangeProfilePictureComponent>
	) {}

	ngOnInit(): void {
		this.user = this.data;
	}

	async edit() {
		if (this.files && this.files[0]) {
			const formData = new FormData();
			formData.append(this.files[0].name, this.files[0]);

			let uri: string | null = null;
			await this.uploadService.uploadProfilePicture(formData).then((url) => {
				if (url && url.path && url.path.trim().length > 0)
					uri = url.path.trim();
			});

			if (this.user.profilePicture !== null) {
				await this.uploadService
					.deleteIfExists(this.user.profilePicture.documentUri ?? '')
					.then();
				this.user.profilePicture.documentUri = uri;
				await this.uploadService
					.updateDocument(this.user.profilePicture)
					.then((res) => {
						if (res) this.user.profilePicture = res;
					});
			} else {
				const document = new Document();
				document.name = 'profilepicture';
				document.documentUri = uri;
				await this.uploadService.postDocument(document).then((res) => {
					if (res) this.user.profilePicture = res;
				});
			}

			await this.userService.updatePerson(this.user).then((res) => {
				if (res) {
					this.user = res;
					this.dialogRef.close(this.user);
				}
			});
		}
	}

	async remove() {
		if (this.user.profilePicture)
			await this.uploadService
				.removeDocument(this.user.profilePicture)
				.then((res) => {
					if (res) {
						this.toastr.success('Foto verwijderd');
						this.dialogRef.close('removed');
					}
				});
	}

	uploadPicture(files: FileList) {
		let correctExtention: boolean = true;
		const acceptedExtentions = TextInjectorService.acceptedImageExtentions;
		for (let i = 0; i < files.length; i++) {
			const extention: string = files[i].name.substring(
				files[i].name.lastIndexOf('.') + 1
			);
			const index: number = acceptedExtentions.indexOf(extention);
			if (index < 0) {
				this.toastr.warning('Controleer het formaat van de afbeelding');
				correctExtention = false;
			}
		}
		if (correctExtention) {
			this.files = files;
			this.edit();
		}
	}
}
