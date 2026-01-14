import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AuthService } from '../../services/auth.service';
import { LoginComponent } from '../login/login.component';
import { User } from '../../models';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {
  user: User | null = null;

  constructor(
    private dialog: MatDialog,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    // Subscribe to reactive user updates
    this.authService.currentUser$.subscribe((user: User | null) => {
      this.user = user;
    });
  }

  login(): void {
    this.dialog.open(LoginComponent, {
      disableClose: false,
      autoFocus: true
    });
  }

  logout(): void {
    this.authService.logout();
  }
}