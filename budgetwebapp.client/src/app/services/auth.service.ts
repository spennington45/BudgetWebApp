import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

export interface User {
  name: string;
  email: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly USER_KEY = 'currentUser';

  constructor(private router: Router) {}

  // replace with API call
  login(): void {
    const mockUser: User = {
      name: 'Jane Doe',
      email: 'jane.doe@example.com'
    };
    localStorage.setItem(this.USER_KEY, JSON.stringify(mockUser));
    this.router.navigate(['/home']);
  }

  // Logout and clear session
  logout(): void {
    localStorage.removeItem(this.USER_KEY);
    this.router.navigate(['/login']);
  }

  // Update may not need
  getCurrentUser(): User | null {
    const userJson = localStorage.getItem(this.USER_KEY);
    return userJson ? JSON.parse(userJson) : null;
  }

  // Check if user is logged in
  isLoggedIn(): boolean {
    return !!this.getCurrentUser();
  }
}