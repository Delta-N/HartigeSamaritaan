import {Entity} from "./entity.model";
import {Participation} from "./participation";
import {Shift} from "./shift";

export class Availability extends Entity {
  public participationId: string;
  public participation: Participation;
  public shiftId: string;
  public shift: Shift;
  public availabilityType: number
  public preference: boolean;
}
