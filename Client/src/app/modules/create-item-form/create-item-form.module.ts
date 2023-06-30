import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CreateItemFormComponent } from './pages/create-item-form/create-item-form.component';
import { InputComponent } from './components/input/input.component';

@NgModule({
  declarations: [CreateItemFormComponent, InputComponent],
  imports: [CommonModule],
  exports: [CreateItemFormComponent],
})
export class CreateItemFormModule {}
