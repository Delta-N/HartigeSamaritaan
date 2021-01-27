import {Entity} from "./entity.model";
import {Project} from "./project";
import {User} from "./user";

export class Manager extends Entity {
  public projectId: string;
  public personId: string;
  public project: Project;
  public person: User;

}
