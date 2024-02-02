import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { APIResponse, Budget } from '../models';
import { forkJoin, Observable } from 'rxjs';
import { environment as env } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class BudgetService {

  constructor(private http: HttpClient) { }

  getBudgetByUserId(id: string): Observable<Array<Budget>>{
    return this.http.get<Budget[]>(`${env.BASE_URL}/Budget/GetBudgetByUserId/${id}`);
  }

  getBudgetDetails(id: string): Observable<Budget> {
    return this.http.get<Budget>(`${env.BASE_URL}/Budget/GetBudgetByBudgetId/${id}`);
  }
}
