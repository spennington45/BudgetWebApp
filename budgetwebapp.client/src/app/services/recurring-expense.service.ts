import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { RecurringExpense } from '../models';
import { Observable } from 'rxjs';
import { environment as env } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class RecurringExpenseService {

  constructor(private http: HttpClient) {}

  // GET: Retrieve all recurring expenses for a user
  getRecurringExpensesByUserId(userId: number): Observable<RecurringExpense[]> {
    return this.http.get<RecurringExpense[]>(`${env.BASE_URL}/RecurringExpense/GetRecurringExpensesByUserId/${userId}`);
  }

  // GET: Retrieve a specific recurring expense by ID
  getRecurringExpenseById(id: number): Observable<RecurringExpense> {
    return this.http.get<RecurringExpense>(`${env.BASE_URL}/RecurringExpense/GetRecurringExpenseById/${id}`);
  }

  // POST: Add a new recurring expense
  addRecurringExpense(expense: RecurringExpense): Observable<RecurringExpense> {
    return this.http.post<RecurringExpense>(`${env.BASE_URL}/RecurringExpense/AddRecurringExpense`, expense);
  }

  // PUT: Update an existing recurring expense
  updateRecurringExpense(expense: RecurringExpense): Observable<RecurringExpense> {
    return this.http.put<RecurringExpense>(`${env.BASE_URL}/RecurringExpense/UpdateRecurringExpense`, expense);
  }

  // DELETE: Delete a recurring expense by ID
  deleteRecurringExpense(id: number): Observable<void> {
    return this.http.delete<void>(`${env.BASE_URL}/RecurringExpense/DeleteRecurringExpense/${id}`);
  }
}