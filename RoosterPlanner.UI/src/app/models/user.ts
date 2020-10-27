import {Entity} from "./entity.model";

export class User extends Entity {
  public id: string;
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

  constructor(id:string,firstName?:string,lastName?:string,email?:string,streetAddress?:string,postalCode?:string,city?:string,country?:string,dateOfBirth?:string,phoneNumber?:string,userRole?:string) {
    super();
    this.id = id;
    this.firstName=firstName;
    this.lastName=lastName;
    this.email=email;
    this.streetAddress=streetAddress;
    this.postalCode=postalCode;
    this.city=city;
    this.country=country;
    this.dateOfBirth=dateOfBirth;
    this.phoneNumber=phoneNumber;
    this.userRole=userRole;
  }
}
