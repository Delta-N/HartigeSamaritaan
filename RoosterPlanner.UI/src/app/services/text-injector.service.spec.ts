import { TestBed } from '@angular/core/testing';

import { TextInjectorService } from './text-injector.service';

describe('TextInjectorService', () => {
  let service: TextInjectorService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TextInjectorService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
