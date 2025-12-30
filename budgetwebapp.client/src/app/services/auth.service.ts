import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { User, Jwt } from '../models';
import { HttpClient } from '@angular/common/http';
import { environment as env } from '../../environments/environment';
import { Observable } from 'rxjs';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly USER_KEY = 'currentUser';

  constructor(private router: Router, private http: HttpClient) {}

  getToken(): string | null {
    return localStorage.getItem('jwt');
  }

  getDecodedToken(): Jwt | null {
    const token = this.getToken();
    if (!token) return null;

    return jwtDecode<Jwt>(token);
  }

  getCurrentUserId(): string | null {
    return this.getDecodedToken()?.userId ?? null;
  }

  getCurrentUserName(): string | null {
    return this.getDecodedToken()?.name ?? null;
  }

  getCurrentUserPicture(): string | null {
    return this.getDecodedToken()?.picture ?? null;
  }

  isLoggedIn(): boolean {
    const token = this.getDecodedToken();
    if (!token) return false;

    return token.exp * 1000 > Date.now();
  }

  // Update may not need
  getCurrentUser(): User | null {
    const userJson = localStorage.getItem(this.USER_KEY);
    return userJson ? JSON.parse(userJson) : null;
  }


  handleCredentialResponse(response: any) {
    const idToken = response.credential;

    this.http.post<{ token: string }>('/GoogleLogin/auth/google', { idToken })
      .subscribe({
        next: (result) => {
          const jwt = result.token;

          sessionStorage.setItem('jwt', jwt);

          const decoded: any = jwtDecode(jwt);
          console.log('Logged in user:', decoded);

          this.router.navigate(['/']);
        },
        error: (err) => {
          console.error('Google login failed:', err);
        }
      });
  }

  logout(): void {
    sessionStorage.removeItem('jwt');   
    this.router.navigate(['/']);   
  }

  loginWithGoogle(idToken: string) {
    return this.http.post(`${env.BASE_URL}/GoogleLogin/auth/google`, { idToken });
  }

  getGoogleClientId(): Observable<string> {
    return this.http.get<string>(`${env.BASE_URL}/GoogleLogin/ClientId`, {
      responseType: 'text' as 'json'
    });
  }
}