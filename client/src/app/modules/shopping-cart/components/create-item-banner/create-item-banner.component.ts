import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-create-item-banner',
  templateUrl: './create-item-banner.component.html',
  styleUrls: ['./create-item-banner.component.css'],
})
export class CreateItemBannerComponent {
  @Output() click = new EventEmitter<void>();

  onClick() {
    this.click.emit();
  }
}
