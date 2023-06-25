import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-shopping-list-item',
  templateUrl: './shopping-list-item.component.html',
  styleUrls: ['./shopping-list-item.component.css']
})
export class ShoppingListItemComponent {
  @Input() name!: string;
  @Input() amount!: number;

  isEditing = false;

  toggleEdit() {
    this.isEditing = !this.isEditing;
  }
}
