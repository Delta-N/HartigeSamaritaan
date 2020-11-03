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


  constructor(id: string, name?: string, address?: string, city?: string, description?: string, startDate?: Date, endDate?: Date, pictureUri?: string, websiteUrl?: string, closed?: boolean,) {
    super();
    this.id = id;
    this.name = name;
    this.address = address;
    this.city = city;
    this.description = description;
    this.startDate = startDate;
    this.endDate = endDate;
    this.pictureUri = pictureUri;
    this.websiteUrl = websiteUrl;
    this.closed = closed;
  }
}
