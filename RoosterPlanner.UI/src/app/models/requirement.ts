import { Entity } from './entity.model';
import { Task } from './task';
import { CertificateType } from './CertificateType';

export class Requirement extends Entity {
	task: Task;
	certificateType: CertificateType;
}
