import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthenticateRequest } from './authenticate-request.model';
import { AuthenticateResponse } from './authenticate-response.model';
import { environment } from 'src/environments/environment';
import { User } from './user.model';

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

  GetCurrentUserDetails() {
    return this.http.get<User>(this.baseUrl + this.Id);
  }

  getUsers() {
    return this.http.get(this.baseUrl);
  }

  RevokeToken(token: string) {
    return this.http.post(this.baseUrl + 'revoke-token', {
      token: token,
    });
  }
}
