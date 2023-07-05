import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateItemBannerComponent } from './create-item-banner.component';

describe('CreateItemBannerComponent', () => {
  let component: CreateItemBannerComponent;
  let fixture: ComponentFixture<CreateItemBannerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CreateItemBannerComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(CreateItemBannerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
