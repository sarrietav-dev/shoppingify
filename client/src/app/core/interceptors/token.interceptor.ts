import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
} from '@angular/common/http';
import { Observable, from, lastValueFrom } from 'rxjs';
import { AuthService } from '../services/auth/auth.service';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {
  constructor(private authService: AuthService) {}

  intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler
  ): Observable<HttpEvent<unknown>> {
    if (!request.url.includes('api')) {
      return next.handle(request);
    }

    return from(this.handle(request, next));
  }

  async handle(req: HttpRequest<unknown>, next: HttpHandler): Promise<HttpEvent<unknown>> {
    const authToken = await this.authService.getToken();

    const authRequest = req.clone({
      setHeaders: {
        Authorization: `Bearer ${authToken}`,
      },
    });

    return lastValueFrom(next.handle(authRequest));
  }
}
