import { Component, Input, OnInit } from '@angular/core';
import { BudgetLineItems, Category, RecurringExpense, SourceType, User } from '../../models';
import {
  ApexAxisChartSeries,
  ApexChart,
  ApexDataLabels,
  ApexLegend,
  ApexNonAxisChartSeries,
  ApexPlotOptions,
  ApexXAxis
} from 'ng-apexcharts';
import { MatSnackBar } from '@angular/material/snack-bar';
import { BudgetLineItemService } from '../../services/budget-line-item.service';
import { SourceTypeService } from '../../services/source-type.service';
import { CategoryService } from '../../services/category.service';
import { RecurringExpenseService } from '../../services/recurring-expense.service';
import { AuthService } from '../../services/auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AddLookupDialogComponent } from '../add-lookup-dialog/add-lookup-dialog.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-budget-line-items',
  templateUrl: './budget-line-items.component.html',
  styleUrls: ['./budget-line-items.component.css']
})
export class BudgetLineItemsComponent implements OnInit {
  // Replace catigory color stuff eventually
  private categoryColorMap: Map<string, string> = new Map();
  private colorPalette: string[] = [
    '#FF6384', '#36A2EB', '#FFCE56', '#4BC0C0',
    '#9966FF', '#FF9F40', '#C9CBCF', '#8E44AD',
    '#2ECC71', '#E67E22', '#1ABC9C', '#D35400'
  ];
  private colorIndex = 0;

  @Input() budgetId: string = '';
  budgetLineItems: BudgetLineItems[] = [];
  originalLineItem: BudgetLineItems | null = null;
  categories: Category[] = [];
  sourceTypes: SourceType[] = [];
  currentUser: User | null = null;

  public expensePieSeries: ApexNonAxisChartSeries = [];
  public expensePieLabels: string[] = [];
  public expensePieColors: string[] = [];

  public expensePieChart: ApexChart = {
    type: 'pie',
    height: 350
  };

  public pieLegend: ApexLegend = {
    position: 'bottom'
  };

  public incomeExpenseSeries: ApexAxisChartSeries = [];

  public incomeExpenseChart: ApexChart = {
    type: 'bar',
    height: 350,
    stacked: true
  };

  public incomeExpenseXAxis: ApexXAxis = {
    categories: ['Income', 'Expenses']
  };

  public incomeExpensePlotOptions: ApexPlotOptions = {
    bar: {
      horizontal: false
    }
  };

  public incomeExpenseDataLabels: ApexDataLabels = {
    enabled: true
  };

  public incomeExpenseLegend: ApexLegend = {
    position: 'bottom'
  };

  currentBudgetId: number = 0;
  barChartLabels = ['Income', 'Expenses'];

  displayedColumns: string[] = ['name', 'value', 'category', 'sourceType', 'actions'];

  newLineItem: BudgetLineItems | null = null;
  editingLineItemId: number | null = null;
  categoryFilter = '';
  sourceTypeFilter = '';

  pieChartLabels: string[] = [];

  constructor(
    private budgetLineItemService: BudgetLineItemService,
    private snackBar: MatSnackBar,
    private route: ActivatedRoute,
    private categoryService: CategoryService,
    private sourceTypeService: SourceTypeService,
    private recurringExpenseService: RecurringExpenseService,
    private authService: AuthService,
    private dialog: MatDialog,
    private router: Router) { }

