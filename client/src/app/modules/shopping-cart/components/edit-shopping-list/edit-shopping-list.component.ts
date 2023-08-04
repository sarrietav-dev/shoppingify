import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Component({
  selector: 'app-edit-shopping-list',
  templateUrl: './edit-shopping-list.component.html',
  styleUrls: ['./edit-shopping-list.component.css'],
})
export class EditShoppingListComponent implements OnInit {
  listState: 'editing' | 'completing' = 'completing';
  @Output('changeState') changeStateEvent =
    new EventEmitter<ShoppingCartState>();

  items$ = new BehaviorSubject<
    {
      category: string;
      items: { id: string; name: string; amount: number; isChecked: boolean }[];
    }[]
  >([
    {
      category: 'Fruits',
      items: [
        { id: '1', isChecked: false, name: 'Apple', amount: 1 },
        { id: '2', isChecked: false, name: 'Banana', amount: 2 },
        { id: '3', isChecked: false, name: 'Orange', amount: 3 },
      ],
    },
    {
      category: 'Vegetables',
      items: [
        { id: '4', isChecked: false, name: 'Tomato', amount: 4 },
        { id: '5', isChecked: false, name: 'Potato', amount: 5 },
        { id: '6', isChecked: false, name: 'Onion', amount: 6 },
      ],
    },
    {
      category: 'Meat',
      items: [
        { id: '7', isChecked: false, name: 'Chicken', amount: 7 },
        { id: '8', isChecked: false, name: 'Beef', amount: 8 },
        { id: '9', isChecked: false, name: 'Pork', amount: 9 },
      ],
    },
  ]);

  items: {
    category: string;
    items: { id: string; name: string; amount: number; isChecked: boolean }[];
  }[] = [];

  ngOnInit() {
    this.items$.subscribe((items) => (this.items = items));
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
    isChecked: boolean;
    category: string;
    index: number;
  } | null {
    for (const [index, category] of this.items.entries()) {
      const item = category.items.find((i) => i.id === id);
      if (item) return { ...item, category: category.category, index };
    }

    return null;
  }

  private getCategory(
    categoryName: string
  ): { index: number; category: string } | null {
    for (const [index, category] of this.items.entries()) {
      if (category.category === categoryName)
        return { index, category: category.category };
    }

    return null;
  }

  changeState(event: ShoppingCartState) {
    this.changeStateEvent.emit(event);
  }
}
