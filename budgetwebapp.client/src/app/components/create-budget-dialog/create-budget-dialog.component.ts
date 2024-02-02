import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-create-budget-dialog',
  templateUrl: './create-budget-dialog.component.html',
  styleUrls: ['./create-budget-dialog.component.css'],
})
export class CreateBudgetDialogComponent {
  selectedDate: any;

  constructor(public dialogRef: MatDialogRef<CreateBudgetDialogComponent>) { }

  onNoClick(): void {
    this.dialogRef.close();
  }

  onSaveClick(): void {
    this.dialogRef.close(this.selectedDate);
  }
}
