import {User} from "./user";
import {Availability} from "./availability";

export class Schedule {

  public person: User;
  public numberOfTimesScheduledThisProject: number;
  public availabilityId: string;
  public scheduledThisDay: boolean;
  public scheduledThisShift: boolean;
  public preference: boolean;
  public availabilities: Availability[];
}
