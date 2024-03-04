import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { Validator } from '../../helpers/validators';
import { User } from '../../models/user';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { UserService } from '../../services/user.service';
import { ToastrService } from 'ngx-toastr';
import { DateConverter } from '../../helpers/date-converter';
import { TextInjectorService } from '../../services/text-injector.service';
import { MaterialModule } from '../../modules/material/material.module';

@Component({
	selector: 'app-change-profile',
	templateUrl: './change-profile.component.html',
	standalone: true,
	imports: [MaterialModule],
	styleUrls: ['./change-profile.component.scss'],
})
export class ChangeProfileComponent implements OnInit {
	user: User;
	updateUser: User;
	checkoutForm;
	nationalities: string[] = TextInjectorService.nationalitiesDutch;
	countries: string[] = TextInjectorService.countries;
	level: string[] = TextInjectorService.level;
	nationalityControl: FormControl;
	languagueControl: FormControl;
	title: string = 'Profiel wijzigen';

	constructor(
		@Inject(MAT_DIALOG_DATA) public data: any,
		private formBuilder: FormBuilder,
		private userService: UserService,
		private toastr: ToastrService,
		public dialogRef: MatDialogRef<ChangeProfileComponent>
	) {
		this.user = data.user;
		this.title = data.title;
		this.nationalityControl = new FormControl('', Validators.required);
		this.languagueControl = new FormControl('', Validators.required);

		this.checkoutForm = this.formBuilder.group({
			id: this.user.id,
			firstName: [
				this.user.firstName !== null ? this.user.firstName : '',
				Validators.required,
			],
			lastName: [
				this.user.lastName !== null ? this.user.lastName : '',
				Validators.required,
			],
			dateOfBirth: [
				this.user.dateOfBirth !== null ? this.user.dateOfBirth : '',
				Validators.compose([Validators.required, Validator.date]),
			],
			streetAddress: [
				this.user.streetAddress !== null ? this.user.streetAddress : '',
				Validators.required,
			],
			postalCode: [
				this.user.postalCode !== null ? this.user.postalCode : '',
				Validator.postalCode,
			],
			city: [
				this.user.city !== null ? this.user.city : '',
				Validators.required,
			],
			phoneNumber: [
				this.user.phoneNumber !== null ? this.user.phoneNumber : '',
				Validator.phoneNumber,
			],
			nationality: this.nationalityControl,

			nativeLanguage: [
				this.user.nativeLanguage !== null ? this.user.nativeLanguage : '',
				Validators.required,
			],
			dutchProficiency: this.languagueControl,
			termsOfUseConsented: this.user.termsOfUseConsented,
		});
	}

	ngOnInit(): void {
		if (this.user.nationality !== null) {
			this.nationalityControl.setValue(
				this.nationalities.find((n) => n === this.user.nationality)
			);
		}

		if (this.user.dutchProficiency !== null) {
			this.languagueControl.setValue(
				this.level.find((n) => n === this.user.dutchProficiency)
			);
		}
	}

	saveProfile(value: User) {
		this.updateUser = value;
		if (this.checkoutForm.status === 'INVALID') {
			this.toastr.error('Niet alle velden zijn correct ingevuld');
		} else {
			const bd: Date = DateConverter.toDate(this.updateUser.dateOfBirth);
			const today: Date = new Date();

			if (bd > today) {
				this.toastr.error('Geboortedatum mag niet in de toekomst liggen');
			} else if (bd.getFullYear() - today.getFullYear() < -100) {
				this.toastr.error('Kom op zo oud ben je echt niet!');
			} else {
				this.userService.updateUser(this.updateUser).then((response) => {
					this.dialogRef.close(response);
				});
			}
		}
	}

	close() {
		this.dialogRef.close(null);
	}
}
