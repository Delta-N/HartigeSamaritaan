import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddProjectTaskComponent } from './add-project-task.component';

describe('AddProjectTaskComponent', () => {
  let component: AddProjectTaskComponent;
  let fixture: ComponentFixture<AddProjectTaskComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddProjectTaskComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AddProjectTaskComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
