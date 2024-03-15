import { Project } from '../models/project';
import * as moment from 'moment';

export class DateConverter {
	static date: Date;

	static toDate(str) {
		const offset = moment().utcOffset();

		if (moment().isDST())
			return moment(str, 'DD-MM-YYYY').add(offset, 'minutes').toDate();
		return moment(str, 'DD-MM-YYYY')
			.add(offset, 'minutes')
			.add(1, 'hour')
			.toDate();
	}

	static addOffset(date: Date) {
		const offset = moment().utcOffset();
		if (moment().isDST()) return moment(date).add(offset, 'minutes').toDate();
		return moment(date).add(offset, 'minutes').add(1, 'hour').toDate();
	}

	//alle dates worden alleen naar de gebruiker toe geconverteerd naar een leesbarevorm
	static toReadableStringFromString(date: string) {
		return moment(date, 'YYYY-MM-DDTHH:mm').format('DD-MM-YYYY');
	}

	static toReadableStringFromDate(date: Date) {
		return moment(date, 'YYYY-MM-DDTHH:mm').format('DD-MM-YYYY');
	}

	static formatProjectDateReadable(project): Project {
		project.participationStartDate = DateConverter.toReadableStringFromString(
			project.participationStartDate
		);
		project.participationEndDate != null
			? (project.participationEndDate =
					DateConverter.toReadableStringFromString(
						project.participationEndDate
					))
			: (project.participationEndDate = null);
		project.projectEndDate = DateConverter.toReadableStringFromString(
			project.projectEndDate
		);
		project.projectStartDate = DateConverter.toReadableStringFromString(
			project.projectStartDate
		);
		return project;
	}

	static todayString(): string {
		return moment().format('DD-MM-YYYY');
	}

	static dateToString(date: Date) {
		if (date != null) {
			return moment(date).format('LL');
		}
	}

	static dateToMoment(date: Date) {
		return moment(date);
	}

	static calculateAge(dateOfBirth: string): string {
		if (dateOfBirth === null || dateOfBirth === undefined) return 'Onbekend';

		const today = new Date();
		const birthDate = DateConverter.toDate(dateOfBirth);
		let age = today.getFullYear() - birthDate.getFullYear();
		const m = today.getMonth() - birthDate.getMonth();

		if (m < 0 || (m === 0 && today.getDate() < birthDate.getDate())) {
			age--;
		}
		return age.toString();
	}
}
