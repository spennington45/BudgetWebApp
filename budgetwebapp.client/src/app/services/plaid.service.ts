import { Injectable } from '@angular/core';

declare var Plaid: any;

@Injectable({
  providedIn: 'root'
})

export class PlaidService {
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