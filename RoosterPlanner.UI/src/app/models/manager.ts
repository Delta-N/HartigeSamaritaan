import { Entity } from './entity.model';
import { Project } from './project';
import { User } from './user';

export class Manager extends Entity {
	projectId: string;
	personId: string;
	project: Project;
	person: User;
}
