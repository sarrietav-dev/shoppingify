import { Component } from '@angular/core';
import { Product } from 'src/app/core/models/Product';
import { ProductsService } from 'src/app/core/services/products/products.service';

@Component({
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css']
})
export class MainComponent {
  constructor(private productService: ProductsService) {}

  cartOpen: boolean | undefined = undefined;
  productOverviewOpen: boolean | undefined = undefined;
  scrollOffset = 0;
  product: Product = {
    id: '',
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

    if (this.productOverviewOpen !== undefined && this.productOverviewOpen) {
      this.productOverviewOpen = false;
    }
  }

  handleProductOverviewClick(product: Product) {
    if (this.productOverviewOpen !== true) {
      this.productOverviewOpen = !this.productOverviewOpen ?? true;
    }

    this.product = product;

    if (this.cartOpen !== undefined && this.cartOpen) {
      this.cartOpen = false;
    }
  }

  handleBackClick() {
    this.productOverviewOpen = false;
  }

  handleDeleteClick(product: Product) {
    this.productService
      .deleteProduct(product.id)
      .subscribe(() => (this.productOverviewOpen = false));
  }
}
