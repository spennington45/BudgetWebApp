import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Budget } from '../models';
import { lastValueFrom, Observable } from 'rxjs';
import { environment as env } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class BudgetService {

  constructor(private http: HttpClient) { }

  async getBudgetByUserId(userId: number): Promise<Budget[]> {
    return await lastValueFrom(
      this.http.get<Budget[]>(`${env.BASE_URL}/Budget/GetBudgetByUserId/${userId}`)
    );
  }

  getBudgetByBudgetId(budgetId: number): Observable<Budget> {
    return this.http.get<Budget>(`${env.BASE_URL}/Budget/GetBudgetByBudgetId/${budgetId}`);
  }

  updateBudget(budget: Budget): Observable<Budget> {
    return this.http.put<Budget>(`${env.BASE_URL}/Budget/UpdateBudget`, budget);
  }

  addBudget(budget: Budget): Observable<Budget> {
    const payload = {
      userId: budget.user.userId,
      date: budget.date,
      budgetLineItems: [],
      user: budget.user,
    }
    return this.http.post<Budget>(`${env.BASE_URL}/Budget/AddBudget`, payload);
  }

  deleteBudget(budgetId: number): Observable<void> {
    return this.http.delete<void>(`${env.BASE_URL}/Budget/DeleteBudget/${budgetId}`);
  }
}