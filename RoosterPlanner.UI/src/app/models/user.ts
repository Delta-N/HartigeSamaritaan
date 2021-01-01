import {Entity} from "./entity.model";
import {Document} from "./document";

export class User extends Entity {
  public city: string;
  public country: string;
  public dateOfBirth: string;
  public email: string;
  public firstName: string;
  public lastName: string;
  public nationality: string;
  public personalRemark: string;
  public phoneNumber: string;
  public postalCode: string;
  public profilePicture: Document;
  public staffRemark: string;
  public streetAddress: string;
  public termsOfUseConsented: string;
  public userRole: string;
}
