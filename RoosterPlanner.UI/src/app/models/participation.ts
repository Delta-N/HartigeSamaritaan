import { Entity } from './entity.model';
import { User } from './user';
import { Project } from './project';
import { Availability } from './availability';

export class Participation extends Entity {
	Availabilities: Availability[];
	active: boolean;
	maxWorkingHoursPerWeek: number;
	person: User;
	project: Project;
	remark: string;
}
