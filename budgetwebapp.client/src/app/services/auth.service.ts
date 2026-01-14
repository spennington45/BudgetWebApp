import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { jwtDecode } from 'jwt-decode';
import { User, Jwt } from '../models';
import { environment as env } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly TOKEN_KEY = 'jwt';

  private currentUserSubject = new BehaviorSubject<User | null>(null);
  currentUser$ = this.currentUserSubject.asObservable();

  constructor(
    private router: Router,
    private http: HttpClient
  ) {
    this.restoreUserFromToken();
  }

  getToken(): string | null {
    return sessionStorage.getItem(this.TOKEN_KEY);
  }

  setToken(token: string): void {
    sessionStorage.setItem(this.TOKEN_KEY, token);
  }

  clearToken(): void {
    sessionStorage.removeItem(this.TOKEN_KEY);
  }

  getDecodedToken(): Jwt | null {
    const token = this.getToken();
    if (!token) return null;

    try {
      return jwtDecode<Jwt>(token);
    } catch {
      return null;
    }
  }

  private restoreUserFromToken(): void {
    const decoded = this.getDecodedToken();
    if (!decoded) {
      this.currentUserSubject.next(null);
      return;
    }

    const restoredUser: User = {
      userId: Number(decoded.userId),
      externalId: decoded.externalId,
      name: decoded.name,
      email: decoded.email,
      pictureUrl: decoded.picture,
      token: this.getToken() ?? ''
    };

    this.currentUserSubject.next(restoredUser);
  }

  handleCredentialResponse(response: any): void {
    const idToken = response.credential;

    this.http.post<{ token: string }>(
      `${env.BASE_URL}/GoogleLogin/auth/google`,
      { idToken }
    )
    .subscribe({
      next: (result) => {
        const jwt = result.token;
        this.setToken(jwt);

        const decoded = jwtDecode<Jwt>(jwt);

        const user: User = {
          userId: Number(decoded.userId),
          externalId: decoded.externalId,
          name: decoded.name,
          email: decoded.email,
          pictureUrl: decoded.picture,
          token: jwt
        };

        this.currentUserSubject.next(user);
        this.router.navigate(['/']);
      },
      error: (err) => {
        console.error('Google login failed:', err);
      }
    });
  }

  loginWithGoogle(idToken: string): Observable<any> {
    return this.http.post(`${env.BASE_URL}/GoogleLogin/auth/google`, { idToken });
  }

  getGoogleClientId(): Observable<string> {
    return this.http.get<string>(`${env.BASE_URL}/GoogleLogin/ClientId`, {
      responseType: 'text' as 'json'
    });
  }

  logout(): void {
    this.clearToken();
    this.currentUserSubject.next(null);
    this.router.navigate(['/']);
  }

  isLoggedIn(): boolean {
    const decoded = this.getDecodedToken();
    if (!decoded) return false;

    return decoded.exp * 1000 > Date.now();
  }
}