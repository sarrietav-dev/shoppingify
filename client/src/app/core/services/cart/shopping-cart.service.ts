import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {BehaviorSubject} from 'rxjs';
import {environment} from 'src/environments/environment';

type Cart = {
  id: string;
  name: string;
  cartItems: CartItem[];
  createdAt: Date;
}

type CartItemStatus = "Checked" | "Unchecked";

type CartItem = {
  quantity: number;
  status: CartItemStatus;
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
  private baseUrl = `${environment.baseUrl}/active-cart`;

  constructor(private http: HttpClient) {
  }

  getActiveCart(id: string) {
    return this.http.get<Cart>(this.baseUrl);
  }

  createActiveCart(cart: Omit<Cart, 'createdAt'>) {
    return this.http.post<{ id: string }>(this.baseUrl, cart);
  }
}
