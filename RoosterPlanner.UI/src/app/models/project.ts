import { Entity } from './entity.model';
import { Document } from './document';

export class Project extends Entity {
	name: string;
	address: string;
	city: string;
	description: string;
	participationStartDate: Date;
	participationEndDate?: Date;
	projectStartDate: Date;
	projectEndDate: Date;
	pictureUri?: Document;
	websiteUrl?: string;
	closed?: boolean;
	contactAdres: string;
}
