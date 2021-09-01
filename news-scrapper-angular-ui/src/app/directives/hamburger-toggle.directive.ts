import { Directive, HostBinding, HostListener } from '@angular/core';
import { Router, Event, NavigationStart } from '@angular/router';

@Directive({
  selector: '[appHamburgerToggle]',
})
export class HamburgerToggleDirective {
  @HostBinding('class.is-active')
  private isActive = false;

  @HostListener('click')
  toggleActive(): void {
    this.isActive = !this.isActive;
  }

  constructor(private router: Router) {
    router.events.subscribe((event: Event) => {
      if (event instanceof NavigationStart) {
        this.isActive = false;
      }
    });
  }
}
