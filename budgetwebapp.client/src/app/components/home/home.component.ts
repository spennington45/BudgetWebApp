import { Component, OnInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { Budget } from '../../models';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { CreateBudgetDialogComponent } from '../create-budget-dialog/create-budget-dialog.component';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {
  @ViewChild(MatSort) sort: MatSort = new MatSort();

  dataSource: Budget[] = [];
  displayedColumns: string[] = ['date', "total"];
  groupedData: { year: number, data: Budget[] }[] = [];

  ngOnInit() {
    this.dataSource = [
      {
        budgetId: '1', userId: '1', date: new Date('2022-01-01'),
        budgetLineItems: [{
          bugetLineItemId: '1', catigoryId: '1', value: 100, budgetId: '1',
          category: { categoryId: '1', categoryName: 'Category A' }
        }], user: { userId: "1", firstName: "user1", lastName: "user1" }
      },
      {
        budgetId: '2', userId: '2', date: new Date('2023-01-01'),
        budgetLineItems: [{
          bugetLineItemId: '1', catigoryId: '1', value: -150, budgetId: '1',
          category: { categoryId: '1', categoryName: 'Category A' }
        }], user: { userId: "2", firstName: "user2", lastName: "user1" }
      },
      {
        budgetId: '2', userId: '2', date: new Date('2023-02-01'),
        budgetLineItems: [{
          bugetLineItemId: '1', catigoryId: '1', value: -75, budgetId: '1',
          category: { categoryId: '1', categoryName: 'Category A' }
        }], user: { userId: "2", firstName: "user2", lastName: "user1" }
      },
      {
        budgetId: '2', userId: '2', date: new Date('2023-02-02'),
        budgetLineItems: [{
          bugetLineItemId: '1', catigoryId: '1', value: -75, budgetId: '1',
          category: { categoryId: '1', categoryName: 'Category A' }
        }], user: { userId: "2", firstName: "user2", lastName: "user1" }
      },
    ];
    this.groupDataByYear();
  }

  constructor(private router: Router, private dialog: MatDialog) { }

  navigateToBudgetDetails(budget: Budget) {
    const formattedDate = budget.date.toISOString().split('T')[0];
    this.router.navigate(['/budget', formattedDate, budget.budgetId]);
  }
  sumOfBudget(budget: Budget) {
    let sum = budget.budgetLineItems
      .reduce((sum, current) => sum + current.value, 0);
    return sum;
  }

  groupDataByYear() {
    const groupedByYear: { [year: number]: Budget[] } = {};

    this.dataSource.forEach(budget => {
      const year = budget.date.getFullYear();
      if (!groupedByYear[year]) {
        groupedByYear[year] = [];
      }
      groupedByYear[year].push(budget);
    });

    this.groupedData = Object.keys(groupedByYear)
      .map(year => ({
        year: parseInt(year),
        data: groupedByYear[parseInt(year)]
      }))
      .sort((a, b) => b.year - a.year);
  }

  createNewBudget() {
    const dialogRef = this.dialog.open(CreateBudgetDialogComponent, {
      width: '300px',
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        const { year, month } = result;
        const existingBudget = this.dataSource.find(budget => {
          const budgetYear = budget.date.getFullYear();
          const budgetMonth = budget.date.getMonth();
          return budgetYear === year && budgetMonth === month;
        });

        if (existingBudget) {
          console.error('Budget already exists for the selected month and year.');
          // Handle error or display a message to the user
        } else {
          // Create new budget logic...
          console.log('Create new budget:', year, month);
        }
      }
    });
  }
}
