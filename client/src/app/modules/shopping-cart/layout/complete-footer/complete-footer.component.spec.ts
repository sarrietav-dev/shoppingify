import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CompleteFooterComponent } from './complete-footer.component';

describe('CompleteFooterComponent', () => {
  let component: CompleteFooterComponent;
  let fixture: ComponentFixture<CompleteFooterComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CompleteFooterComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(CompleteFooterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
