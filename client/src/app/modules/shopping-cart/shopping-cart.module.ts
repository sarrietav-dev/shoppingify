import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ShoppingCartComponent } from './pages/shopping-cart/shopping-cart.component';
import { CreateItemBannerComponent } from './components/create-item-banner/create-item-banner.component';
import { ShoppingListItemComponent } from './components/shopping-list-item/shopping-list-item.component';
import { CreateItemFormComponent } from './components/create-item-form/create-item-form.component';
import { ReactiveFormsModule } from '@angular/forms';
import { LayoutWithSaveComponent } from './layout/layout-with-save/layout-with-save.component';
import { EditShoppingListComponent } from './components/edit-shopping-list/edit-shopping-list.component';
import { EmptyListComponent } from './components/empty-list/empty-list.component';
import { SaveInputComponent } from './layout/save-input/save-input.component';
import { CompleteFooterComponent } from './layout/complete-footer/complete-footer.component';

@NgModule({
  exports: [ShoppingCartComponent],
  declarations: [
    ShoppingCartComponent,
    CreateItemBannerComponent,
    ShoppingListItemComponent,
    CreateItemFormComponent,
    LayoutWithSaveComponent,
    EditShoppingListComponent,
    EmptyListComponent,
    SaveInputComponent,
    CompleteFooterComponent,
  ],
  imports: [CommonModule, ReactiveFormsModule],
})
export class ShoppingCartModule {}
