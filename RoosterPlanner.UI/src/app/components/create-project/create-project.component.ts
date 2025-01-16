import { Component, Inject, OnInit } from '@angular/core';
import { Project } from '../../models/project';
import { FormBuilder, Validators } from '@angular/forms';
import { ProjectService } from '../../services/project.service';
import { Validator } from '../../helpers/validators';
import { ToastrService } from 'ngx-toastr';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { DateConverter } from '../../helpers/date-converter';
import { UploadService } from '../../services/upload.service';
import { Document } from '../../models/document';
import { TextInjectorService } from '../../services/text-injector.service';

@Component({
	selector: 'app-create-project',
	templateUrl: './create-project.component.html',
	styleUrls: ['./create-project.component.scss'],
})
export class CreateProjectComponent implements OnInit {
	project: Project = new Project();
	updatedProject: Project;
	checkoutForm;
	title = 'Project toevoegen';
	files: FileList;

	constructor(
		private formBuilder: FormBuilder,
		private projectService: ProjectService,
		private toastr: ToastrService,
		public dialogRef: MatDialogRef<CreateProjectComponent>,
		@Inject(MAT_DIALOG_DATA) public data: any,
		private uploadService: UploadService
	) {
		const today = DateConverter.todayString();
		if (!this.data.createProject) {
			this.project = this.data.project;
		}
		this.title = this.data.title;
		this.checkoutForm = this.formBuilder.group({
			id: this.project.id != null ? this.project.id : '',
			name: [
				this.project.name != null ? this.project.name : '',
				Validators.required,
			],
			address: [
				this.project.address != null ? this.project.address : '',
				Validators.required,
			],
			city: [
				this.project.city != null ? this.project.city : '',
				Validators.required,
			],
			description: [
				this.project.description != null ? this.project.description : '',
				Validators.required,
			],
			participationStartDate: [
				this.project.participationStartDate != null
					? this.project.participationStartDate
					: '',
				Validator.date,
			],
			participationEndDate: [
				this.project.participationEndDate != null
					? this.project.participationEndDate
					: '',
				[Validator.dateOrNull],
			],
			projectStartDate: [
				this.project.projectStartDate != null
					? this.project.projectStartDate
					: today,
				Validator.date,
			],
			projectEndDate: [
				this.project.projectEndDate != null ? this.project.projectEndDate : '',
				Validator.date,
			],
			pictureUri:
				this.project.pictureUri != null ? this.project.pictureUri : null,
			websiteUrl:
				this.project.websiteUrl != null ? this.project.websiteUrl : '',
			contactAdres: [
				this.project.contactAdres != null ? this.project.contactAdres : '',
				Validator.email,
			],
		});
	}

	ngOnInit(): void {}

	async saveProject(value: Project) {
		this.updatedProject = value;
		if (this.checkoutForm.status === 'INVALID') {
			this.toastr.error('Niet alle velden zijn correct ingevuld');
		} else {
			const prSdate = DateConverter.toDate(
				this.updatedProject.projectStartDate
			);
			const prEdate = DateConverter.toDate(this.updatedProject.projectEndDate);
			const parSdate = DateConverter.toDate(
				this.updatedProject.participationStartDate
			);
			//alleen participation end date is optioneel
			let parEdate = null;
			this.updatedProject.participationEndDate != null
				? (parEdate = DateConverter.toDate(
						this.updatedProject.participationEndDate
				  ))
				: null;

			if (prEdate < prSdate) {
				this.toastr.error(
					'Project einddatum mag niet voor project startdatum liggen'
				);
				return;
			}
			if (parSdate < prSdate || parSdate > prEdate) {
				this.toastr.error(
					'Participatie startdatum moet tussen projectstart en project einddatum liggen'
				);
				return;
			}
			if (parEdate != null && parEdate < parSdate) {
				this.toastr.error(
					'Participatie eindatum mag niet voor de participatie startdatum liggen'
				);
				return;
			}
			if (parEdate != null && parEdate > (prEdate || parEdate < prSdate)) {
				this.toastr.error(
					'Participatie einddatum moet tussen projectstart en project einddatum liggen'
				);
				return;
			}

			if (this.files && this.files[0]) {
				const formData = new FormData();
				formData.append(this.files[0].name, this.files[0]);

				let uri: string = null;
				await this.uploadService.uploadProjectPicture(formData).then((url) => {
					if (url && url.path && url.path.trim().length > 0)
						uri = url.path.trim();
				});

				if (this.project.pictureUri != null) {
					await this.uploadService
						.deleteIfExists(this.project.pictureUri.documentUri)
						.then();
					this.project.pictureUri.documentUri = uri;
					await this.uploadService
						.updateDocument(this.project.pictureUri)
						.then((res) => {
							if (res) this.updatedProject.pictureUri = res;
						});
				} else {
					const document = new Document();
					document.name = 'projectpicture';
					document.documentUri = uri;
					await this.uploadService.postDocument(document).then((res) => {
						if (res) this.updatedProject.pictureUri = res;
					});
				}
			}

			//create new project
			if (this.data.createProject) {
				await this.projectService
					.postProject(this.updatedProject)
					.then(async (response) => (this.updatedProject = response));
				this.dialogRef.close(this.updatedProject);
			}
			//edit project
			else {
				this.updatedProject.lastEditBy = this.project.lastEditBy;
				this.updatedProject.lastEditDate = this.project.lastEditDate;
				this.updatedProject.rowVersion = this.project.rowVersion;
				await this.projectService
					.updateProject(this.updatedProject)
					.then(async (response) => {
						if (response) {
							this.project = response;
							this.dialogRef.close(this.updatedProject);
						} else this.dialogRef.close('false');
					});
			}
		}
	}

	uploadPicture(files: FileList) {
		let correctExtention = true;
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
		if (correctExtention) this.files = files;
	}
}
