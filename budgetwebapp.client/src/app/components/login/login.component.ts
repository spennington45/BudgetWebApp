import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { environment } from '../../../environments/environment';
import { MatDialogRef } from '@angular/material/dialog';

declare const google: any;

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  constructor(private dialogRef: MatDialogRef<LoginComponent>, private authService: AuthService) {}

ngOnInit(): void {
  this.authService.getGoogleClientId().subscribe(clientId => {
    google.accounts.id.initialize({
      client_id: clientId,
      callback: (response: any) => this.handleCredentialResponse(response)
    });
    console.log(clientId);
    google.accounts.id.renderButton(
      document.getElementById('googleBtn'),
      { theme: 'outline', size: 'large', width: '100%' }
    );
  });
}

  close(): void {
    this.dialogRef.close();
  }

  handleCredentialResponse(response: any) {
    const idToken = response.credential;

    this.authService.loginWithGoogle(idToken).subscribe({
      next: (result: any) => {
        localStorage.setItem('authToken', result.token);
        this.dialogRef.close();
      },
      error: (err) => {
        console.error('Google login failed', err);
      }
    });
  }
}