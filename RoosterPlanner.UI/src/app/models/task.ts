import {Entity} from "./entity.model";
import {Category} from "./category";
import {Requirement} from "./requirement";

export class Task extends Entity {

  public name: string;
  public category: Category;
  public color: string;
  public documentUri: string;
  public requirements: Requirement[];
  public description:string;
}
