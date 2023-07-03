import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CreateItemFormComponent } from './pages/create-item-form/create-item-form.component';

@NgModule({
  declarations: [CreateItemFormComponent],
  imports: [CommonModule],
  exports: [CreateItemFormComponent],
})
export class CreateItemFormModule {}
