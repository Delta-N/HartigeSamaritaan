import {BaseFilter} from "./base";

export class ShiftFilter extends BaseFilter {

  public projectId: string | undefined
  public tasks: string[]
  public date: Date
  public start: string;
  public end: string;
  public participantsRequired: number;
}
