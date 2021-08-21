import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http'
import { AuthenticateRequest } from './authenticate-request.model';
import { AuthenticateResponse } from './authenticate-response.model';


@Injectable({
  providedIn: 'root'
})
export class UsersService {

  constructor(private http: HttpClient) { }

  readonly baseUrl = 'http://localhost/news-scrapper.api/users/';

  formData: AuthenticateRequest = new AuthenticateRequest();

  token: string='';

  postLoginDetails(){
    return this.http.post<AuthenticateResponse>(this.baseUrl+'authenticate', this.formData);
  }

  getUsers(){
    return this.http.get(this.baseUrl);
  }
}
