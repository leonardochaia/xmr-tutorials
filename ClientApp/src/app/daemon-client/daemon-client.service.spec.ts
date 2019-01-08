import { TestBed, inject } from '@angular/core/testing';

import { DaemonClientService } from './daemon-client.service';

describe('DaemonClientService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [DaemonClientService]
    });
  });

  it('should be created', inject([DaemonClientService], (service: DaemonClientService) => {
    expect(service).toBeTruthy();
  }));
});
