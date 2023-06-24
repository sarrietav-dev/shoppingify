import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ShoppingCartComponent } from './pages/shopping-cart/shopping-cart.component';

@NgModule({
  exports: [ShoppingCartComponent],
  declarations: [ShoppingCartComponent],
  imports: [CommonModule],
})
export class ShoppingCartModule {}
