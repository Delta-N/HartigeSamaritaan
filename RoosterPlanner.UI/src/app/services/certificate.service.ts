import {Injectable} from '@angular/core';
import {ApiService} from './api.service';
import {ErrorService} from './error.service';
import {HttpResponse} from '@angular/common/http';
import {HttpRoutes} from '../helpers/HttpRoutes';
import {Certificate} from '../models/Certificate';
import {CertificateType} from '../models/CertificateType';
import {EntityHelper} from '../helpers/entity-helper';
import {DateConverter} from '../helpers/date-converter';

@Injectable({
  providedIn: 'root'
})
export class CertificateService {

  constructor(private apiService: ApiService,
              private errorService: ErrorService) {
  }

  async getAllCertificateTypes(): Promise<CertificateType[]> {
    let certificateTypes: CertificateType[] = [];
    await this.apiService.get <HttpResponse<CertificateType[]>>(`${HttpRoutes.certificateApiUrl}/types`)
      .toPromise()
      .then(res => {
          if (res.ok) {
            certificateTypes = res.body;
          }
        }, Error => {
          this.errorService.httpError(Error);
        }
      );
    return certificateTypes;
  }

  async getCertificateType(guid: string): Promise<CertificateType> {
    if (!guid) {
      this.errorService.error('certificateTypeId mag niet leeg zijn');
      return null;
    }
    let certificateType: CertificateType = null;
    await this.apiService.get <HttpResponse<CertificateType>>(`${HttpRoutes.certificateApiUrl}/types/${guid}`)
      .toPromise()
      .then(res => {
          if (res.ok) {
            certificateType = res.body;
          }
        }, Error => {
          this.errorService.httpError(Error);
        }
      );
    return certificateType;
  }

  async postCertificateType(certificateType: CertificateType): Promise<CertificateType> {
    if (certificateType == null || certificateType.name == null) {
      this.errorService.error('Ongeldige CertificateType');
      return null;
    }
    if (!certificateType.id) {
      certificateType.id = EntityHelper.returnEmptyGuid();
    }

    let resCertificateType: CertificateType = null;
    await this.apiService.post<HttpResponse<CertificateType>>(`${HttpRoutes.certificateApiUrl}/types`, certificateType)
      .toPromise()
      .then(res => {
        if (res.ok) {
          resCertificateType = res.body;
        }
      }, Error => {
        this.errorService.httpError(Error);
      });
    return resCertificateType;
  }

  async updateCertificateType(updatedCertificateType: CertificateType): Promise<CertificateType> {
    if (!updatedCertificateType || !updatedCertificateType.name || !updatedCertificateType || updatedCertificateType.id == EntityHelper.returnEmptyGuid()) {
      this.errorService.error('Ongeldige CertificateType');
      return null;
    }
    let resCertificateType: CertificateType = null;
    await this.apiService.put<HttpResponse<CertificateType>>(`${HttpRoutes.certificateApiUrl}/types`, updatedCertificateType)
      .toPromise()
      .then(res => {
        if (res.ok) {
          resCertificateType = res.body;
        }
      }, Error => {
        this.errorService.httpError(Error);
      });
    return resCertificateType;
  }

  async deleteCertificateType(guid: string): Promise<boolean> {
    if (!guid) {
      this.errorService.error('Ongeldige CertificateType');
      return null;
    }
    let result = false;
    await this.apiService.delete<HttpResponse<CertificateType>>(`${HttpRoutes.certificateApiUrl}/types/${guid}`)
      .toPromise()
      .then(res => {
        if (res.ok) {
          result = true;
        }
      }, Error => {
        this.errorService.httpError(Error);
      });
    return result;
  }

  async getCertificate(guid: string): Promise<Certificate> {
    if (!guid) {
      this.errorService.error('certificateId mag niet leeg zijn');
      return null;
    }
    let certificate: Certificate = null;
    await this.apiService.get<HttpResponse<Certificate>>(`${HttpRoutes.certificateApiUrl}/certificate/${guid}`)
      .toPromise()
      .then(res => {
          if (res.ok) {
            certificate = res.body;
          }
        }
        , Error => {
          this.errorService.httpError(Error);
        });
    return certificate;
  }

  async postCertificate(certificate: Certificate): Promise<Certificate> {
    if (!certificate || !certificate.person || !certificate.certificateType || !certificate.dateIssued) {
      this.errorService.error('Ongeldig certificaat');
      return null;
    }

    if (!certificate.id) {
      certificate.id = EntityHelper.returnEmptyGuid();
    }

    if (certificate.dateExpired) {
      certificate.dateExpired = DateConverter.toDate(certificate.dateExpired);
    }
    else {
      certificate.dateExpired = null;
    }
    certificate.dateIssued = DateConverter.toDate(certificate.dateIssued);

    let resCertificate: Certificate = null;
    await this.apiService.post<HttpResponse<Certificate>>(`${HttpRoutes.certificateApiUrl}/certificate`, certificate)
      .toPromise()
      .then(res => {
        if (res.ok) {
          resCertificate = res.body;
        }
      }, Error => {
        this.errorService.httpError(Error);
      });
    return resCertificate;
  }

  async updateCertificate(certificate: Certificate): Promise<Certificate> {
    if (!certificate || !certificate.person || !certificate.certificateType || !certificate.dateIssued || !certificate.id || certificate.id == EntityHelper.returnEmptyGuid()) {
      this.errorService.error('Ongeldig certificaat');
      return null;
    }
    if (certificate.dateExpired) {
      certificate.dateExpired = DateConverter.toDate(certificate.dateExpired);
    }
    else {
      certificate.dateExpired = null;
    }
    certificate.dateIssued = DateConverter.toDate(certificate.dateIssued);

    let resCertificate: Certificate = null;
    await this.apiService.put<HttpResponse<Certificate>>(`${HttpRoutes.certificateApiUrl}/certificate`, certificate)
      .toPromise()
      .then(res => {
        if (res.ok) {
          resCertificate = res.body;
        }
      }, Error => {
        this.errorService.httpError(Error);
      });
    return resCertificate;
  }

  async deleteCertificate(guid: string): Promise<boolean> {
    if (!guid) {
      this.errorService.error('CertificateId is leeg');
      return null;
    }
    let result = false;
    await this.apiService.delete<HttpResponse<Certificate>>(`${HttpRoutes.certificateApiUrl}/certificate/${guid}`)
      .toPromise()
      .then(res => {
        if (res.ok) {
          result = true;
        }
      }, Error => {
        this.errorService.httpError(Error);
      });
    return result;
  }
}
