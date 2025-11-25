import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BudgetLineItems } from '../models';
import { Observable } from 'rxjs';
import { environment as env } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class BudgetLineItemService {

  constructor(private http: HttpClient) { }

  getBudgetLineItemsByBudgetId(id: number): Observable<BudgetLineItems[]> {
    return this.http.get<BudgetLineItems[]>(`${env.BASE_URL}/BudgetLineItem/GetBudgetLineItemsByBudgetId/${id}`);
  }

  getBudgetLineItemById(id: number): Observable<BudgetLineItems> {
    return this.http.get<BudgetLineItems>(`${env.BASE_URL}/BudgetLineItem/GetBudgetLineItemById/${id}`);
  }

  addBudgetLineItem(lineItem: BudgetLineItems): Observable<BudgetLineItems> {
    const payload = {
      categoryId: lineItem.categoryId,
      value: lineItem.value,
      budgetId: lineItem.budgetId,
      sourceTypeId: lineItem.sourceTypeId,
      label: lineItem.label,
      category: lineItem.category,
      sourceType: lineItem.sourceType
    };
    console.log(payload);
    return this.http.post<BudgetLineItems>(`${env.BASE_URL}/BudgetLineItem/AddBudgetLineItem`, payload);
  }

  updateBudgetLineItem(lineItem: BudgetLineItems): Observable<BudgetLineItems> {
    return this.http.put<BudgetLineItems>(`${env.BASE_URL}/BudgetLineItem/UpdateBudgetLineItem`, lineItem);
  }

  deleteBudgetLineItem(id: number): Observable<void> {
    return this.http.delete<void>(`${env.BASE_URL}/BudgetLineItem/DeleteBudgetLineItem/${id}`);
  }
}