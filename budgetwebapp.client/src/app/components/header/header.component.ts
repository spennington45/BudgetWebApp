import { Component, OnInit } from '@angular/core';
import { UserService } from '../../services/user.service'; 
import { AuthService } from '../../services/auth.service';
import { LoginComponent } from '../login/login.component';
import { User } from '../../models';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {
  user: User | null = null;

  constructor(private dialog: MatDialog, private authService: AuthService, private userService: UserService) {}

  ngOnInit(): void {
    // this.user = this.userService.getUserById(this.user.id);
  }

  login(): void {
    this.dialog.open(LoginComponent, {
      //width: '500px',
      disableClose: false,   
      autoFocus: true
    });
  }

  logout(): void {
    this.authService.logout();
  }
}