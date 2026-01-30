import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import * as _moment from 'moment';
import { default as _rollupMoment } from 'moment';
import { BudgetService } from '../../services/budget.service';
import { Budget, User } from '../../models';
import { AuthService } from '../../services/auth.service';

const moment = _rollupMoment || _moment;

@Component({
  selector: 'app-create-budget-dialog',
  templateUrl: './create-budget-dialog.component.html',
  styleUrls: ['./create-budget-dialog.component.scss']
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
    public data: { existingBudgets: Budget[], budgetToEdit?: Budget}
  ) {}

  ngOnInit() {
    const currentYear = new Date().getFullYear();
    this.years = Array.from({ length: 10 }, (_, i) => currentYear - 5 + i);

    if (this.data.budgetToEdit) {
      const date = new Date(this.data.budgetToEdit.date);
      this.selectedYear = date.getFullYear();
      this.selectedMonth = date.getMonth();
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
    const selectedDate = new Date(this.selectedYear, this.selectedMonth, 1);

    if (!this.data.budgetToEdit) {
      const exists = this.data.existingBudgets.some(b =>
        b.date.getFullYear() === selectedDate.getFullYear() &&
        b.date.getMonth() === selectedDate.getMonth()
      );

      if (exists) {
        alert("A budget already exists for this month and year.");
        return;
      }
    }

    if (this.data.budgetToEdit) {
      const updatedBudget = {
        ...this.data.budgetToEdit,
        date: selectedDate,
        displayDate: moment(selectedDate).format('MMMM, YYYY')
      };

      this.budgetService.updateBudget(updatedBudget).subscribe({
        next: () => this.dialogRef.close(updatedBudget),
        error: err => console.error('Failed to update budget:', err)
      });

    } else {
      const newBudget: Budget = {
        userId: this.user.userId,
        date: selectedDate,
        budgetLineItems: [],
        user: this.user,
        displayDate: moment(selectedDate).format('MMMM, YYYY'),
        budgetId: 0
      };

      this.budgetService.addBudget(newBudget).subscribe({
        next: createdBudget => this.dialogRef.close(createdBudget),
        error: err => console.error('Failed to create budget:', err)
      });
    }
  }
}