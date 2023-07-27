import { Component, Input } from '@angular/core';
import { Product } from 'src/app/core/models/Product';

@Component({
  selector: 'app-product-overview',
  templateUrl: './product-overview.component.html',
  styleUrls: ['./product-overview.component.css'],
})
export class ProductOverviewComponent {
  @Input() product!: Product;
}
