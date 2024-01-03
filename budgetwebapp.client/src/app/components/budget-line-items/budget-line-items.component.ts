import { Component, Input } from '@angular/core';
import { BudgetLineItems, Category } from '../../app.module';

@Component({
  selector: 'app-budget-line-items',
  templateUrl: './budget-line-items.component.html',
  styleUrls: ['./budget-line-items.component.css']
})
export class BudgetLineItemsComponent {
  @Input() budgetId: string;
  // Fetch budget line items based on this.budgetId
  budgetLineItems: BudgetLineItems[] = []; // Replace with actual data

  // Calculate pie chart data
  categories: Category[] = []; // Replace with actual data
  pieChartData: number[] = []; // Replace with actual data
}
