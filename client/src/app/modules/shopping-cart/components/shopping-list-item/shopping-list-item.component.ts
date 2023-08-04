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
  @Input() isChecked: boolean = false;
  @Input() state: 'editing' | 'completing' = 'editing';

  @Output('increment') incrementEvent = new EventEmitter<string>();
  @Output('decrement') decrementEvent = new EventEmitter<string>();
  @Output('delete') deleteEvent = new EventEmitter<string>();
  @Output('toggleCheck') toggleCheckEvent = new EventEmitter<string>();

  private _isEditing: boolean = false;

  get isEditing() {
    if (this.state === 'completing') this._isEditing = false;
    return this._isEditing;
  }

  set isEditing(value: boolean) {
    this._isEditing = value;
  }

  toggleEdit() {
    if (this.state === 'completing') return;
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

  deleteItem() {
    this.deleteEvent.emit(this.id ?? 'id');
  }

  toggleCheck() {
    this.isChecked = !this.isChecked;
    this.toggleCheckEvent.emit(this.id ?? 'id');
  }
}
