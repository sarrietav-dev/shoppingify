import { Component } from '@angular/core';

@Component({
  selector: 'app-shopping-cart',
  templateUrl: './shopping-cart.component.html',
  styleUrls: ['./shopping-cart.component.css'],
})
export class ShoppingCartComponent {
  isCreateItemFormVisible = false;
  state: ShoppingCartState = 'empty';

  changeState(state: ShoppingCartState) {
    this.state = state;
  }

  toggleCreateItemForm() {
    this.isCreateItemFormVisible = !this.isCreateItemFormVisible;
  }
}
