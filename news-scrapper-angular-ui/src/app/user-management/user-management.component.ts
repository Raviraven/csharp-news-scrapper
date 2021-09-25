import { Component, OnInit } from '@angular/core';
import { User } from '../shared/user.model';
import { UsersService } from '../shared/users.service';

@Component({
  selector: 'app-user-management',
  templateUrl: './user-management.component.html',
  styleUrls: ['./user-management.component.scss'],
})
export class UserManagementComponent implements OnInit {
  constructor(public userService: UsersService) {}

  UserDetails!: User;
  Token: string = '';

  ngOnInit(): void {
    this.userService.GetCurrentUserDetails().subscribe(
      (res) => {
        this.UserDetails = res;
      },
      (err) => {}
    );
  }

  OnSubmit() {
    this.userService.RevokeToken(this.Token).subscribe(
      (res) => {
        this.Token = '';
      },
      (err) => {}
    );
  }
}
