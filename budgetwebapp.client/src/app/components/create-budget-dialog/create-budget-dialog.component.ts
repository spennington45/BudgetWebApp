import { Component, Inject, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { BudgetService } from '../../services/budget.service';
import { Budget, User } from '../../models';
import { AuthService } from '../../services/auth.service';

@Component({
    selector: 'app-create-budget-dialog',
    templateUrl: './create-budget-dialog.component.html',
    styleUrls: ['./create-budget-dialog.component.scss'],
    changeDetection: ChangeDetectionStrategy.Eager,
    standalone: false
})
export class CreateBudgetDialogComponent implements OnInit {
  months = [
    'January', 'February', 'March', 'April', 'May', 'June',
    'July', 'August', 'September', 'October', 'November', 'December'
  ];

  years: number[] = [];
  selectedYear!: number;
  selectedMonth!: number; 
  user!: User;

  constructor(
    public dialogRef: MatDialogRef<CreateBudgetDialogComponent>,
    private budgetService: BudgetService,
    private authService: AuthService,
    @Inject(MAT_DIALOG_DATA)
    public data: { existingBudgets: Budget[], budgetToEdit?: Budget }
  ) {}

  ngOnInit() {
    const currentYear = new Date().getFullYear();
    this.years = Array.from({ length: 10 }, (_, i) => currentYear - 5 + i);

    if (this.data.budgetToEdit) {
      this.selectedYear = this.data.budgetToEdit.year;
      this.selectedMonth = this.data.budgetToEdit.month - 1;
    } else {
      this.selectedYear = currentYear;
      this.selectedMonth = new Date().getMonth();
    }

    this.authService.currentUser$.subscribe(user => {
      if (!user) throw new Error("User must be logged in before creating a budget.");
      this.user = user;
    });
  }

  selectedMonthLabel(): string {
    return this.months[this.selectedMonth];
  }

  onNoClick(): void {
    this.dialogRef.close();
  }

  onSaveClick(): void {
    const year = this.selectedYear;
    const month = this.selectedMonth + 1; 

    if (!this.data.budgetToEdit) {
      const exists = this.data.existingBudgets.some(b =>
        b.year === year && b.month === month
      );

      if (exists) {
        alert("A budget already exists for this month and year.");
        return;
      }
    }

    if (this.data.budgetToEdit) {
      const updatedBudget: Budget = {
        ...this.data.budgetToEdit,
        year,
        month
      };

      this.budgetService.updateBudget(updatedBudget).subscribe({
        next: () => this.dialogRef.close(updatedBudget),
        error: err => console.error('Failed to update budget:', err)
      });

    } else {
      const newBudget: Budget = {
        budgetId: 0,
        userId: this.user.userId,
        year,
        month,
        budgetLineItems: [],
        user: this.user
      };

      this.budgetService.addBudget(newBudget).subscribe({
        next: createdBudget => this.dialogRef.close(createdBudget),
        error: err => console.error('Failed to create budget:', err)
      });
    }
  }
}
