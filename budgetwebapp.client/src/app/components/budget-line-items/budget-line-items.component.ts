import { Component, Input, OnInit } from '@angular/core';
import { BudgetLineItems, Category, SourceType } from '../../models';
import { ChartData, ChartType } from 'chart.js';
import { MatSnackBar } from '@angular/material/snack-bar';
import { BudgetService } from '../../services/budget.service';

@Component({
  selector: 'app-budget-line-items',
  templateUrl: './budget-line-items.component.html',
  styleUrls: ['./budget-line-items.component.css']
})
export class BudgetLineItemsComponent implements OnInit {
  @Input() budgetId: string = '';
  budgetLineItems: BudgetLineItems[] = [];
  categories: Category[] = [];
  sourceTypes: SourceType[] = [];

  displayedColumns: string[] = ['label', 'value', 'catigory', 'sourceType', 'actions'];

  newLineItem: BudgetLineItems | null = null;
  editingLineItemId: string | null = null;

  pieChartLabels: string[] = [];
  pieChartData: ChartData = {
    labels: [],
    datasets: [{ data: [], label: 'Budget Categories' }]
  };
  pieChartType: ChartType = 'pie';

  constructor(private budgetService: BudgetService, private snackBar: MatSnackBar) { }

  ngOnInit() {
    this.getLineItemData();
    this.getSourceTypes();
  }

  getLineItemData() {
    this.budgetLineItems = [];
    this.budgetService.getBudgetLineItemsByBudgetId(this.budgetId).subscribe(response => {
      this.budgetLineItems = response;

      const existingCategoryIds = new Set(this.categories.map(c => c.categoryId));
      response.forEach(item => {
        if (!existingCategoryIds.has(item.category.categoryId)) {
          this.categories.push(item.category);
          existingCategoryIds.add(item.category.categoryId);
        }
      });

      this.getPieChartData();
    });
  }

  getSourceTypes() {
    this.budgetService.getSourceTypes().subscribe(response => {
      this.sourceTypes = response;
    });
  }

  getPieChartData() {
    this.pieChartLabels = this.categories.map(item => item.categoryName);
    this.pieChartData.labels = this.pieChartLabels;
    this.pieChartData.datasets[0].data = this.calculatePercentages();
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
    const tempId = 'temp-new';
    this.newLineItem = {
      budgetLineItemId: tempId,
      catigoryId: '',
      value: 0,
      budgetId: this.budgetId,
      sourceTypeId: '',
      label: '',
      category: {} as Category,
      sourceType: {} as SourceType
    };
    this.budgetLineItems.unshift(this.newLineItem);
    this.editingLineItemId = tempId;
  }

  isNewLineItemValid(): boolean {
    return !!this.newLineItem &&
      this.newLineItem.label.trim() !== '' &&
      this.newLineItem.value > 0 &&
      !!this.newLineItem.category &&
      !!this.newLineItem.sourceType;
  }

  saveNewLineItem(): void {
    if (!this.isNewLineItemValid() || !this.newLineItem) {
      this.snackBar.open('Please fill out all fields.', 'Close', { duration: 3000 });
      return;
    }

    const { category, sourceType } = this.newLineItem;
    this.newLineItem.catigoryId = category?.categoryId ?? '';
    this.newLineItem.sourceTypeId = sourceType?.sourceTypeId ?? '';

    this.budgetService.createBudgetLineItem(this.budgetId, this.newLineItem).subscribe(() => {
      this.snackBar.open('Line item added successfully', 'Close', { duration: 2000 });
      this.newLineItem = null;
      this.getLineItemData();
    });
  }

  cancelNewLineItem(): void {
    this.newLineItem = null;
  }

  startEdit(lineItem: BudgetLineItems): void {
    this.editingLineItemId = lineItem.budgetLineItemId;
  }

  cancelEdit(): void {
    this.editingLineItemId = null;
  }

  saveEdit(lineItem: BudgetLineItems): void {
    if (lineItem.category) {
      lineItem.catigoryId = lineItem.category.categoryId;
    }
    if (lineItem.sourceType) {
      lineItem.sourceTypeId = lineItem.sourceType.sourceTypeId;
    }

    this.budgetService.updateBudgetLineItem(lineItem.budgetLineItemId, lineItem).subscribe(() => {
      this.snackBar.open('Line item updated successfully', 'Close', { duration: 2000 });
      this.editingLineItemId = null;
      this.getLineItemData();
    });
  }

  deleteLineItem(lineItem: BudgetLineItems): void {
    if (confirm(`Are you sure you want to delete "${lineItem.label}"?`)) {
      this.budgetService.deleteBudgetLineItem(lineItem.budgetLineItemId).subscribe(() => {
        this.snackBar.open('Line item deleted', 'Close', { duration: 2000 });
        this.getLineItemData();
      });
    }
  }
}