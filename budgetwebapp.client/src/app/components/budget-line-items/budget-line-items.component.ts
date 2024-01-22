import { Component, Input } from '@angular/core';
import { BudgetLineItems, Category } from '../../models';
import { ChartData, ChartType } from 'chart.js';

@Component({
  selector: 'app-budget-line-items',
  templateUrl: './budget-line-items.component.html',
  styleUrls: ['./budget-line-items.component.css']
})
export class BudgetLineItemsComponent {
  @Input() budgetId: string = "";
  budgetLineItems: BudgetLineItems[] = [];
  categories: Category[] = [];
  displayedColumns: string[] = ['bugetLineItemId', 'catigory', 'value']; 
  // Pie chart data
  pieChartLabels: string[] = [];
  pieChartData: ChartData = { labels: [], datasets: [{ data: [], label: 'Budget Categories' }] };
  pieChartType: ChartType = 'pie';

  ngOnInit() {
    // Fetch actual data for budget line items and categories
    // For now, let's use placeholder data
    this.budgetLineItems = [
      { bugetLineItemId: '1', catigoryId: '1', value: '100', budgetId: '1', category: { categoryId: '1', categoryName: 'Category A' } },
      { bugetLineItemId: '2', catigoryId: '2', value: '150', budgetId: '1', category: { categoryId: '2', categoryName: 'Category B' } },
    ];

    this.categories = [
      { categoryId: '1', categoryName: 'Category A' },
      { categoryId: '2', categoryName: 'Category B' },
      // Add more categories as needed
    ];

    // Populate placeholder data for the pie chart
    this.pieChartLabels = this.categories.map(category => category.categoryName);
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
