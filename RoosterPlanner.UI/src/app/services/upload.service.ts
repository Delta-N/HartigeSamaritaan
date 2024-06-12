import { HttpClient, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { HttpRoutes } from '../helpers/HttpRoutes';
import { Uploadresult } from '../models/uploadresult';
import { ErrorService } from './error.service';
import { Document } from '../models/document';

@Injectable({ providedIn: 'root' })
export class UploadService {
	constructor(
		private http: HttpClient,
		private apiService: ApiService,
		private errorService: ErrorService
	) {}

	async uploadInstruction(formData: FormData): Promise<Uploadresult> {
		if (!formData) {
			this.errorService.error('Instuctie-bestand is leeg');
			return null;
		}
		let uploadResult: Uploadresult = null;
		await this.apiService
			.postMultiPartFormData<Uploadresult>(
				`${HttpRoutes.uploadApiUrl}`,
				formData
			)
			.toPromise()
			.then((res) => {
				if (res.succeeded) uploadResult = res;
			});
		return uploadResult;
	}

	async uploadProjectPicture(formData: FormData): Promise<Uploadresult> {
		if (!formData) {
			this.errorService.error('Projectfoto-bestand is leeg');
			return null;
		}
		let uploadResult: Uploadresult = null;
		await this.apiService
			.postMultiPartFormData<Uploadresult>(
				`${HttpRoutes.uploadApiUrl}/UploadProjectPicture`,
				formData
			)
			.toPromise()
			.then((res) => {
				if (res.succeeded) uploadResult = res;
			});
		return uploadResult;
	}

	async uploadProfilePicture(formData: FormData): Promise<Uploadresult> {
		if (!formData) {
			this.errorService.error('Profielfoto-bestand is leeg');
			return null;
		}
		let uploadResult: Uploadresult = null;
		await this.apiService
			.postMultiPartFormData<Uploadresult>(
				`${HttpRoutes.uploadApiUrl}/UploadProfilePicture`,
				formData
			)
			.toPromise()
			.then((res) => {
				if (res.succeeded) uploadResult = res;
			});
		return uploadResult;
	}

	async uploadPP(formData: FormData): Promise<Uploadresult> {
		if (!formData) {
			this.errorService.error('TOS-bestand is leeg');
			return null;
		}
		let uploadResult: Uploadresult = null;
		await this.apiService
			.postMultiPartFormData<Uploadresult>(
				`${HttpRoutes.uploadApiUrl}/UploadPP`,
				formData
			)
			.toPromise()
			.then((res) => {
				if (res.succeeded) uploadResult = res;
			});
		return uploadResult;
	}

	async deleteIfExists(url: string): Promise<Uploadresult> {
		if (!url) {
			this.errorService.error('url is leeg');
		}
		let uploadResult: Uploadresult = null;

		await this.apiService
			.delete<Uploadresult>(`${HttpRoutes.uploadApiUrl}?Url=${url}`)
			.toPromise()
			.then((res) => {
				uploadResult = res;
			});
		return uploadResult;
	}

	async getPP(): Promise<Document> {
		let TOS: Document = null;
		await this.apiService
			.get<HttpResponse<Document>>(`${HttpRoutes.uploadApiUrl}/PrivacyPolicy`)
			.toPromise()
			.then(
				(res) => {
					if (res.ok) TOS = res.body;
				},
				(Error) => {
					this.errorService.httpError(Error);
				}
			);
		return TOS;
	}

	async postDocument(document: Document): Promise<Document> {
		if (!document) {
			this.errorService.error('Document is ongeldig');
			return null;
		}
		let postedDocument: Document = null;
		await this.apiService
			.post<HttpResponse<Document>>(
				`${HttpRoutes.uploadApiUrl}/document`,
				document
			)
			.toPromise()
			.then(
				(res) => {
					if (res.ok) postedDocument = res.body;
				},
				(Error) => {
					this.errorService.httpError(Error);
				}
			);
		return postedDocument;
	}

	async updateDocument(document: Document): Promise<Document> {
		if (!document) {
			this.errorService.error('Document is ongeldig');
			return null;
		}
		let updatedDocument: Document = null;
		await this.apiService
			.put<HttpResponse<Document>>(`${HttpRoutes.uploadApiUrl}`, document)
			.toPromise()
			.then(
				(res) => {
					if (res.ok) updatedDocument = res.body;
				},
				(Error) => {
					this.errorService.httpError(Error);
				}
			);
		return updatedDocument;
	}

	async removeDocument(document: Document): Promise<boolean> {
		if (!document) {
			this.errorService.error('Document is ongeldig');
			return null;
		}
		let deleted = false;
		await this.apiService
			.delete<HttpResponse<Document>>(
				`${HttpRoutes.uploadApiUrl}/document/${document.id}`
			)
			.toPromise()
			.then(
				(res) => {
					if (res.ok) deleted = true;
				},
				(Error) => {
					this.errorService.httpError(Error);
				}
			);
		return deleted;
	}
}
