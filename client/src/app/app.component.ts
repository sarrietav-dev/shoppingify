import { Component, HostListener, OnInit } from '@angular/core';
import { Product } from './core/models/Product';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  title = 'client';
  cartOpen: boolean | undefined = true;
  scrollOffset = 0;
  product: Product = {
    id: '1',
    category: 'Mango and thing',
    name: 'Mango',
    image: 'https://images.unsplash.com/photo-1553279768-865429fa0078',
    note: `
      Lorem ipsum dolor sit amet consectetur adipisicing elit. Illum eligendi
      molestias vitae! Sit iusto molestiae distinctio nesciunt omnis sunt.
      Maiores ea ipsum saepe! Placeat saepe voluptates maiores quam vel totam?
      Aperiam ipsa obcaecati ratione. Hic ut deserunt laborum, vero temporibus
      sunt ipsa, vel laudantium voluptates odio adipisci perferendis nihil.
      Esse, consequatur facilis eligendi ipsa vel laboriosam cupiditate sint et`,
  };

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
