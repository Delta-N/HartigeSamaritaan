import {Task} from "./task";

export class AvailabilityData {

  public projectTasks: Task[];
  public knownAvailabilities: ScheduleStatus[];
}

export class ScheduleStatus{
  public date:string;
  public status: number;
  // 0= incomplete
  // 1= At least one day availabilbe
  // 2 = scheduled
  // 3 = unavailable
}
