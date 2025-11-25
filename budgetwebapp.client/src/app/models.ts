import { NgModule } from '@angular/core';
import { MAT_DATE_FORMATS } from '@angular/material/core';

export interface Budget {
  budgetId: number;
  userId: number;
  date: Date;
  budgetLineItems: Array<BudgetLineItems>;
  user: User;
  displayDate?: string;
  [key: string]: any;
}

export interface APIResponse<T> {
  results: Array<T>;
  totalCount?: number;
  success?: boolean;
  message?: string;
}

export interface BudgetLineItems {
  budgetLineItemId: number;
  categoryId: number;
  value: number;
  budgetId: number;
  sourceTypeId: number;
  label: string;
  category: Category;
  sourceType: SourceType;
}
export interface User {
  userId: number;
  firstName: string;
  lastName: string;
}
export interface Category {
  categoryId: number;
  categoryName: string;
}
export interface SourceType {
  sourceTypeId: number;
  sourceName: string;
}
export interface RecurringExpense {
  recurringExpensesId: number;
  categoryId: number;
  value: string;
  label: string;
  sourceTypeId: number;
  userId: number;
}
export interface BudgetTotal {
  budgetTotalId: number;
  totalValue: number;
  userId: number;
}

export const MY_FORMATS = {
  parse: { dateInput: 'MM/YYYY' },
  display: {
    dateInput: 'MMMM YYYY',
    monthYearLabel: 'MMMM YYYY',
    dateA11yLabel: 'LL',
    monthYearA11yLabel: 'MMMM YYYY',
  },
};

NgModule({
  providers: [{ provide: MAT_DATE_FORMATS, useValue: MY_FORMATS }]
})
