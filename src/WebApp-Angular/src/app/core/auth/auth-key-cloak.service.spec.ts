import { TestBed } from '@angular/core/testing';

import { AuthKeyCloakService } from './auth-key-cloak.service';

describe('AuthKeyCloakService', () => {
  let service: AuthKeyCloakService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AuthKeyCloakService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
