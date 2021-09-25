import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthErrorHandlerService } from '../shared/auth-error-handler.service';
import { UsersService } from '../shared/users.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {
  constructor(
    public service: UsersService,
    private router: Router,
    private authErrorHandlerService: AuthErrorHandlerService
  ) {}

  ngOnInit(): void {}

  errors: string[] = [];

  private clearErrors() {
    this.errors = [];
  }

  onSubmit(form: NgForm) {
    this.clearErrors();
    this.service.postLoginDetails().subscribe(
      (res) => {
        this.service.Token = res.jwtToken;
        this.service.Id = res.id;
        this.authErrorHandlerService.ClearErrors();
        this.router.navigate(['']);
      },
      (err) => {
        this.errors.push(err.error.message);
      }
    );
  }
}
