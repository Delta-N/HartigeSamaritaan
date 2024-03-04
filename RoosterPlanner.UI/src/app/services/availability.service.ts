import { Injectable } from '@angular/core';
import { HttpResponse } from '@angular/common/http';
import { HttpRoutes } from '../helpers/HttpRoutes';
import { ErrorService } from './error.service';
import { ApiService } from './api.service';
import { AvailabilityData } from '../models/availabilitydata';
import { EntityHelper } from '../helpers/entity-helper';
import { Availability } from '../models/availability';
import { Schedule } from '../models/schedule';

@Injectable({
	providedIn: 'root',
})
export class AvailabilityService {
	constructor(
		private errorService: ErrorService,
		private apiService: ApiService
	) {}

	async getAvailabilityData(
		projectId: string | null,
		userId: string
	): Promise<AvailabilityData | null> {
		if (!projectId) {
			this.errorService.error('projectId mag niet leeg zijn');
			return null;
		}
		if (!userId) {
			this.errorService.error('userId mag niet leeg zijn');
			return null;
		}
		let availabilityDate: AvailabilityData | null = null;
		await this.apiService
			.get<HttpResponse<AvailabilityData>>(
				`${HttpRoutes.availabilityApiUrl}/find/${projectId}/${userId}`
			)
			.toPromise()
			.then(
				(res) => {
					if (res?.ok) availabilityDate = res.body;
				},
				(Error) => {
					this.errorService.httpError(Error);
				}
			);
		return availabilityDate;
	}

	async getScheduledAvailabilities(
		participationId: string | null
	): Promise<Availability[] | null> {
		if (!participationId) {
			this.errorService.error('participationId mag niet leeg zijn');
			return null;
		}
		let availabilities: Availability[] | null = null;
		await this.apiService
			.get<HttpResponse<Availability[]>>(
				`${HttpRoutes.availabilityApiUrl}/scheduled/${participationId}`
			)
			.toPromise()
			.then(
				(res) => {
					if (res?.ok) availabilities = res.body;
				},
				(Error) => {
					this.errorService.httpError(Error);
				}
			);
		return availabilities;
	}

	async getScheduledAvailabilitiesOnDate(
		projectId: string | null,
		date: Date
	): Promise<Availability[] | null> {
		if (!projectId) {
			this.errorService.error('participationId mag niet leeg zijn');
			return null;
		}
		if (!date) {
			this.errorService.error('datum mag niet leeg zijn');
			return null;
		}
		let availabilities: Availability[] | null = null;
		await this.apiService
			.get<HttpResponse<Availability[]>>(
				`${HttpRoutes.availabilityApiUrl}/scheduled/${projectId}/${date.toISOString()}`
			)
			.toPromise()
			.then(
				(res) => {
					if (res?.ok) availabilities = res.body;
				},
				(Error) => {
					this.errorService.httpError(Error);
				}
			);
		return availabilities;
	}

	async getAvailabilityDataOfProject(
		projectId: string | null
	): Promise<AvailabilityData | null> {
		if (!projectId) {
			this.errorService.error('projectId mag niet leeg zijn');
			return null;
		}
		let availabilityDate: AvailabilityData | null = null;
		await this.apiService
			.get<HttpResponse<AvailabilityData>>(
				`${HttpRoutes.availabilityApiUrl}/find/${projectId}`
			)
			.toPromise()
			.then(
				(res) => {
					if (res?.ok) availabilityDate = res.body;
				},
				(Error) => {
					this.errorService.httpError(Error);
				}
			);
		return availabilityDate;
	}

	async postAvailability(
		availability: Availability
	): Promise<Availability | null> {
		if (
			!availability ||
			!availability.participation ||
			!availability.shift ||
			availability.type === undefined
		) {
			this.errorService.error('Ongeldige availability');
			return null;
		}

		if (!availability.id) {
			availability.id = EntityHelper.returnEmptyGuid();
		}
		let resAvailability: Availability | null = null;
		await this.apiService
			.post<HttpResponse<Availability>>(
				`${HttpRoutes.availabilityApiUrl}`,
				availability
			)
			.toPromise()
			.then(
				(res) => {
					if (res?.ok) resAvailability = res.body;
				},
				(Error) => {
					this.errorService.httpError(Error);
				}
			);
		return resAvailability;
	}

	async updateAvailability(
		availability: Availability
	): Promise<Availability | null> {
		if (
			!availability ||
			!availability.participationId ||
			!availability.shiftId ||
			availability.type === undefined
		) {
			this.errorService.error('Ongeldige availability');
			return null;
		}
		let updatedAvailability: Availability | null = null;
		if (!availability.id) {
			this.errorService.error('availabilityId is leeg');
			return null;
		}
		await this.apiService
			.put<HttpResponse<Availability>>(
				`${HttpRoutes.availabilityApiUrl}`,
				availability
			)
			.toPromise()
			.then(
				(res) => {
					if (res?.ok) updatedAvailability = res.body;
				},
				(Error) => {
					this.errorService.httpError(Error);
				}
			);
		return updatedAvailability;
	}

	async changeAvailabilities(scheduled: Schedule[]): Promise<boolean> {
		if (!scheduled) {
			this.errorService.error('Ongeldige schedule ontvangen');
			return false;
		}
		let success: boolean = false;
		await this.apiService
			.patch<HttpResponse<boolean>>(
				`${HttpRoutes.availabilityApiUrl}`,
				scheduled
			)
			.toPromise()
			.then(
				(res) => {
					if (res?.ok) success = true;
				},
				(Error) => {
					this.errorService.httpError(Error);
				}
			);
		return success;
	}
}
