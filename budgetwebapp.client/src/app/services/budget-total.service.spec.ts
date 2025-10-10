import { TestBed } from '@angular/core/testing';

import { BudgetTotalService } from './budget-total.service';

describe('BudgetTotalService', () => {
  let service: BudgetTotalService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(BudgetTotalService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
