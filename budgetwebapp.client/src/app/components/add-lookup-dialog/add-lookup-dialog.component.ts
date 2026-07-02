import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
    selector: 'app-add-lookup-dialog',
    templateUrl: './add-lookup-dialog.component.html',
    styleUrls: ['./add-lookup-dialog.component.scss'],
    standalone: false
})
export class AddLookupDialogComponent {

  value = '';
  errorMessage = '';

  constructor(
    private dialogRef: MatDialogRef<AddLookupDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: {
      title: string;
      fieldLabel: string;
      existing: any[];
      property: string;
      saveFn: (payload: any) => any;
    }
  ) {}

  save() {
    const trimmed = this.value.trim();

    if (!trimmed) {
      this.errorMessage = 'Value cannot be empty.';
      return;
    }

    const exists = this.data.existing.some(
      item => item[this.data.property].toLowerCase() === trimmed.toLowerCase()
    );

    if (exists) {
      this.errorMessage = `${this.data.fieldLabel} already exists.`;
      return;
    }

    const payload: any = {};
    payload[this.data.property] = trimmed;

    this.data.saveFn(payload).subscribe({
      next: (createdItem: any) => this.dialogRef.close(createdItem),
      error: (err: any) => {
        console.error('Failed to create item', err);
        this.errorMessage = 'Failed to save. Try again.';
      }
    });
  }

  cancel() {
    this.dialogRef.close(null);
  }
}
