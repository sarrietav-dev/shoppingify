import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Product } from 'src/app/core/models/Product';

@Component({
  selector: 'app-product-overview',
  templateUrl: './product-overview.component.html',
  styleUrls: ['./product-overview.component.css'],
})
export class ProductOverviewComponent {
  @Input() product!: Product;
  @Output() backClick = new EventEmitter<void>();
  @Output() addToCartClick = new EventEmitter<Product>();
  @Output() deleteClick = new EventEmitter<Product>();

  handleToCartClick() {
    this.addToCartClick.emit(this.product);
  }

  handleDeleteClick() {
    this.deleteClick.emit(this.product);
  }
}
