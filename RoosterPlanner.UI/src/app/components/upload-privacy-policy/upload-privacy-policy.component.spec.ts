import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UploadPrivacyPolicyComponent } from './upload-privacy-policy.component';

describe('UploadTosComponent', () => {
  let component: UploadPrivacyPolicyComponent;
  let fixture: ComponentFixture<UploadPrivacyPolicyComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UploadPrivacyPolicyComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(UploadPrivacyPolicyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
