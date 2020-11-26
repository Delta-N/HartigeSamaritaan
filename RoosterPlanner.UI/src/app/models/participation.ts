import {Entity} from "./entity.model";
import {User} from "./user";
import {Project} from "./project";

export class Participation extends Entity {
  public person: User;
  public project: Project;
  public maxWorkingHoursPerWeek: number;
  public Availabilities: any; //list van availiblities maken
  public WantsToWorkWith: any; //list van collaborations
}
