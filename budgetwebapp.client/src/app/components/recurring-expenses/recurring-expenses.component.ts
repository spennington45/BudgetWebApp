import { Component, OnInit } from '@angular/core';
import { RecurringExpense, Category, SourceType, User } from '../../models';
import { RecurringExpenseService } from '../../services/recurring-expense.service';
import { CategoryService } from '../../services/category.service';
import { SourceTypeService } from '../../services/source-type.service';
import { AuthService } from '../../services/auth.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-recurring-expenses',
  templateUrl: './recurring-expenses.component.html',
  styleUrls: ['./recurring-expenses.component.css']
})
export class RecurringExpensesComponent implements OnInit {

  recurringExpenses: RecurringExpense[] = [];

  displayedColumns: string[] = [
    'label',
    'value',
    'category',
    'source',
    'actions'
  ];

  categories: Category[] = [];
  sourceTypes: SourceType[] = [];
  editingId: number | null = null;
  newExpense: RecurringExpense | null = null;
  currentUser: User | null = null;
  originalExpense: RecurringExpense | null = null;

  constructor(
    private recurringService: RecurringExpenseService,
    private categoryService: CategoryService,
    private snackBar: MatSnackBar,
    private authService: AuthService,
    private sourceTypeService: SourceTypeService,
  ) { }

  ngOnInit(): void {
    this.loadCategories();
    this.loadSourceTypes();
    this.authService.currentUser$.subscribe(user => {
      if (user) {
        this.currentUser = user;
        this.loadRecurringExpenses();
      }
    });
  }

  loadCategories() {
    this.categoryService.getAllCategories().subscribe({
      next: cats => this.categories = cats,
      error: err => console.error('Failed to load categories', err)
    });
  }

  loadSourceTypes() {
    this.sourceTypeService.getAllSourceTypes().subscribe({
      next: sources => this.sourceTypes = sources,
      error: err => console.error('Failed to load categories', err)
    });
  }

  loadRecurringExpenses() {
    if (!this.currentUser) return;

    this.recurringService.getRecurringExpensesByUserId(this.currentUser.userId).subscribe({
      next: expenses => {
        this.recurringExpenses = expenses;
        this.recurringExpenses = [...this.recurringExpenses];
      }
    });
  }

  totalRecurringExpenses(): number {
    return this.recurringExpenses.reduce((sum, exp) => sum + exp.value, 0);
  }

  addNewRecurringExpense() {
    this.cancelEdit();

    const tempId = -1;

    this.newExpense = {
      recurringExpensesId: tempId,
      label: '',
      value: 0,
      categoryId: 0,
      sourceTypeId: 0,
      userId: this.currentUser?.userId!,
      category: {} as Category,
      sourceType: {} as SourceType,
      user: {} as User
    };

    this.recurringExpenses.unshift(this.newExpense);
    this.recurringExpenses = [...this.recurringExpenses];

    this.editingId = tempId;
  }

  isValid(exp: RecurringExpense): boolean {
    return !!exp.label && exp.value != 0 && exp.categoryId > 0 && exp.sourceTypeId > 0;
  }

  saveNewRecurringExpense() {
    if (!this.newExpense || !this.isValid(this.newExpense)) {
      this.snackBar.open('Please fill out all fields.', 'Close', { duration: 3000 });
      return;
    }

    if (this.currentUser) {
      this.newExpense.user = this.currentUser;
      this.recurringService.addRecurringExpense(this.newExpense).subscribe({
        next: created => {
          this.snackBar.open('Recurring expense added', 'Close', { duration: 3000 });

          const updated = this.recurringExpenses.filter(
            e => e.recurringExpensesId !== -1
          );
          updated.unshift(created);

          this.recurringExpenses = updated;

          this.newExpense = null;
          this.editingId = null;
        },
        error: err => console.error('Failed to add recurring expense', err)
      });
    }
  }

  cancelNewRecurringExpense() {
    if (this.newExpense) {
      const index = this.recurringExpenses.findIndex(
        item => item.recurringExpensesId === this.newExpense?.recurringExpensesId
      );

      if (index !== -1) {
        this.recurringExpenses.splice(index, 1);
        this.recurringExpenses = [...this.recurringExpenses];
      }
    }

    this.newExpense = null;
    this.editingId = null;
  }

  startEdit(exp: RecurringExpense) {
    this.cancelNewRecurringExpense();
    this.editingId = exp.recurringExpensesId;

    this.originalExpense = JSON.parse(JSON.stringify(exp));
  }

  saveEdit(exp: RecurringExpense) {
    if (!this.isValid(exp)) return;

    this.recurringService.updateRecurringExpense(exp).subscribe({
      next: () => {
        this.snackBar.open('Recurring expense updated', 'Close', { duration: 3000 });
        this.editingId = null;
        this.loadRecurringExpenses();
      },
      error: err => console.error('Failed to update recurring expense', err)
    });
  }

  cancelEdit() {
    if (this.originalExpense) {
      const index = this.recurringExpenses.findIndex(
        e => e.recurringExpensesId === this.originalExpense!.recurringExpensesId
      );

      if (index !== -1) {
        this.recurringExpenses[index] = { ...this.originalExpense };
      }
    }

    this.originalExpense = null;
    this.editingId = null;
  }

  deleteRecurringExpense(exp: RecurringExpense) {
    this.recurringService.deleteRecurringExpense(exp.recurringExpensesId).subscribe({
      next: () => {
        this.snackBar.open('Recurring expense deleted', 'Close', { duration: 3000 });
        this.loadRecurringExpenses();
      },
      error: err => console.error('Failed to delete recurring expense', err)
    });
  }

  compareCategories(a: Category, b: Category): boolean {
    return a && b ? a.categoryId === b.categoryId : a === b;
  }
}