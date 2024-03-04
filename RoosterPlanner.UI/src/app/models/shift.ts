import { Entity } from './entity.model';
import { Task } from './task';
import { Project } from './project';
import { Availability } from './availability';

export class Shift extends Entity {
	project: Project;
	task: Task;
	date: Date;
	startTime: string;
	endTime: string;
	participantsRequired: number;
	availabilities: Availability[];
}
