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
  bugetLineItemId: string;
  catigoryId: string;
  value: string;
  budgetId: string;
  category: Category;
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
