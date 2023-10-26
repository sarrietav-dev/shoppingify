import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {environment} from 'src/environments/environment';
import {Product} from '../../models/Product';

@Injectable({
  providedIn: 'root',
})
export class ProductsService {
  private baseUrl = `${environment.baseUrl}/products`;

  constructor(private http: HttpClient) {
  }

  getProducts() {
    return this.http.get<Product[]>(this.baseUrl);
  }

  getProduct(id: string) {
    return this.http.get<Product>(`${this.baseUrl}/${id}`);
  }

  createProduct(product: Omit<Product, 'id'>) {
    return this.http.post<Product>(this.baseUrl, product);
  }

  getCategories() {
    return this.http.get<string[]>(`${this.baseUrl}/api/products/categories`);
  }

  deleteProduct(id: string) {
    return this.http.delete(`${this.baseUrl}/${id}`);
  }
}
