import {Entity} from "./entity.model";
import {User} from "./user";
import {Project} from "./project";

export class Participation extends Entity {
public id:string;
public person:User;
public project:Project;
public MaxWorkingHoursPerWeek:number;
public Availabilities:any; //list van availiblities maken todo
public WantsToWorkWith:any; //list van collaborations


  constructor(id: string, person?: User, project?: Project, MaxWorkingHoursPerWeek?: number, Availabilities?: any, WantsToWorkWith?: any) {
    super();
    this.id = id;
    this.person = person;
    this.project = project;
    this.MaxWorkingHoursPerWeek = MaxWorkingHoursPerWeek;
    this.Availabilities = Availabilities;
    this.WantsToWorkWith = WantsToWorkWith;
  }
}
