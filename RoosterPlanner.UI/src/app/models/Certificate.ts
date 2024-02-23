import {Entity} from './entity.model';
import {User} from './user';
import {CertificateType} from './CertificateType';

export class Certificate extends Entity {
  public dateIssued: Date;
  public dateExpired: Date;
  public person: User;
  public certificateType: CertificateType;
}
