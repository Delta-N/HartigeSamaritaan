import { Entity } from './entity.model';
import { Document } from './document';
import { Certificate } from './Certificate';

export class User extends Entity {
	city: string;
	dateOfBirth: string;
	dutchProficiency: string;
	email: string;
	firstName: string;
	lastName: string;
	nationality: string;
	nativeLanguage: string;
	personalRemark: string;
	phoneNumber: string;
	postalCode: string;
	profilePicture: Document;
	pushDisabled: boolean;
	staffRemark: string;
	streetAddress: string;
	termsOfUseConsented: string;
	userRole: string;
	certificates: Certificate[];
}
