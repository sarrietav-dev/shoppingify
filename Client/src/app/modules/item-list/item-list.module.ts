import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ItemDashboardComponent } from './pages/item-dashboard/item-dashboard.component';
import { DashboardItemComponent } from './components/dashboard-item/dashboard-item.component';

@NgModule({
  exports: [ItemDashboardComponent],
  declarations: [ItemDashboardComponent, DashboardItemComponent],
  imports: [CommonModule],
})
export class ItemListModule {}
