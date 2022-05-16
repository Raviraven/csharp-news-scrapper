import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthenticateRequest } from './authenticate-request.model';
import { AuthenticateResponse } from './authenticate-response.model';
import { environment } from 'src/environments/environment';
import { User } from './user.model';
import * as moment from 'moment';

const TOKEN_LOCAL_STORAGE_KEY: string = 'token';
const EXPIRATION_LOCAL_STORAGE_KEY: string = 'expires_at';
@Injectable({
  providedIn: 'root',
})
export class UsersService {
  constructor(private http: HttpClient) {}

  readonly baseUrl = environment.apiUrl + 'users/';

  formData: AuthenticateRequest = new AuthenticateRequest();

  Token: string = '';
  Id: number = 0;

  postLoginDetails() {
    return this.http.post<AuthenticateResponse>(
      this.baseUrl + 'authenticate',
      this.formData
    );
  }

  isLoggedIn() {
    console.log('isLoggedIn');
    console.log(moment().isBefore(this.getExpiration()));
    return moment().isBefore(this.getExpiration());
  }

  logout() {
    localStorage.removeItem(TOKEN_LOCAL_STORAGE_KEY);
    localStorage.removeItem(EXPIRATION_LOCAL_STORAGE_KEY);
  }

  GetCurrentUserDetails() {
    return this.http.get<User>(this.baseUrl + this.Id);
  }

  getUsers() {
    return this.http.get(this.baseUrl);
  }

  getExpiration() {
    const expiration = localStorage.getItem(EXPIRATION_LOCAL_STORAGE_KEY);
    const expiresAt = JSON.parse(expiration ?? '{}');
    return moment(expiresAt);
  }

  RevokeToken(token: string) {
    return this.http.post(this.baseUrl + 'revoke-token', {
      token: token,
    });
  }

  public setSession(authResult: AuthenticateResponse) {
    console.log(moment());
    const expiresAt = moment().add(authResult.expiresInDays, 'days');

    localStorage.setItem(TOKEN_LOCAL_STORAGE_KEY, authResult.jwtToken);
    localStorage.setItem(
      EXPIRATION_LOCAL_STORAGE_KEY,
      JSON.stringify(expiresAt.valueOf())
    );
  }
}
