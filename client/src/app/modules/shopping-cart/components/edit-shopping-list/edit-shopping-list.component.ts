import {Component, OnInit, Output, EventEmitter} from '@angular/core';
import {BehaviorSubject, map, Observable} from 'rxjs';
import {ShoppingCartService} from "../../../../core/services/cart/shopping-cart.service";

@Component({
  selector: 'app-edit-shopping-list',
  templateUrl: './edit-shopping-list.component.html',
  styleUrls: ['./edit-shopping-list.component.css'],
})
export class EditShoppingListComponent implements OnInit {
  listState: 'editing' | 'completing' = 'completing';
  @Output('changeState') changeStateEvent =
    new EventEmitter<ShoppingCartState>();

  constructor(private cartService: ShoppingCartService) {
  }

  items: Observable<{
    category: string;
    items: { id: string; name: string; amount: number; status: boolean }[];
  }[]> = new BehaviorSubject([]);

  ngOnInit() {
    this.items = this.cartService.getActiveCart().pipe(
      map((cart) => {
        return cart.cartItems.map((cartItem) => {
          return {
            category: cartItem.product.category,
            items: [
              {
                id: cartItem.product.id,
                name: cartItem.product.name,
                amount: cartItem.quantity,
                status: cartItem.status === 'Checked',
              },
            ],
          };
        });
      })
    );
  }

  toggleListState() {
    this.listState = this.listState === 'completing' ? 'editing' : 'completing';
  }

  decrement(id: string) {
    const item = this.getItem(id);
    if (!item) return;

    const category = this.getCategory(item.category);
    if (!category) return;

    if (item.amount - 1 < 1) return;

    this.items[category.index].items[item.index].amount--;

    this.items$.next(this.items);
  }

  increment(id: string) {
    const item = this.getItem(id);
    if (!item) return;

    const category = this.getCategory(item.category);
    if (!category) return;

    this.items[category.index].items[item.index].amount++;

    this.items$.next(this.items);
  }

  delete(id: string) {
    const item = this.getItem(id);
    if (!item) return;

    const category = this.getCategory(item.category);
    if (!category) return;

    const index = this.items[category.index].items.findIndex(
      (i) => i.id === id
    );

    this.items[category.index].items.splice(index, 1);

    if (this.items[category.index].items.length === 0) {
      this.items.splice(category.index, 1);
    }

    this.items$.next(this.items);
  }

  toggleCheck(id: string) {
    const item = this.getItem(id);
    if (!item) return;

    const category = this.getCategory(item.category);
    if (!category) return;

    this.items[category.index].items[item.index].isChecked =
      !this.items[category.index].items[item.index].isChecked;

    this.items$.next(this.items);
  }

  private getItem(id: string): {
    id: string;
    name: string;
    amount: number;
    status: boolean;
    category: string;
    index: number;
  } | null {
    for (const [index, category] of this.items.entries()) {
      const item = category.items.find((i) => i.id === id);
      if (item) return {...item, category: category.category, index};
    }

    return null;
  }

  private getCategory(
    categoryName: string
  ): { index: number; category: string } | null {
    for (const [index, category] of this.items.entries()) {
      if (category.category === categoryName)
        return {index, category: category.category};
    }

    return null;
  }

  changeState(event: ShoppingCartState) {
    this.changeStateEvent.emit(event);
  }
}
