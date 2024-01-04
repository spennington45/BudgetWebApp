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

  getBudgetByUserId(id: string): Observable<APIResponse<Array<Budget>>> {
    //let params = new HttpParams().set('ordering', ordering)
    return this.http.get<APIResponse<Array<Budget>>>(`${env.BASE_URL}/GetBudgetByUserId/${id}`);
  }

  getBudgetDetails(id: string): Observable<APIResponse<Budget>> {
    return this.http.get<APIResponse<Budget>>(`${env.BASE_URL}/GetBudgetByBudgetId/${id}`);
  }
}
