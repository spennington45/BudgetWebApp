import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-create-budget-dialog',
  templateUrl: './create-budget-dialog.component.html',
  styleUrls: ['./create-budget-dialog.component.css']
})
export class CreateBudgetDialogComponent {
  selectedDate: Date = new Date();

  constructor(public dialogRef: MatDialogRef<CreateBudgetDialogComponent>) { }

  onNoClick(): void {
    this.dialogRef.close();
  }

  onSaveClick(): void {
    const selectedYear = this.selectedDate.getFullYear();
    const selectedMonth = this.selectedDate.getMonth();
    this.dialogRef.close({ year: selectedYear, month: selectedMonth });
  }
}
