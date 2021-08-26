import { Component, OnInit } from '@angular/core';
import { AuthErrorHandlerService } from '../shared/auth-error-handler.service';

@Component({
  selector: 'app-auth-errors',
  templateUrl: './auth-errors.component.html',
  styleUrls: ['./auth-errors.component.scss'],
})
export class AuthErrorsComponent implements OnInit {
  constructor(public service: AuthErrorHandlerService) {}

  ngOnInit(): void {}
}
