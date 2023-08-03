import { Component, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-save-input',
  template: `
    <div class="relative h-full w-full">
      <input
        type="text"
        class="h-full w-full rounded-xl border-2 border-primary px-6 placeholder:text-sm placeholder:font-semibold placeholder:text-gray-300 focus:border-secondary focus:outline-none focus:ring-0"
        placeholder="Enter a name"
      />
      <button
        (click)="saveEvent.emit('saving')"
        class="absolute right-0 h-full rounded-xl bg-primary px-6 py-2 font-semibold text-white"
        type="button"
      >
        Save
      </button>
    </div>
  `,
})
export class SaveInputComponent {
  @Output('save') saveEvent = new EventEmitter<string>();
}
