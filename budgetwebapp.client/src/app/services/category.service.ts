import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Category } from '../models';
import { Observable } from 'rxjs';
import { environment as env } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {

  constructor(private http: HttpClient) {}

  getAllCategories(): Observable<Category[]> {
    return this.http.get<Category[]>(`${env.BASE_URL}/Category/GetAllCategories`);
  }

  getCategoryById(id: number): Observable<Category> {
    return this.http.get<Category>(`${env.BASE_URL}/Category/GetCategoryById/${id}`);
  }

  addCategory(category: Category): Observable<Category> {
    return this.http.post<Category>(`${env.BASE_URL}/Category/AddCategory`, category);
  }

  updateCategory(category: Category): Observable<Category> {
    return this.http.put<Category>(`${env.BASE_URL}/Category/UpdateCategory`, category);
  }

  deleteCategory(id: number): Observable<void> {
    return this.http.delete<void>(`${env.BASE_URL}/Category/DeleteCategory/${id}`);
  }
}