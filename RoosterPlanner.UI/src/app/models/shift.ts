import {Entity} from './entity.model';
import {Task} from './task';
import {Project} from './project';
import {Availability} from './availability';

export class Shift extends Entity {
  public project: Project;
  public task: Task;
  public date: Date;
  public startTime: string;
  public endTime: string;
  public participantsRequired: number;
  public availabilities: Availability[];
}
