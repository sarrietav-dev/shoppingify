import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-layout-with-save',
  template: `
    <div class="relative flex h-screen flex-col bg-shopping-list pt-10">
      <div class="flex-shrink-0 px-4">
        <app-create-item-banner
          (click)="toggleCreateItemForm()"
        ></app-create-item-banner>
      </div>
      <div class="grow overflow-hidden">
        <ng-content select="[body]"></ng-content>
      </div>
      <div class="w-full flex-shrink-0 basis-24 bg-white px-6 py-4">
        <ng-content select="[footer]"></ng-content>
      </div>
    </div>
  `,
})
export class LayoutWithSaveComponent {
  @Output('createClick') onCreateClick = new EventEmitter<ShoppingCartState>();

  toggleCreateItemForm() {
    this.onCreateClick.emit('creating');
  }
}
