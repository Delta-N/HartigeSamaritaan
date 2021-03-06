﻿import {Entity} from "./entity.model";
import {Category} from "./category";
import {Document} from "./document";
import {Requirement} from "./requirement";

export class Task extends Entity {

  public name: string;
  public category: Category;
  public color: string;
  public instruction: Document;
  public description: string;
  public requirements: Requirement[];
}
