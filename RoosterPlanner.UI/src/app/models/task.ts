import { Entity } from './entity.model';
import { Category } from './category';
import { Document } from './document';
import { Requirement } from './requirement';

export class Task extends Entity {
	name: string;
	category: Category;
	color: string;
	instruction: Document;
	description: string;
	requirements: Requirement[];
}
