import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CoreModule } from './core/core.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ItemListModule } from './modules/item-list/item-list.module';
import { ShoppingCartModule } from './modules/shopping-cart/shopping-cart.module';
import { CreateItemFormModule } from './modules/create-item-form/create-item-form.module';

@NgModule({
  declarations: [AppComponent],
  imports: [
    BrowserModule,
    AppRoutingModule,
    CoreModule,
    BrowserAnimationsModule,
    ItemListModule,
    ShoppingCartModule,
    CreateItemFormModule
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
