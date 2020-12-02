import {Entity} from "./entity.model";

export class User extends Entity {
  public firstName: string;
  public lastName: string;
  public email: string;
  public streetAddress: string;
  public postalCode: string;
  public city: string;
  public country: string;
  public dateOfBirth: string;
  public phoneNumber: string;
  public userRole: string;
}
