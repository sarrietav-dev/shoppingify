import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ShoppingCartComponent } from './pages/shopping-cart/shopping-cart.component';
import { CreateItemBannerComponent } from './components/create-item-banner/create-item-banner.component';

@NgModule({
  exports: [ShoppingCartComponent],
  declarations: [ShoppingCartComponent, CreateItemBannerComponent],
  imports: [CommonModule],
})
export class ShoppingCartModule {}
