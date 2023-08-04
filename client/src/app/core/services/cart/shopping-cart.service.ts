import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { environment } from 'src/environments/environment';

type Cart = {
  id: string;
  name: string;
  lineItems: LineItem[];
  cartCount: number;
  itemCount: number;
  checkedItems: number;
}

type LineItem = {
  quantity: number;
  isChecked: boolean;
  product: Product;
}

type Product = {
  id: string;
  name: string;
  note: string;
  category: string;
  image: string;
}

@Injectable({
  providedIn: 'root',
})
export class ShoppingCartService {
  private baseUrl = `${environment.baseUrl}/api/ShoppingCart`;

  constructor(private http: HttpClient) {}

  getCart(id: string) {
    return this.http.get<Cart>(this.baseUrl + `/${id}`);
  }
}
