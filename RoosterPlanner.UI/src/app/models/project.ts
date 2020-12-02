import {Entity} from "./entity.model";

export class Project extends Entity {
  public name: string;
  public address: string;
  public city: string;
  public description: string;
  public participationStartDate: Date;
  public participationEndDate?: Date;
  public projectStartDate: Date;
  public projectEndDate: Date;
  public pictureUri?: string;
  public websiteUrl?: string;
  public closed?: boolean;
}
