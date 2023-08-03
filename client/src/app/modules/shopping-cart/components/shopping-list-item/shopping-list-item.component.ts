import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-shopping-list-item',
  templateUrl: './shopping-list-item.component.html',
  styleUrls: ['./shopping-list-item.component.css'],
})
export class ShoppingListItemComponent {
  @Input() name!: string;
  @Input() amount: number = 0;
  @Input() id: string = '';

  @Output('increment') incrementEvent = new EventEmitter<string>();
  @Output('decrement') decrementEvent = new EventEmitter<string>();
  @Output('delete') deleteEvent = new EventEmitter<string>();

  isEditing = false;

  toggleEdit() {
    this.isEditing = !this.isEditing;
  }

  increment() {
    this.amount++;
    this.incrementEvent.emit(this.id ?? 'id');
  }

  decrement() {
    if (this.amount - 1 < 1) return;

    this.amount--;
    this.decrementEvent.emit(this.id ?? 'id');
  }

  delete() {
    this.deleteEvent.emit(this.id ?? 'id');
  }
}
