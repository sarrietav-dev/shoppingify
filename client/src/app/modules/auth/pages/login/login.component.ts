import { Component } from '@angular/core';
import { AuthService } from 'src/app/core/services/auth/auth.service';

@Component({
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  constructor(private authService: AuthService) {}

  signInWithGoogle() {
    this.authService.signInWithGoogle();
  }

  signInWithGithub() {
    this.authService.signInWithGithub();
  }
}
