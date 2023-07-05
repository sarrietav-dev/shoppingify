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
        <ng-content></ng-content>
      </div>
      <div class="w-full flex-shrink-0 basis-24 bg-white px-6 py-4">
        <form class="relative h-full w-full">
          <input
            type="text"
            class="h-full w-full rounded-xl border-2 border-primary px-6 placeholder:text-sm placeholder:font-semibold placeholder:text-gray-300 focus:border-secondary focus:outline-none focus:ring-0"
            placeholder="Enter a name"
          />
          <button
            class="absolute right-0 h-full rounded-xl bg-primary px-6 py-2 font-semibold text-white"
            type="submit"
          >
            Save
          </button>
        </form>
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
