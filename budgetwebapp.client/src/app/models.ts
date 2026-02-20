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
  externalId: string;
  name: string;
  email: string;
  pictureUrl?: string;
  token: string;
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
  recurringExpenseId: number;
  categoryId: number;
  value: number;
  label: string;
  sourceTypeId: number;
  userId: number;
  category?: Category | null;
  sourceType?: SourceType | null;
  user: User;
}
export interface BudgetTotal {
  budgetTotalId: number;
  totalValue: number;
  userId: number;
}
export interface GoogleTokenRequest {
  idToken: string;
}
export interface Jwt {
  userId: string;
  externalId: string;
  email: string;
  name: string;
  picture?: string;
  exp: number;
}
export interface GooglePayload {
  iss: string;
  azp: string;
  aud: string;
  sub: string;
  email: string;
  email_verified: string;
  name: string;
  picture: string;
  given_name: string;
  family_name: string;
  iat: number;
  exp: number;
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
