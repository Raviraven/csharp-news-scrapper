import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UsersService } from '../shared/users.service';

import { catchError } from 'rxjs/operators';
import { Observable, throwError } from 'rxjs';
import { Router } from '@angular/router';
import { AuthErrorHandlerService } from '../shared/auth-error-handler.service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(
    private auth: UsersService,
    private router: Router,
    private authErrorHandlerService: AuthErrorHandlerService
  ) {}

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    const authToken = this.auth.Token;

    const authReq = req.clone({
      headers: req.headers.set('Authorization', authToken),
    });

    return next.handle(authReq).pipe(
      catchError((error: HttpErrorResponse) => {
        let errorMsg = this.buildErrorConsoleMessage(error);
        this.handleError(error);
        return throwError(errorMsg);
      })
    );
  }

  private buildErrorConsoleMessage(error: HttpErrorResponse) {
    let errorMsg = '';

    if (error.error instanceof ErrorEvent) {
      //console.log('this is client side error');
      errorMsg = `Error: ${error.error.message}`;
    } else {
      //console.log('this is server side error');
      errorMsg = `Error Code: ${error.status},  Message: ${error.message}`;
    }
    return errorMsg;
  }

  private handleError(error: HttpErrorResponse) {
    if (error.status > 401 && error.status < 500) return;

    this.authErrorHandlerService.ClearErrors();
    this.authErrorHandlerService.AddError(
      `${error.status.toString()} - ${error.statusText}`
    );

    this.router.navigate(['/login']);
  }
}
