import { TestBed } from '@angular/core/testing';

import { AvailabilityService } from './availability.service';

describe('AvailibilityService', () => {
  let service: AvailabilityService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AvailabilityService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
