import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MainComponent } from './pages/main/main.component';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CoreModule } from 'src/app/core/core.module';
import { ItemListModule } from '../item-list/item-list.module';
import { ShoppingCartModule } from '../shopping-cart/shopping-cart.module';
import { MainRoutingModule } from './main-routing.module';



@NgModule({
  declarations: [
    MainComponent
  ],
  imports: [
    CommonModule,
    CoreModule,
    ItemListModule,
    ShoppingCartModule,
    MainRoutingModule
  ]
})
export class MainModule { }
