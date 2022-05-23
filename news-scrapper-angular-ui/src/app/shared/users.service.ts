import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthenticateRequest } from './authenticate-request.model';
import { AuthenticateResponse } from './authenticate-response.model';
import { environment } from 'src/environments/environment';
import { User } from './user.model';
import * as moment from 'moment';
import { Observable, of } from 'rxjs';

const TOKEN_LOCAL_STORAGE_KEY: string = 'token';
const EXPIRATION_LOCAL_STORAGE_KEY: string = 'expires_at';

export interface LoginResponse {
  success: boolean;
  error: string;
}
@Injectable({
  providedIn: 'root',
})
export class UsersService {
  constructor(private http: HttpClient) {}

  readonly baseUrl = environment.apiUrl + 'users/';

  formData: AuthenticateRequest = new AuthenticateRequest();

  private token: string = '';
  private id: number = 0;

  postLoginDetails(): Observable<LoginResponse> {
    let result: LoginResponse = {
      success: false,
      error: '',
    };

    this.http
      .post<AuthenticateResponse>(this.baseUrl + 'authenticate', this.formData)
      .subscribe(
        (res) => {
          this.setSession(res);
          this.token = res.jwtToken;
          this.id = res.id;

          result.success = true;
        },
        (err) => {
          result.success = false;
          result.error = err.error.message;
        }
      );
    return of(result);
  }

  isLoggedIn() {
    return moment().isBefore(this.getExpiration());
  }

  logout() {
    localStorage.removeItem(TOKEN_LOCAL_STORAGE_KEY);
    localStorage.removeItem(EXPIRATION_LOCAL_STORAGE_KEY);
    this.token = '';
  }

  GetCurrentUserDetails() {
    return this.http.get<User>(this.baseUrl + this.id);
  }

  getUsers() {
    return this.http.get(this.baseUrl);
  }

  getExpiration() {
    const expiration = localStorage.getItem(EXPIRATION_LOCAL_STORAGE_KEY);
    const expiresAt = JSON.parse(expiration ?? '{}');
    return moment(expiresAt);
  }

  getToken(): string {
    if (!this.token || this.token.length < 0) {
      this.token = localStorage.getItem(TOKEN_LOCAL_STORAGE_KEY) ?? '';
    }
    return this.token;
  }

  RevokeToken(token: string) {
    return this.http.post(this.baseUrl + 'revoke-token', {
      token: token,
    });
  }

  public setSession(authResult: AuthenticateResponse) {
    const expiresAt = moment().add(authResult.expiresInDays, 'days');

    localStorage.setItem(TOKEN_LOCAL_STORAGE_KEY, authResult.jwtToken);
    localStorage.setItem(
      EXPIRATION_LOCAL_STORAGE_KEY,
      JSON.stringify(expiresAt.valueOf())
    );
  }
}
