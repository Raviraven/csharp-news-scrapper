import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UsersService } from '../shared/users.service';

@Component({
  selector: 'app-logout',
  templateUrl: './logout.component.html',
  styleUrls: ['./logout.component.scss'],
})
export class LogoutComponent implements OnInit {
  constructor(private usersService: UsersService, private router: Router) {}

  ngOnInit(): void {
    this.usersService.logout();
    this.redirect();
  }

  private async redirect() {
    await new Promise((f) => setTimeout(f, 3000));
    this.router.navigate(['/']);
  }
}
