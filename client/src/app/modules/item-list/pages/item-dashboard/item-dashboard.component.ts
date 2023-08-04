import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { Product } from 'src/app/core/models/Product';
import { ProductsService } from 'src/app/core/services/products/products.service';

@Component({
  selector: 'item-dashboard',
  templateUrl: './item-dashboard.component.html',
  styleUrls: ['./item-dashboard.component.css'],
})
export class ItemDashboardComponent implements OnInit {
  products: ProductsGroupedByCategory[] = [];
  @Output() itemClicked = new EventEmitter<Product>();

  constructor(private productService: ProductsService) {}

  ngOnInit(): void {
    this.productService.getProducts().subscribe((products) => {
      this.products = this.groupProductsByCategory(products);
    });
  }

  onItemClicked(product: Product) {
    this.itemClicked.emit(product);
  }

  groupProductsByCategory(products: Product[]): ProductsGroupedByCategory[] {
    const groupedProducts = products.reduce((groupedProducts, product) => {
      const category = product.category;
      if (groupedProducts[category] == null) {
        groupedProducts[category] = [];
      }
      groupedProducts[category].push(product);
      return groupedProducts;
    }, {} as { [key: string]: Product[] });

    return Object.keys(groupedProducts).map((category) => {
      return {
        category,
        products: groupedProducts[category],
      };
    });
  }
}

type ProductsGroupedByCategory = {
  category: string;
  products: Product[];
};
