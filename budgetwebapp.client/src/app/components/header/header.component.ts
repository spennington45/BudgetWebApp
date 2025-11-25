import { Component, OnInit } from '@angular/core';
import { UserService } from '../../services/user.service'; 
import { AuthService } from '../../services/auth.service';
import { User } from '../../models';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {
  user: User | null = null;

  constructor(private authService: AuthService, private userService: UserService) {}

  ngOnInit(): void {
    // this.user = this.userService.getUserById(this.user.id);
  }

  login(): void {
    this.authService.login();
  }

  logout(): void {
    this.authService.logout();
  }
}