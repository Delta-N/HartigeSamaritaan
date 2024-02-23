import {Entity} from './entity.model';
import {User} from './user';
import {Project} from './project';
import {Availability} from './availability';

export class Participation extends Entity {
  public Availabilities: Availability[];
  public active: boolean;
  public maxWorkingHoursPerWeek: number;
  public person: User;
  public project: Project;
  public remark: string;
}
