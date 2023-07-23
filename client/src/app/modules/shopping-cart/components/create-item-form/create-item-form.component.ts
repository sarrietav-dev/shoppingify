import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ProductsService } from 'src/app/core/services/products/products.service';

@Component({
  selector: 'app-create-item-form',
  templateUrl: './create-item-form.component.html',
  styleUrls: ['./create-item-form.component.css'],
})
export class CreateItemFormComponent {
  constructor(
    private fb: FormBuilder,
    private productService: ProductsService
  ) {}

  @Output() cancelForm = new EventEmitter<ShoppingCartState>();

  createItemForm = this.fb.group({
    name: ['', Validators.required],
    note: [''],
    image: [
      '',
      Validators.pattern('(http(s?):)([/|.|\\w|\\s|-])*'),
    ],
    category: ['', Validators.required],
  });

  onSubmit() {
    console.log(this.createItemForm);

    if (this.createItemForm.valid) {
      this.productService
        .createProduct({
          name: this.createItemForm.value.name!,
          note: this.createItemForm.value.note!,
          image: this.createItemForm.value.image!,
          category: this.createItemForm.value.category!,
        })
        .subscribe(() => this.cancelForm.emit('editing'));
      this.createItemForm.reset();
    }
  }

  onCancel() {
    this.cancelForm.emit('editing');
  }
}
