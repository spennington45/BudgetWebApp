import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { RecurringExpensesComponent } from './components/recurring-expenses/recurring-expenses.component';
import { BudgetDetailsComponent } from './components/budget-details/budget-details.component';

const routes: Routes = [
  {
    path: 'home',
    component: HomeComponent,
  },
  {
    path: '',
    redirectTo: '/home',
    pathMatch: 'full'
  },
  {
    path: 'budget/:date/:id',
    component: BudgetDetailsComponent,
  },
  { 
    path: 'recurring-expenses', 
    component: RecurringExpensesComponent 
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
