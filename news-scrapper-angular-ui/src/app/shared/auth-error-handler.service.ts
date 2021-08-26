import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class AuthErrorHandlerService {
  constructor() {}

  private errors: string[] = [];

  AddError(error: string) {
    this.errors.push(error);
  }

  GetErrors() {
    return this.errors;
  }

  ClearErrors() {
    this.errors = [];
  }
}
