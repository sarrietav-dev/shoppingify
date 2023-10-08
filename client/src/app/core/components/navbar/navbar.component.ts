import { Component, EventEmitter, Output } from '@angular/core';
import { AuthService } from '../../services/auth/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css'],
})
export class NavbarComponent {
  @Output() cartClicked = new EventEmitter();

  constructor(private authService: AuthService, private router: Router) {}

  onCartClicked() {
    this.cartClicked.emit();
  }

  async onLogoutClicked() {
    await this.authService.signOut();
    this.router.navigate(['/login']);
  }
}
