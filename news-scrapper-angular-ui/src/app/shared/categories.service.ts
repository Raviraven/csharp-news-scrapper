import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { CategoryAdd } from './categories/category-add.model';
import { CategoryEdit } from './categories/category-edit.model';
import { Category } from './categories/category.model';

@Injectable({
  providedIn: 'root'
})
export class CategoriesService {

  constructor(private http: HttpClient) { }

  readonly baseUrl = environment.apiUrl + 'categories/';

  getAll(){
    return this.http.get<Category[]>(this.baseUrl);
  }

  add(model: CategoryAdd){
    return this.http.post<CategoryAdd>(this.baseUrl, model);
  }

  save(model: CategoryEdit){
    return this.http.put<CategoryEdit>(this.baseUrl, model);
  }

  get(id: number){
    return this.http.get<Category>(this.baseUrl+id);
  }

  delete(id: number){
    return this.http.delete(this.baseUrl + id);
  }
}
