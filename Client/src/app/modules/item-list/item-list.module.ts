import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ItemDashboardComponent } from './pages/item-dashboard/item-dashboard.component';

@NgModule({
  exports: [ItemDashboardComponent],
  declarations: [ItemDashboardComponent],
  imports: [CommonModule],
})
export class ItemListModule {}
