import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateBudgetDialogComponent } from './create-budget-dialog.component';

describe('CreateBudgetDialogComponent', () => {
  let component: CreateBudgetDialogComponent;
  let fixture: ComponentFixture<CreateBudgetDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CreateBudgetDialogComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(CreateBudgetDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
