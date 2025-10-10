import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { User } from '../models';
import { Observable } from 'rxjs';
import { environment as env } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: HttpClient) {}

  getUserById(id: number): Observable<User> {
    return this.http.get<User>(`${env.BASE_URL}/User/GetUserById/${id}`);
  }

  addUser(user: User): Observable<User> {
    return this.http.post<User>(`${env.BASE_URL}/User/AddUser`, user);
  }

  updateUser(user: User): Observable<User> {
    return this.http.put<User>(`${env.BASE_URL}/User/UpdateUser`, user);
  }
}