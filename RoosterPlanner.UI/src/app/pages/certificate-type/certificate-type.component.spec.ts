import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CertificateTypeComponent } from './certificate-type.component';

describe('CertificateTypeComponent', () => {
  let component: CertificateTypeComponent;
  let fixture: ComponentFixture<CertificateTypeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CertificateTypeComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CertificateTypeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
