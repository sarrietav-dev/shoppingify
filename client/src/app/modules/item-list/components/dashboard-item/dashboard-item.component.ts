import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'dashboard-item',
  templateUrl: './dashboard-item.component.html',
  styleUrls: ['./dashboard-item.component.css'],
})
export class DashboardItemComponent {
  @Input() title!: string;
  @Output("click") itemClicked = new EventEmitter();

  onClick() {
    this.itemClicked.emit();
  }
}
