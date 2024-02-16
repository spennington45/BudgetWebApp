import { Component, Input } from '@angular/core';
import { BudgetLineItems, Category } from '../../models';
import { ChartData, ChartType } from 'chart.js';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { BudgetService } from '../../services/budget.service';

@Component({
  selector: 'app-budget-line-items',
  templateUrl: './budget-line-items.component.html',
  styleUrls: ['./budget-line-items.component.css']
})

export class BudgetLineItemsComponent {
  @Input() budgetId: string = "";
  budgetLineItems: BudgetLineItems[] = [];
  categories: Category[] = [];
  displayedColumns: string[] = ['label', 'value', 'catigory', 'sourceType', ]; 
  // Pie chart data
  pieChartLabels: string[] = [];
  pieChartData: ChartData = { labels: [], datasets: [{ data: [], label: 'Budget Categories' }] };
  pieChartType: ChartType = 'pie';

  constructor(private router: Router, private dialog: MatDialog, private snackBar: MatSnackBar, private budgetService: BudgetService) { }

  ngOnInit() {
    // TODO Fetch actual data for budget line items and categories
    this.getLineItemData();
    //this.getPieChartData();
  }

  getLineItemData() {
    this.budgetLineItems = [];
    this.budgetService.getBudgetLineItemsByBudgetId(this.budgetId).subscribe(response => {
      this.budgetLineItems = response;
      // TODO Figure out a better way to do this ewww....
      for (let i = 0; i < response.length; i++) {
        let exists = false;
        for (let j = 0; j < this.categories.length; j++) {
          if (this.categories[j].categoryId === response[i].category.categoryId) {
            exists = true;
            break;
          }
        }
        if (!exists) {
          this.categories.push(response[i].category);
        }
      }
      this.getPieChartData();
    });
  }

  getPieChartData() {
    // Populate placeholder data for the pie chart
    this.pieChartLabels = this.categories.map(item => item.categoryName);
    this.pieChartData.labels = this.pieChartLabels;
    this.pieChartData.datasets[0].data = this.calculatePercentages();
  }

  calculatePercentages(): number[] {
    const categoryTotals: { [categoryId: string]: number } = {};

    // Calculate the total value for each category
    this.budgetLineItems.forEach(item => {
      if (categoryTotals[item.category.categoryId] === undefined) {
        categoryTotals[item.category.categoryId] = 0;
      }
      categoryTotals[item.category.categoryId] += +item.value;
    });

    // Calculate the percentage for each category
    const percentages: number[] = [];
    this.categories.forEach(category => {
      const total = categoryTotals[category.categoryId] || 0;
      percentages.push(total);
    });

    return percentages;
  }
}
