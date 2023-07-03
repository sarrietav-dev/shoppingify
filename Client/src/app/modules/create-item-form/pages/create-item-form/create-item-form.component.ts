import { Component } from '@angular/core';
import { FormBuilder } from '@angular/forms';

@Component({
  selector: 'app-create-item-form',
  templateUrl: './create-item-form.component.html',
  styleUrls: ['./create-item-form.component.css'],
})
export class CreateItemFormComponent {
  constructor(private fb: FormBuilder) {}

  createItemForm = this.fb.group({
    name: [''],
    note: [''],
    image: [''],
    category: [''],
  });

  onSubmit() {
    console.log(this.createItemForm.value);
  }
}
