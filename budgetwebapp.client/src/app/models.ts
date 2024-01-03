export interface Budget {
  budgetId: string;
  userId: string;
  date: Date;
  budgetLineItems: Array<BudgetLineItems>;
  user: User;
  displayDate?: string;
}

export interface APIResponse<T> {
  results: Array<T>;
}

interface BudgetLineItems {
  bugetLineItemId: string;
  catigoryId: string;
  value: string;
  budgetId: string;
  category: Category;
}
interface User {
  userId: string;
  firstName: string;
  lastName: string;
}
interface Category {
  categoryId: string;
  categoryName: string;
}
