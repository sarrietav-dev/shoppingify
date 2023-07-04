import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ShoppingCartComponent } from './pages/shopping-cart/shopping-cart.component';
import { CreateItemBannerComponent } from './components/create-item-banner/create-item-banner.component';
import { ShoppingListItemComponent } from './components/shopping-list-item/shopping-list-item.component';
import { CreateItemFormComponent } from './components/create-item-form/create-item-form.component';
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
  exports: [ShoppingCartComponent],
  declarations: [
    ShoppingCartComponent,
    CreateItemBannerComponent,
    ShoppingListItemComponent,
    CreateItemFormComponent,
  ],
  imports: [CommonModule, ReactiveFormsModule],
})
export class ShoppingCartModule {}
