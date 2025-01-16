import { BaseFilter } from './base';

export class ShiftFilter extends BaseFilter {
	projectId: string;
	tasks: string[];
	date: Date;
	start: string;
	end: string;
	participantsRequired: number;
}
