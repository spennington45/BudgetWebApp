import { Component, Input, OnInit } from '@angular/core';
import { Budget, BudgetLineItems, Category, SourceType } from '../../models';
import { ChartData, ChartType, ChartOptions, ChartDataset } from 'chart.js';
import { MatSnackBar } from '@angular/material/snack-bar';
import { BudgetLineItemService } from '../../services/budget-line-item.service';
import { SourceTypeService } from '../../services/source-type.service';
import { CategoryService } from '../../services/category.service';

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
  categories: Category[] = [];
  sourceTypes: SourceType[] = [];
  barChartLabels = ['Income', 'Expenses'];
  barChartData: ChartData<'bar'> = {
    labels: [],
    datasets: [
      {
        label: 'Expenses',
        data: [],
        backgroundColor: [],
        yAxisID: 'y',
      },
      {
        label: 'Income (100%)',
        data: [],
        backgroundColor: '#4CAF50',
        yAxisID: 'y1',
      }
    ]
  };

  barChartOptions: ChartOptions<'bar'> = {
    responsive: true,
    scales: {
      y: {
        beginAtZero: true,
        title: {
          display: true,
          text: 'Amount ($)'
        }
      },
      y1: {
        beginAtZero: true,
        position: 'right',
        title: {
          display: true,
          text: 'Income %'
        },
        ticks: {
          callback: value => `${value}%`
        },
        grid: {
          drawOnChartArea: false
        }
      }
    }
  };
  barChartType: 'bar' = 'bar';

  displayedColumns: string[] = ['label', 'value', 'category', 'sourceType', 'actions'];

  newLineItem: BudgetLineItems | null = null;
  editingLineItemId: number | null = null;

  pieChartLabels: string[] = [];
  pieChartData: ChartData<'pie', number[], string> = {
    labels: [],
    datasets: [{ data: [], label: 'Budget Categories' }]
  };
  pieChartType: ChartType = 'pie';
  pieChartOptions: ChartOptions<'pie'> = {
  responsive: true,
  maintainAspectRatio: false,
  plugins: {
    legend: {
      position: 'bottom'
    },
    tooltip: {
      callbacks: {
        label: (context) => {
          const label = context.label || '';
          const value = context.raw as number;
          return `${label}: $${value.toFixed(2)}`;
        }
      }
    }
  }
};

  constructor(
    private budgetLineItemService: BudgetLineItemService, 
    private snackBar: MatSnackBar, 
    private categoryService: CategoryService, 
    private sourceTypeService: SourceTypeService) { }

  ngOnInit() {
    this.getLineItemData();
    this.getSourceTypes();
    this.getCategories();
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
    const expenseItems = this.budgetLineItems.filter(item => item.value <= 0);
    const incomeItems = this.budgetLineItems.filter(item => item.value > 0);

    const expenseMap = new Map<string, number>();
    const incomeMap = new Map<string, number>();

    expenseItems.forEach(item => {
      const catName = item.category?.categoryName ?? 'Unknown';
      const currentTotal = expenseMap.get(catName) || 0;
      expenseMap.set(catName, currentTotal + Math.abs(item.value));
    });

    incomeItems.forEach(item => {
      const catName = item.category?.categoryName ?? 'Unknown';
      const currentTotal = incomeMap.get(catName) || 0;
      incomeMap.set(catName, currentTotal + item.value);
    });

    this.pieChartLabels = Array.from(expenseMap.keys());
    this.pieChartData.labels = this.pieChartLabels;
    this.pieChartData.datasets[0].data = Array.from(expenseMap.values());
    this.pieChartData.datasets[0].label = 'Expense Breakdown';
    this.pieChartData.datasets[0].backgroundColor = this.generateColors(this.pieChartLabels.length);

    const barDatasets: ChartDataset<'bar'>[] = [];

    incomeMap.forEach((value, category) => {
      barDatasets.push({
        label: category,
        data: [value, 0],
        backgroundColor: this.getColorForCategory(category),
        stack: 'income'
      });
    });

    expenseMap.forEach((value, category) => {
      barDatasets.push({
        label: category,
        data: [0, value], 
        backgroundColor: this.getColorForCategory(category),
        stack: 'expense'
      });
    });

    this.barChartData = {
      labels: this.barChartLabels,
      datasets: barDatasets
    };

    this.pieChartData = { ...this.pieChartData };
    this.barChartData = { ...this.barChartData };
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
      budgetLineItemId: tempId,
      categoryId: 0,
      value: 0,
      budgetId: Number(this.budgetId),
      sourceTypeId: 0,
      label: '',
      category: {} as Category,
      sourceType: {} as SourceType,
    };
    this.budgetLineItems.unshift(this.newLineItem);
    this.editingLineItemId = tempId;
    this.budgetLineItems = [...this.budgetLineItems];
  }

  isNewLineItemValid(): boolean {
    return !!this.newLineItem &&
      this.newLineItem.label.trim() !== '' &&
      this.newLineItem.value != 0 &&
      !!this.newLineItem.category &&
      !!this.newLineItem.sourceType;
  }

  saveNewLineItem(): void {
  if (!this.isNewLineItemValid() || !this.newLineItem) {
    this.snackBar.open('Please fill out all fields.', 'Close', { duration: 3000 });
    return;
  }

  const { category, sourceType } = this.newLineItem;
  this.newLineItem.categoryId = category?.categoryId ?? 0;
  this.newLineItem.sourceTypeId = sourceType?.sourceTypeId ?? 0;

  const tempId = this.newLineItem.budgetLineItemId;

  this.budgetLineItemService.addBudgetLineItem(this.newLineItem).subscribe({
    next: (response) => {
      this.snackBar.open('Line item added successfully', 'Close', { duration: 2000 });
      const index = this.budgetLineItems.findIndex(item => item.budgetLineItemId === tempId);
      if (index !== -1) {
        this.budgetLineItems[index].budgetLineItemId = response.budgetLineItemId;
      }
      this.newLineItem = null;
    },
    error: (err) => {
      this.snackBar.open('Failed to save line item.', 'Close', { duration: 3000 });
      const index = this.budgetLineItems.findIndex(item => item.budgetLineItemId === tempId);
      if (index !== -1) {
        this.budgetLineItems.splice(index, 1);
        this.budgetLineItems = [...this.budgetLineItems];
      }
      this.newLineItem = null;
    }
  });

  this.getChartData();
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
      lineItem.categoryId = lineItem.category.categoryId;
    }
    if (lineItem.sourceType) {
      lineItem.sourceTypeId = lineItem.sourceType.sourceTypeId;
    }

    this.budgetLineItemService.updateBudgetLineItem(lineItem).subscribe(() => {
      this.snackBar.open('Line item updated successfully', 'Close', { duration: 2000 });
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
    if (confirm(`Are you sure you want to delete "${lineItem.label}"?`)) {
      // this.budgetLineItemService.deleteBudgetLineItem(lineItem.budgetLineItemId).subscribe(() => {
      //   this.snackBar.open('Line item deleted', 'Close', { duration: 2000 });
      //   this.getLineItemData();
      // });
      let index = this.budgetLineItems.indexOf(lineItem);
      if (index != -1) {
        this.budgetLineItems.splice(index, 1);
      }
      this.budgetLineItems = [...this.budgetLineItems];
      this.getChartData();
    }
  }
}