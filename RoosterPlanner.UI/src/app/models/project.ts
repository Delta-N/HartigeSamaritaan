export class Project {
    public id: string;
    public name: string;
    public address: string;
    public city: string;
    public description:string;
    public startDate: Date;
    public endDate?: Date;
    public pictureUri?:string;
    //waarom websiteUri?
    public websiteUri?: string;
    public closed?: boolean;


  constructor(id: string, name?: string, address?: string, city?: string, description?: string, startDate?: Date, endDate?: Date, pictureUri?: string, websiteUri?: string, closed?: boolean) {
    this.id = id;
    this.name = name;
    this.address = address;
    this.city = city;
    this.description = description;
    this.startDate = startDate;
    this.endDate = endDate;
    this.pictureUri = pictureUri;
    this.websiteUri = websiteUri;
    this.closed = closed;
  }
}
