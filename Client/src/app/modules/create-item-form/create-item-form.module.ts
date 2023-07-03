import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CreateItemFormComponent } from './pages/create-item-form/create-item-form.component';
import { ReactiveFormsModule } from '@angular/forms';

@NgModule({
  declarations: [CreateItemFormComponent],
  imports: [CommonModule, ReactiveFormsModule],
  exports: [CreateItemFormComponent],
})
export class CreateItemFormModule {}
