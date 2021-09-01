import { Component, OnInit } from '@angular/core';
import { Router, Event, NavigationStart } from '@angular/router';
import { UsersService } from '../shared/users.service';

@Component({
  selector: 'app-sidenav',
  templateUrl: './sidenav.component.html',
  styleUrls: ['./sidenav.component.scss'],
})
export class SidenavComponent implements OnInit {
  private _navigationActive: any = undefined;

  public get NavigationActive(): boolean {
    if (this._navigationActive === undefined) return false;

    return this._navigationActive;
  }
  public set NavigationActive(value: boolean) {
    this._navigationActive = value;
  }

  constructor(public usersService: UsersService, private router: Router) {
    router.events.subscribe((event: Event) => {
      if (event instanceof NavigationStart) {
        this._navigationActive = false;
      }
    });
  }

  ngOnInit(): void {}

  toggleNavigation() {
    if (this._navigationActive === undefined) this._navigationActive = false;

    this._navigationActive = !this._navigationActive;
  }
}
