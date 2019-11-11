import { Entity } from './entity.model';

export class Task extends Entity {
  public name: string;
  public color: string;
  public documentUri: string;

  // TODO: Category
  // public category: Category;
}
