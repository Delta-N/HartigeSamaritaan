import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ScheduleManagerComponent } from './schedule-manager.component';

describe('ScheduleManagerComponent', () => {
  let component: ScheduleManagerComponent;
  let fixture: ComponentFixture<ScheduleManagerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ScheduleManagerComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ScheduleManagerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
