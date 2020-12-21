import {User} from "./user";

export class Schedule {

  public person: User;
  public numberOfTimesScheduledThisProject: number;
  public availabilityId: string;
  public scheduledThisDay: boolean;
  public scheduledThisShift: boolean;
  public preference: boolean;
}
