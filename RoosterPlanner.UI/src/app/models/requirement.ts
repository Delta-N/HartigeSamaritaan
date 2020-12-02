import {Entity} from "./entity.model";
import {Task} from "./task";

export class Requirement extends Entity {
  public task: Task;
  public certificateType: any; //aanpassen zodra we verder gaan met certificaten
}
