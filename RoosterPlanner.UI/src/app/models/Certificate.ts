import {Entity} from "./entity.model";
import {User} from "./user";
import {CertificateType} from "./CertificateType";

export class Certificate extends Entity {
  public dateIssued: Date;
  public dateExpired: Date | null;
  public person: User;
  public certificateType: CertificateType;
}
