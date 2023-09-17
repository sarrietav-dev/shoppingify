import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/core/services/auth/auth.service';

@Component({
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  constructor(private authService: AuthService, private router: Router) {}

  async signInWithGoogle() {
    await this.authService.signInWithGoogle();
    this.redirect()
  }

  async signInWithGithub() {
    await this.authService.signInWithGithub();
    this.redirect()
  }

  private redirect(): void {
    this.router.navigate(['/']);
  }
}
