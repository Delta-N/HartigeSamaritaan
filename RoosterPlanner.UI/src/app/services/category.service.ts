import {Injectable} from '@angular/core';
import {ApiService} from "./api.service";
import {ToastrService} from "ngx-toastr";
import {Category} from "../models/category";
import {HttpResponse} from "@angular/common/http";
import {Task} from "../models/task";
import {HttpRoutes} from "../helpers/HttpRoutes";
import {EntityHelper} from "../helpers/entity-helper";

@Injectable({
  providedIn: 'root'
})
export class CategoryService {

  constructor(private apiService: ApiService,
              private toastr: ToastrService) {
  }

  async getCategory(guid: string): Promise<Category> {
    let category: Category = null;
    await this.apiService.get<HttpResponse<Category>>(`${HttpRoutes.taskApiUrl}/GetCategory/${guid}`).toPromise().then(response => {
      category = response.body;
    })
    return category;
  }

  async getAllCategory(): Promise<Category[]> {
    let categories: Category[] = [];
    await this.apiService.get<HttpResponse<Category[]>>(`${HttpRoutes.taskApiUrl}/GetAllCategories`).toPromise().then(response =>
      categories = response.body
    );
    return categories
  }

  postCategory(category: Category) {
    if (category === null || category.name === null) {
      this.toastr.error("Ongeldige category")
      return null;
    }

    if (category.id === null || category.id === "") {
      category.id = EntityHelper.returnEmptyGuid()
    }
    return this.apiService.post<HttpResponse<Task>>(`${HttpRoutes.taskApiUrl}/SaveCategory`, category).toPromise()
  }

  updateCategory(category: Category) {
    if (category === null || category.name === null) {
      this.toastr.error("Ongeldige category")
      return null;
    }
    if (category.id === null || category.id === "") {
      this.toastr.error("CategoryID is leeg")
      return null;
    }
    return this.apiService.patch<HttpResponse<Task>>(`${HttpRoutes.taskApiUrl}/UpdateCategory`, category).toPromise()
  }

  deleteCategory(guid: string) {
    if (guid === null || guid == "") {
      this.toastr.error("CategoryID is leeg")
      return null;
    }
    return this.apiService.delete<HttpResponse<Number>>(`${HttpRoutes.taskApiUrl}/DeleteCategory/${guid}`).toPromise()
  }
}
