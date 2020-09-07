import { Entity } from './entity.model';

export class Project extends Entity {
  public name: string;
  public address: string;
  public city: string;
  public description: string;
  public startDate: Date;
  public endDate: Date;
  public pictureUri: string;
  public websiteUrl: string;
  public closed: boolean;

  // TODO:
  // public projectTasks: Array<ProjectTask>;
  // public participation: Array<Participation>;

  constructor() {
    super();
    // this.projectTasks = new Array<ProjectTask>();
    // this.participation = new Array<Participation>();
  }
}
