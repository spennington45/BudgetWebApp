import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { RecurringExpense } from '../models';
import { Observable } from 'rxjs';
import { environment as env } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class RecurringExpenseService {

  constructor(private http: HttpClient) { }

  getRecurringExpensesByUserId(userId: number): Observable<RecurringExpense[]> {
    return this.http.get<RecurringExpense[]>(`${env.BASE_URL}/RecurringExpense/GetRecurringExpensesByUserId/${userId}`);
  }

  getRecurringExpenseById(id: number): Observable<RecurringExpense> {
    return this.http.get<RecurringExpense>(`${env.BASE_URL}/RecurringExpense/GetRecurringExpenseById/${id}`);
  }

  addRecurringExpense(expense: RecurringExpense): Observable<RecurringExpense> {
    return this.http.post<RecurringExpense>(`${env.BASE_URL}/RecurringExpense/AddRecurringExpense`, expense);
  }

  updateRecurringExpense(expense: RecurringExpense): Observable<RecurringExpense> {
    return this.http.put<RecurringExpense>(`${env.BASE_URL}/RecurringExpense/UpdateRecurringExpense`, expense);
  }

  deleteRecurringExpense(id: number): Observable<void> {
    return this.http.delete<void>(`${env.BASE_URL}/RecurringExpense/DeleteRecurringExpense/${id}`);
  }
}