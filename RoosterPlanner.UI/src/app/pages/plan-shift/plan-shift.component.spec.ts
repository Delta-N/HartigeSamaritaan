import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlanShiftComponent } from './plan-shift.component';

describe('PlanShiftComponent', () => {
  let component: PlanShiftComponent;
  let fixture: ComponentFixture<PlanShiftComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PlanShiftComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PlanShiftComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
