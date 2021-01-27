import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddCertificatetypeComponent } from './add-certificatetype.component';

describe('AddCertificatetypeComponent', () => {
  let component: AddCertificatetypeComponent;
  let fixture: ComponentFixture<AddCertificatetypeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddCertificatetypeComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AddCertificatetypeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
