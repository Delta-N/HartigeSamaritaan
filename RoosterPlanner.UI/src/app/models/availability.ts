import { Entity } from './entity.model';
import { Participation } from './participation';
import { Shift } from './shift';

export class Availability extends Entity {
	participationId: string;
	participation: Participation;
	shiftId: string;
	shift: Shift;
	type: number;
	preference: boolean;
	pushEmailSend: boolean;
}
