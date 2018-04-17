import { TestBed, inject } from '@angular/core/testing';

import { AlfredApiService } from './alfred-api.service';

describe('AlfredApiService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [AlfredApiService]
    });
  });

  it('should be created', inject([AlfredApiService], (service: AlfredApiService) => {
    expect(service).toBeTruthy();
  }));
});
