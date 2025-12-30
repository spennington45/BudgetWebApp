import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import * as _moment from 'moment';
import { default as _rollupMoment } from 'moment';
import { BudgetService } from '../../services/budget.service';
import { UserService } from '../../services/user.service';
import { Budget, User } from '../../models';

const moment = _rollupMoment || _moment;

@Component({
  selector: 'app-create-budget-dialog',
  templateUrl: './create-budget-dialog.component.html',
  styleUrls: ['./create-budget-dialog.component.scss']
})
export class CreateBudgetDialogComponent {
  months = ['January', 'February', 'March', 'April', 'May', 'June',
    'July', 'August', 'September', 'October', 'November', 'December'];

  years: number[] = [];
  selectedYear!: number;
  selectedMonth!: number;
  user!: User;

  constructor(public dialogRef: MatDialogRef<CreateBudgetDialogComponent>, private budgetService: BudgetService, private userService: UserService) { }

  ngOnInit() {
    const currentYear = new Date().getFullYear();
    this.years = Array.from({ length: 10 }, (_, i) => currentYear - 5 + i);
    this.selectedYear = currentYear;
    this.selectedMonth = new Date().getMonth();
    this.user = { // Replace with real user data
      userId: 1,
      firstName: 'Default',
      lastName: 'User', 
    };
  }

  selectedMonthLabel(): string {
    return this.months[this.selectedMonth];
  }

    onNoClick(): void {
    this.dialogRef.close();
  }

  onSaveClick(): void {
    const selectedDate = new Date(this.selectedYear, this.selectedMonth, 1);

    const newBudget: Budget = {
      userId: this.user.userId,
      date: selectedDate,
      budgetLineItems: [],
      user: this.user,
      displayDate: moment(selectedDate).format('MMMM, YYYY'),
      budgetId: 0
    };

    this.budgetService.addBudget(newBudget).subscribe({
      next: (createdBudget) => {
        this.dialogRef.close(createdBudget);
      },
      error: (err) => {
        console.error('Failed to create budget:', err);
      }
    });
  }
}