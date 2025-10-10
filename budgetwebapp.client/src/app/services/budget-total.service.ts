import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BudgetTotal } from '../models';
import { Observable } from 'rxjs';
import { environment as env } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class BudgetTotalService {

  constructor(private http: HttpClient) {}

  getBudgetTotalByUserId(userId: number): Observable<BudgetTotal> {
    return this.http.get<BudgetTotal>(`${env.BASE_URL}/BudgetTotal/GetBudgetTotalByUserId/${userId}`);
  }

  updateBudgetTotal(budgetTotal: BudgetTotal): Observable<BudgetTotal> {
    return this.http.put<BudgetTotal>(`${env.BASE_URL}/BudgetTotal/UpdateBudgetTotal`, budgetTotal);
  }

  addBudgetTotal(budgetTotal: BudgetTotal): Observable<BudgetTotal> {
    return this.http.post<BudgetTotal>(`${env.BASE_URL}/BudgetTotal/AddBudgetTotal`, budgetTotal);
  }
}