import { TestBed } from '@angular/core/testing';

import { BudgetLineItemService } from './budget-line-item.service';

describe('BudgetLineItemService', () => {
  let service: BudgetLineItemService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(BudgetLineItemService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
