import {Entity} from "./entity.model";

export class Project extends Entity {
  public id: string;
  public name: string;
  public address: string;
  public city: string;
  public description: string;
  public startDate: Date;
  public endDate?: Date;
  public pictureUri?: string;
  public websiteUrl?: string;
  public closed?: boolean;
}
