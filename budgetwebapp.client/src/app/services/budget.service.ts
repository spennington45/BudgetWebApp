import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Budget, BudgetLineItems, SourceType } from '../models';
import { Observable } from 'rxjs';
import { environment as env } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class BudgetService {

  constructor(private http: HttpClient) { }

  getBudgetByUserId(id: string): Observable<Array<Budget>> {
    return this.http.get<Budget[]>(`${env.BASE_URL}/Budget/GetBudgetByUserId/${id}`);
  }

  getBudgetLineItemsByBudgetId(id: string): Observable<Array<BudgetLineItems>> {
    return this.http.get<BudgetLineItems[]>(`${env.BASE_URL}/Budget/GetBudgetLineItemsByBudgetId/${id}`);
  }

  createBudgetLineItem(budgetId: string, lineItem: BudgetLineItems): Observable<BudgetLineItems> {
    return this.http.post<BudgetLineItems>(`${env.BASE_URL}/Budget/CreateBudgetLineItem/${budgetId}`, lineItem);
  }

  deleteBudgetLineItem(id: string): Observable<void> {
    return this.http.delete<void>(`${env.BASE_URL}/Budget/DeleteBudgetLineItem/${id}`);
  }

  updateBudgetLineItem(id: string, lineItem: BudgetLineItems): Observable<BudgetLineItems> {
    return this.http.put<BudgetLineItems>(`${env.BASE_URL}/Budget/UpdateBudgetLineItem/${id}`, lineItem);
  }

  getSourceTypes(): Observable<SourceType[]> {
    return this.http.get<SourceType[]>(`${env.BASE_URL}/Budget/GetSourceTypes`);
  }
}
