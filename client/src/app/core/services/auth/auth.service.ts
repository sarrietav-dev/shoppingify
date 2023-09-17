import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Auth, GithubAuthProvider, GoogleAuthProvider, signInWithPopup, signInWithRedirect } from '@angular/fire/auth';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private fbAuth: Auth, private http: HttpClient) { }

  async signInWithGoogle() {
    const userCredentials = await signInWithPopup(this.fbAuth, new GoogleAuthProvider());
  }

  async signInWithGoogleRedirect() {
    const userCredentials = await signInWithRedirect(this.fbAuth, new GoogleAuthProvider());
  }

  async signInWithGithub() {
    const userCredentials = await signInWithPopup(this.fbAuth, new GithubAuthProvider());
  }

  async signOut() {
    await this.fbAuth.signOut();
  }

  get user() {
    return this.fbAuth.currentUser;
  }
}
