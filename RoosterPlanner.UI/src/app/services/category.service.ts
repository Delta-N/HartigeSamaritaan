import {Injectable} from '@angular/core';
import {ApiService} from "./api.service";
import {Category} from "../models/category";
import {HttpResponse} from "@angular/common/http";
import {HttpRoutes} from "../helpers/HttpRoutes";
import {EntityHelper} from "../helpers/entity-helper";
import {ErrorService} from "./error.service";

@Injectable({
  providedIn: 'root'
})
export class CategoryService {

  constructor(private apiService: ApiService,
              private errorService: ErrorService) {
  }

  async getCategory(guid: string | null): Promise<Category | null> {
    if (!guid) {
      this.errorService.error("categoryID mag niet leeg zijn")
      return null;
    }
    let category: Category | null = null;
    await this.apiService.get<HttpResponse<Category>>(`${HttpRoutes.taskApiUrl}/GetCategory/${guid}`)
      .toPromise()
      .then(res => {
        if (res?.ok)
          category = res.body;
      }, Error => {
        this.errorService.httpError(Error)
      })
    return category;
  }

  async getAllCategory(): Promise<Category[]> {
    let categories: Category[] | null = [];
    await this.apiService.get<HttpResponse<Category[]>>(`${HttpRoutes.taskApiUrl}/GetAllCategories`)
      .toPromise()
      .then(res => {
          if (res?.ok)
            categories = res.body
        }, Error => {
          this.errorService.httpError(Error)
        }
      );
    return categories
  }

  async postCategory(category: Category): Promise<Category |null> {
    if (!category || !category.name) {
      this.errorService.error("Ongeldige category")
      return null;
    }

    if (!category.id) {
      category.id = EntityHelper.returnEmptyGuid()
    }
    let resCategory: Category | null= null;
    await this.apiService.post<HttpResponse<Category>>(`${HttpRoutes.taskApiUrl}/SaveCategory`, category)
      .toPromise()
      .then(res => {
        if (res?.ok)
          resCategory = res.body
      }, Error => {
        this.errorService.httpError(Error)
      })
    return resCategory
  }

  async updateCategory(category: Category): Promise<Category | null> {
    if (!category || !category.name) {
      this.errorService.error("Ongeldige category")
      return null;
    }
    let updatedCategory: Category | null = null;
    if (!category.id) {
      this.errorService.error("CategoryID is leeg")
      return null;
    }
    await this.apiService.put<HttpResponse<Category>>(`${HttpRoutes.taskApiUrl}/UpdateCategory`, category)
      .toPromise()
      .then(res => {
          if (res?.ok)
            updatedCategory = res.body;
        }, Error => {
          this.errorService.httpError(Error)
        }
      )
    return updatedCategory
  }

  async deleteCategory(guid: string| null): Promise<boolean | null> {
    if (!guid) {
      this.errorService.error("CategoryID is leeg")
      return null;
    }
    let deleted: boolean = false;
    await this.apiService.delete<HttpResponse<Category>>(`${HttpRoutes.taskApiUrl}/DeleteCategory/${guid}`)
      .toPromise()
      .then(res => {
        if (res?.ok)
          deleted = true;
      }, Error => {
        this.errorService.httpError(Error)
      })
    return deleted;
  }
}
