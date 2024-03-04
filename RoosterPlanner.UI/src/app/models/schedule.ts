import { User } from './user';
import { Availability } from './availability';

export class Schedule {
	person: User;
	numberOfTimesScheduledThisProject: number;
	availabilityId: string;
	scheduledThisDay: boolean;
	scheduledThisShift: boolean;
	preference: boolean;
	availabilities: Availability[];
	hoursScheduledThisWeek: number;
	employability: number;
}
