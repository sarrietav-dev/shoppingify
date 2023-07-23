import { Component, HostListener, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  title = 'client';
  cartOpen: boolean | undefined = undefined;
  scrollOffset = 0;

  get cartTopPosition() {
    return this.scrollOffset + 'px';
  }

  handleScroll(event: Event) {
    const target = event.target as HTMLElement;
    this.scrollOffset = target.scrollTop;
  }

  handleNavbarCartClick() {
    this.cartOpen = !this.cartOpen ?? true;
  }
}
