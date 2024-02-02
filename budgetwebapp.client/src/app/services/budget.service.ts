import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Budget, BudgetLineItems } from '../models';
import { Observable } from 'rxjs';
import { environment as env } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class BudgetService {

  constructor(private http: HttpClient) { }

  getBudgetByUserId(id: string): Observable<Array<Budget>>{
    return this.http.get<Budget[]>(`${env.BASE_URL}/Budget/GetBudgetByUserId/${id}`);
  }

  getBudgetLineItemsByBudgetId(id: string): Observable<Array<BudgetLineItems>> {
    return this.http.get<BudgetLineItems[]>(`${env.BASE_URL}/Budget/GetBudgetLineItemsByBudgetId/${id}`);
  }
}
