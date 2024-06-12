import { Entity } from './entity.model';
import { User } from './user';
import { CertificateType } from './CertificateType';

export class Certificate extends Entity {
	dateIssued: Date;
	dateExpired: Date;
	person: User;
	certificateType: CertificateType;
}