  ngOnInit() {
    this.authService.currentUser$.subscribe(user => {
      if (user) {
        this.currentUser = user;
      }
    });

    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      this.currentBudgetId = id ? Number(id) : 0;
      this.getSourceTypes();
      this.getCategories();
      this.getLineItemData();
    });
  }


  getLineItemData() {
    this.budgetLineItems = [];
    this.budgetLineItemService.getBudgetLineItemsByBudgetId(Number.parseInt(this.budgetId)).subscribe(response => {
      this.budgetLineItems = response;

      const existingCategoryIds = new Set(this.categories.map(c => c.categoryId));
      response.forEach(item => {
        if (!existingCategoryIds.has(item.category.categoryId)) {
          this.categories.push(item.category);
          existingCategoryIds.add(item.category.categoryId);
        }
      });

      this.getChartData();
    });
  }

  getSourceTypes() {
    this.sourceTypeService.getAllSourceTypes().subscribe(response => {
      this.sourceTypes = response;
    });
    // this.sourceTypes = [...this.sourceTypes];
  }

  getCategories() {
    this.categoryService.getAllCategories().subscribe(response => {
      this.categories = response;
    });
    //this.categories = [...this.categories];
  }

  getChartData() {
    const expenseItems = this.budgetLineItems.filter(x => x.value <= 0);
    const incomeItems = this.budgetLineItems.filter(x => x.value > 0);

    const expenseMap = new Map<string, number>();
    const incomeMap = new Map<string, number>();

    expenseItems.forEach(item => {
      const category = item.category?.categoryName || 'Unknown';

      expenseMap.set(
        category,
        (expenseMap.get(category) || 0) + Math.abs(item.value)
      );
    });

    incomeItems.forEach(item => {
      const category = item.category?.categoryName || 'Unknown';

      incomeMap.set(
        category,
        (incomeMap.get(category) || 0) + item.value
      );
    });

    this.expensePieLabels = [...expenseMap.keys()];
    this.expensePieSeries = [...expenseMap.values()];
    this.expensePieColors = this.generateColors(this.expensePieLabels.length);

    const series: ApexAxisChartSeries = [];

    incomeMap.forEach((value, category) => {
      series.push({
        name: category,
        data: [value, 0]
      });
    });

    expenseMap.forEach((value, category) => {
      series.push({
        name: category,
        data: [0, value]
      });
    });

    this.incomeExpenseSeries = series;
  }

  getColorForCategory(category: string): string {
    if (this.categoryColorMap.has(category)) {
      return this.categoryColorMap.get(category)!;
    }

    const color = this.colorPalette[this.colorIndex % this.colorPalette.length];
    this.categoryColorMap.set(category, color);
    this.colorIndex++;

    return color;
  }

  generateColors(count: number): string[] {
    const palette = ['#FF6384', '#36A2EB', '#FFCE56', '#4BC0C0', '#9966FF', '#FF9F40', '#8BC34A', '#E91E63'];
    const colors: string[] = [];
    for (let i = 0; i < count; i++) {
      colors.push(palette[i % palette.length]);
    }
    return colors;
  }


  calculatePercentages(): number[] {
    const categoryTotals: { [categoryId: string]: number } = {};

    this.budgetLineItems.forEach(item => {
      const catId = item.category.categoryId;
      categoryTotals[catId] = (categoryTotals[catId] || 0) + +item.value;
    });

    return this.categories.map(cat => categoryTotals[cat.categoryId] || 0);
  }

  addNewLineItem() {
    const tempId = 0
    this.newLineItem = {
      budgetLineItemId: 0,
      budgetId: Number(this.budgetId),

      transactionId: null,
      pendingTransactionId: null,
      date: new Date().toISOString().split('T')[0],
      value: 0,
      name: '',
      merchantName: '',
      pending: false,

      categoryId: 0,
      plaidAccountId: null,
      userId: this.currentUser?.userId ?? 0,
      sourceTypeId: 0,

      createdAt: '',
      updatedAt: '',

      category: {} as Category,
      sourceType: {} as SourceType
    };
    this.budgetLineItems.unshift(this.newLineItem);
    this.editingLineItemId = tempId;
    this.budgetLineItems = [...this.budgetLineItems];
  }

  isNewLineItemValid(): boolean {
    return !!this.newLineItem &&
      this.newLineItem.name?.trim() !== '' &&
      this.newLineItem.value != 0 &&
      !!this.newLineItem.category &&
      !!this.newLineItem.sourceType;
  }

  isLineItemValid(item: BudgetLineItems): boolean {
    return item.name?.trim() !== '' &&
      item.value != 0 &&
      !!item.category &&
      !!item.sourceType;
  }

  saveNewLineItem(): void {
    if (!this.isNewLineItemValid() || !this.newLineItem) {
      this.snackBar.open('Please fill out all fields.', 'Close', { duration: 5000 });
      return;
    }

    const { category, sourceType } = this.newLineItem;
    this.newLineItem.categoryId = category?.categoryId ?? 0;
    this.newLineItem.sourceTypeId = sourceType?.sourceTypeId ?? 0;

    const tempId = this.newLineItem.budgetLineItemId;

    this.budgetLineItemService.addBudgetLineItem(this.newLineItem).subscribe({
      next: (response) => {
        this.snackBar.open('Line item added successfully', 'Close', { duration: 5000 });
        const index = this.budgetLineItems.findIndex(item => item.budgetLineItemId === tempId);
        if (index !== -1) {
          this.budgetLineItems[index].budgetLineItemId = response.budgetLineItemId;
        }
        this.newLineItem = null;
        this.getChartData();
      },
      error: (err) => {
        this.snackBar.open('Failed to save line item.', 'Close', { duration: 5000 });
        const index = this.budgetLineItems.findIndex(item => item.budgetLineItemId === tempId);
        if (index !== -1) {
          this.budgetLineItems.splice(index, 1);
          this.budgetLineItems = [...this.budgetLineItems];
        }
        this.newLineItem = null;
      }
    });
  }


  cancelNewLineItem(): void {
    if (this.newLineItem) {
      const index = this.budgetLineItems.findIndex(
        item => item.budgetLineItemId === this.newLineItem?.budgetLineItemId
      );

      if (index !== -1) {
        this.budgetLineItems.splice(index, 1);
        this.budgetLineItems = [...this.budgetLineItems];
      }

      this.newLineItem = null;
      this.editingLineItemId = null;
    }
  }


  startEdit(lineItem: BudgetLineItems): void {
    this.editingLineItemId = lineItem.budgetLineItemId;
    this.originalLineItem = JSON.parse(JSON.stringify(lineItem));
  }

  cancelEdit(): void {
    if (this.originalLineItem) {
      const index = this.budgetLineItems.findIndex(
        item => item.budgetLineItemId === this.originalLineItem!.budgetLineItemId
      );
      if (index !== -1) {
        this.budgetLineItems[index] = { ...this.originalLineItem };
        this.budgetLineItems = [...this.budgetLineItems];
      }
    }
    this.originalLineItem = null;
    this.editingLineItemId = null;
  }

  saveEdit(lineItem: BudgetLineItems): void {
    if (lineItem.category) {
      lineItem.categoryId = lineItem.category.categoryId;
    }
    if (lineItem.sourceType) {
      lineItem.sourceTypeId = lineItem.sourceType.sourceTypeId;
    }

    this.budgetLineItemService.updateBudgetLineItem(lineItem).subscribe(() => {
      this.snackBar.open('Line item updated successfully', 'Close', { duration: 5000 });
      this.editingLineItemId = null;
      this.getLineItemData();
    });
    let index = this.budgetLineItems.indexOf(lineItem);
    if (index != -1) {
      this.budgetLineItems[index] = lineItem;
    }
    this.budgetLineItems = [...this.budgetLineItems];
    this.getChartData();
    this.editingLineItemId = null;
  }

  deleteLineItem(lineItem: BudgetLineItems): void {
    if (confirm(`Are you sure you want to delete "${lineItem.name}"?`)) {
      this.budgetLineItemService.deleteBudgetLineItem(lineItem.budgetLineItemId).subscribe(() => {
        this.snackBar.open('Line item deleted', 'Close', { duration: 2000 });
        this.getLineItemData();
      });
      let index = this.budgetLineItems.indexOf(lineItem);
      if (index != -1) {
        this.budgetLineItems.splice(index, 1);
      }
      this.budgetLineItems = [...this.budgetLineItems];
      this.getChartData();
    }
  }

  compareCategories = (c1: Category, c2: Category): boolean => {
    return c1 && c2 ? c1.categoryId === c2.categoryId : c1 === c2;
  };

  compareSourceTypes(s1: SourceType, s2: SourceType): boolean {
    return s1 && s2 ? s1.sourceTypeId === s2.sourceTypeId : s1 === s2;
  }

  currentBudgetTotal(): number {
    return this.budgetLineItems.reduce((sum, item) => sum + (item.value || 0), 0);
  }

  addRecurringExpenses(): void {
    const userId = this.currentUser?.userId;
    if (userId == null) {
      this.snackBar.open('No logged in user', 'Close', { duration: 2000 });
      return;
    }
    this.recurringExpenseService.getRecurringExpensesByUserId(userId).subscribe({
      next: (expenses) => {
        const newItems: BudgetLineItems[] = [];

        expenses.forEach(exp => {
          const converted = this.convertRecurringToBudgetItem(exp);

          const isDuplicate = this.budgetLineItems.some(item =>
            item.name === converted.name &&
            item.categoryId === converted.categoryId &&
            item.sourceTypeId === converted.sourceTypeId &&
            item.value === converted.value
          );

          if (!isDuplicate) {
            this.newLineItem = converted
            this.saveNewLineItem();
            newItems.push(converted);
          }
          else {
            this.snackbarQueue.push(`${converted.name} is a duplicate line item.`);
            this.showQueuedSnackbars();
          }
        });

        if (newItems.length > 0) {
          this.budgetLineItems = [...this.budgetLineItems, ...newItems];
        }
      },
      error: (err) => {
        console.error('Failed to load recurring expenses', err);
      }
    });
  }

  private convertRecurringToBudgetItem(exp: RecurringExpense): BudgetLineItems {
    const category = this.categories.find(c => c.categoryId === exp.categoryId);
    const sourceType = this.sourceTypes.find(s => s.sourceTypeId === exp.sourceTypeId);

    if (!category) {
      throw new Error(`Category not found for categoryId ${exp.categoryId}`);
    }

    if (!sourceType) {
      throw new Error(`SourceType not found for sourceTypeId ${exp.sourceTypeId}`);
    }

    return {
      budgetLineItemId: 0,
      categoryId: exp.categoryId,
      value: exp.value,
      budgetId: this.currentBudgetId,
      sourceTypeId: exp.sourceTypeId,
      name: exp.label,
      merchantName: exp.label,
      pending: false,
      transactionId: null,
      pendingTransactionId: null,
      date: new Date().toISOString().split('T')[0],
      plaidAccountId: null,
      userId: this.currentUser?.userId ?? 0,
      createdAt: '',
      updatedAt: '',
      category: category,
      sourceType: sourceType,
    };
  }

  private snackbarQueue: string[] = [];
  private snackbarActive = false;

  private showQueuedSnackbars() {
    if (this.snackbarActive || this.snackbarQueue.length === 0) {
      return;
    }

    this.snackbarActive = true;
    const message = this.snackbarQueue.shift()!;

    const ref = this.snackBar.open(message, 'Close', {
      duration: 3000,
      panelClass: ['stacked-snackbar']
    });

    ref.afterDismissed().subscribe(() => {
      this.snackbarActive = false;
      this.showQueuedSnackbars();
    });
  }

  openTransaction(row: BudgetLineItems) {
    this.router.navigate(['/budget', row.budget?.year, row.budget?.month, row.budgetId, "transaction", row.budgetLineItemId]);
  }

  openAddCategoryDialog(lineItem: BudgetLineItems) {
    const dialogRef = this.dialog.open(AddLookupDialogComponent, {
      width: '400px',
      data: {
        title: 'Add Category',
        fieldLabel: 'Category Name',
        existing: this.categories,
        property: 'categoryName',
        saveFn: (payload: any) => this.categoryService.addCategory(payload)
      }
    });

    dialogRef.afterClosed().subscribe((newCategory: Category) => {
      if (newCategory) {
        this.categories.push(newCategory);
        lineItem.category = newCategory;
        lineItem.categoryId = newCategory.categoryId;
      }
    });
  }

  openAddSourceTypeDialog(lineItem: BudgetLineItems) {
    const dialogRef = this.dialog.open(AddLookupDialogComponent, {
      width: '400px',
      data: {
        title: 'Add Source Type',
        fieldLabel: 'Source Type Name',
        existing: this.sourceTypes,
        property: 'sourceName',
        saveFn: (payload: any) => this.sourceTypeService.addSourceType(payload)
      }
    });

    dialogRef.afterClosed().subscribe((newType: SourceType) => {
      if (newType) {
        this.sourceTypes.push(newType);
        lineItem.sourceType = newType;
        lineItem.sourceTypeId = newType.sourceTypeId;
      }
    });
  }

  get filteredCategories() {
    return this.categories.filter(c =>
      c.categoryName.toLowerCase().includes(this.categoryFilter.toLowerCase())
    );
  }

  get filteredSourceTypes() {
    return this.sourceTypes.filter(s =>
      s.sourceName.toLowerCase().includes(this.sourceTypeFilter.toLowerCase())
    );
  }

}