import { Task } from './task';

export class AvailabilityData {
	projectTasks: Task[];
	knownAvailabilities: ScheduleStatus[];
}

export class ScheduleStatus {
	date: string;
	status: number;
	// 0= incomplete
	// 1= At least one day availabilbe
	// 2 = scheduled
	// 3 = unavailable
}
