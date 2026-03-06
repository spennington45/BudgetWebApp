import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment as env } from '../../environments/environment';

declare var Plaid: any;

@Injectable({
  providedIn: 'root'
})

export class PlaidService {
  constructor(private http: HttpClient) { }

  createLinkToken(userId: string) {
    return this.http.post<{ link_token: string }>(
      `${env.BASE_URL}/Plaid/create-link-token`,
      JSON.stringify(userId),
      { headers: { 'Content-Type': 'application/json' } }
    );
  }

  exchangePublicToken(publicToken: string) {
    return this.http.post(
      `${env.BASE_URL}/Plaid/exchange`,
      JSON.stringify(publicToken),
      { headers: { 'Content-Type': 'application/json' } }
    );
  }

  openPlaidLink(token: string, onSuccess: (publicToken: string) => void): void {
    const handler = Plaid.create({
      token: token,
      onSuccess: (public_token: string, metadata: any) => {
        console.log('Plaid Link success:', metadata);
        onSuccess(public_token);
      },
      onExit: (err: any, metadata: any) => {
        console.error('Plaid Link exited:', err, metadata);
      }
    });

    handler.open();
  }
}