import {Entity} from "./entity.model";
import {User} from "./user";
import {Project} from "./project";

export class Participation extends Entity {
  public id: string;
  public person: User;
  public project: Project;
  public maxWorkingHoursPerWeek: number;
  public Availabilities: any; //list van availiblities maken
  public WantsToWorkWith: any; //list van collaborations


  constructor(id: string, person?: User, project?: Project, maxWorkingHoursPerWeek?: number, Availabilities?: any, WantsToWorkWith?: any) {
    super();
    this.id = id;
    this.person = person;
    this.project = project;
    this.maxWorkingHoursPerWeek = maxWorkingHoursPerWeek;
    this.Availabilities = Availabilities;
    this.WantsToWorkWith = WantsToWorkWith;
  }
}
