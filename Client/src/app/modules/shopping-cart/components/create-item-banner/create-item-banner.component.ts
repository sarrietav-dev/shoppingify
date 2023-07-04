import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-create-item-banner',
  templateUrl: './create-item-banner.component.html',
  styleUrls: ['./create-item-banner.component.css'],
})
export class CreateItemBannerComponent {
  @Input("click") onClick!: () => void;
}
