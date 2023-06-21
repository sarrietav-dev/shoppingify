import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavbarComponent } from './components/navbar/navbar.component';
import { MatTooltipModule } from '@angular/material/tooltip';

@NgModule({
  exports: [NavbarComponent],
  declarations: [NavbarComponent],
  imports: [CommonModule, MatTooltipModule],
})
export class CoreModule {}
