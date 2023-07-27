import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ItemDashboardComponent } from './pages/item-dashboard/item-dashboard.component';
import { DashboardItemComponent } from './components/dashboard-item/dashboard-item.component';
import { ProductOverviewComponent } from './components/product-overview/product-overview.component';

@NgModule({
  exports: [ItemDashboardComponent, ProductOverviewComponent],
  declarations: [ItemDashboardComponent, DashboardItemComponent, ProductOverviewComponent],
  imports: [CommonModule],
})
export class ItemListModule {}
