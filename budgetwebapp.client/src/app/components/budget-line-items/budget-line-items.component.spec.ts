import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BudgetLineItemsComponent } from './budget-line-items.component';

describe('BudgetLineItemsComponent', () => {
  let component: BudgetLineItemsComponent;
  let fixture: ComponentFixture<BudgetLineItemsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [BudgetLineItemsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(BudgetLineItemsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
