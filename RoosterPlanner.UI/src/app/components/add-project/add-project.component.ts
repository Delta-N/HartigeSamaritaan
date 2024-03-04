import { Component, OnInit, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatCheckboxChange } from '@angular/material/checkbox';
import { Project } from '../../models/project';
import { MaterialModule } from '../../modules/material/material.module';

@Component({
	selector: 'app-add-project',
	templateUrl: './add-project.component.html',
	standalone: true,
	imports: [MaterialModule],

	styleUrls: ['./add-project.component.scss'],
})
export class AddProjectComponent implements OnInit {
	selectedProjects: Project[] = [];

	constructor(
		@Inject(MAT_DIALOG_DATA) public data: any,
		public dialogRef: MatDialogRef<AddProjectComponent>
	) {}

	ngOnInit(): void {}

	OnChange(event: MatCheckboxChange) {
		const project: Project = this.data.find((p) => p.id === event.source.id);
		if (event.checked) {
			this.addToArray(project);
		}
		if (!event.checked) {
			this.removeFromArray(project);
		}
	}

	addToArray(project: Project) {
		let present = false;
		for (let i = 0; i < this.selectedProjects.length; i++) {
			if (this.selectedProjects[i].id === project.id) {
				present = true;
			}
		}
		if (!present) {
			this.selectedProjects.push(project);
		}
	}

	removeFromArray(project: Project) {
		const index = this.selectedProjects.indexOf(project);
		this.selectedProjects.splice(index, 1);
	}

	addParticipations() {
		this.dialogRef.close(this.selectedProjects);
	}
}
