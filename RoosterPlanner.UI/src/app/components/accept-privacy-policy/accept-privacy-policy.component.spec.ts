import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AcceptPrivacyPolicyComponent } from './accept-privacy-policy.component';

describe('AcceptPrivacyPolicyComponent', () => {
  let component: AcceptPrivacyPolicyComponent;
  let fixture: ComponentFixture<AcceptPrivacyPolicyComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AcceptPrivacyPolicyComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AcceptPrivacyPolicyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
