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
      budgetLineItemId: lineItem.budgetLineItemId,
      budgetId: lineItem.budgetId,
      transactionId: lineItem.transactionId ?? null,
      pendingTransactionId: lineItem.pendingTransactionId ?? null,
      date: lineItem.date,
      value: lineItem.value,
      name: lineItem.name,
      merchantName: lineItem.merchantName,
      pending: lineItem.pending,
      categoryId: lineItem.categoryId,
      plaidAccountId: lineItem.plaidAccountId ?? null,
      userId: lineItem.userId,
      sourceTypeId: lineItem.sourceTypeId
    };
    console.log(payload);
    return this.http.post<BudgetLineItems>(`${env.BASE_URL}/BudgetLineItem/AddBudgetLineItem`, payload);
  }

  updateBudgetLineItem(lineItem: BudgetLineItems): Observable<BudgetLineItems> {
    const payload = {
      budgetLineItemId: lineItem.budgetLineItemId,
      budgetId: lineItem.budgetId,
      transactionId: lineItem.transactionId ?? null,
      pendingTransactionId: lineItem.pendingTransactionId ?? null,
      date: lineItem.date,
      value: lineItem.value,
      name: lineItem.name,
      merchantName: lineItem.merchantName,
      pending: lineItem.pending,
      categoryId: lineItem.categoryId,
      plaidAccountId: lineItem.plaidAccountId ?? null,
      userId: lineItem.userId,
      sourceTypeId: lineItem.sourceTypeId
    };
    return this.http.put<BudgetLineItems>(`${env.BASE_URL}/BudgetLineItem/UpdateBudgetLineItem`, payload);
  }

  deleteBudgetLineItem(id: number): Observable<void> {
    return this.http.delete<void>(`${env.BASE_URL}/BudgetLineItem/DeleteBudgetLineItem/${id}`);
  }
}