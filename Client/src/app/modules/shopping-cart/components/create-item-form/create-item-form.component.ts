import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-create-item-form',
  templateUrl: './create-item-form.component.html',
  styleUrls: ['./create-item-form.component.css'],
})
export class CreateItemFormComponent {
  constructor(private fb: FormBuilder) {}

  @Output() cancelForm = new EventEmitter<void>();

  createItemForm = this.fb.group({
    name: ['', Validators.required],
    note: [''],
    image: [
      '',
      Validators.pattern('(http(s?):)([/|.|\\w|\\s|-])*\\.(?:jpg|gif|png)'),
    ],
    category: ['', Validators.required],
  });

  onSubmit() {
    console.log(this.createItemForm.value);
  }

  onCancel() {
    this.cancelForm.emit();
  }
}
