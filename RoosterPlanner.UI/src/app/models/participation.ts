import {Entity} from "./entity.model";
import {User} from "./user";
import {Project} from "./project";
import {Availability} from "./availability";

export class Participation extends Entity {
  public person: User;
  public project: Project;
  public maxWorkingHoursPerWeek: number;
  public Availabilities: Availability[];
  public remark:string;
}
