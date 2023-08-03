import { Component, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-complete-footer',
  template: `
    <div class="flex h-full items-center justify-evenly ">
      <button (click)="cancel()" class="font-semibold">cancel</button>
      <button
        (click)="complete()"
        class="rounded-lg bg-cyan-400 px-4 py-3 font-semibold text-white"
      >
        Complete
      </button>
    </div>
  `,
})
export class CompleteFooterComponent {
  @Output('complete') completeEvent = new EventEmitter<string>();
  @Output('cancel') cancelEvent = new EventEmitter<string>();

  complete() {
    this.completeEvent.emit('complete');
  }

  cancel() {
    this.cancelEvent.emit('cancel');
  }
}
