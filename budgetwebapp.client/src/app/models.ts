export interface Budget {
  budgetId: string;
  userId: string;
  date: Date;
  budgetLineItems: Array<BudgetLineItems>;
  user: User;
  displayDate?: string;
  [key: string]: any;
}

export interface APIResponse<T> {
  results: Array<T>;
}

export interface BudgetLineItems {
  budgetLineItemId: string;
  catigoryId: string;
  value: number;
  budgetId: string;
  sourceTypeId: string;
  category: Category;
  sourceType: SourceType;
}
export interface User {
  userId: string;
  firstName: string;
  lastName: string;
}
export interface Category {
  categoryId: string;
  categoryName: string;
}
export interface SourceType {
  sourceTypeId: string;
  sourceName: string;
}
export interface RecurringExpense {
  recurringExpensesId: string;
  categoryId: string;
  value: string;
  label: string;
  sourceTypeId: string;
  userId: string;
}
export interface BudgetTotal {
  budgetTotalId: string;
  totalValue: string;
  userId: string;
}
