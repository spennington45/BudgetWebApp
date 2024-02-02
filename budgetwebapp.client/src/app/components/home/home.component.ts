import { Component, OnInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { Budget } from '../../models';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { CreateBudgetDialogComponent } from '../create-budget-dialog/create-budget-dialog.component';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Observable } from 'rxjs';
import { BudgetService } from '../../services/budget.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {
  @ViewChild(MatSort) sort: MatSort = new MatSort();

  dataSource: Budget[] = [];
  displayedColumns: string[] = ['date', "total", 'actions'];
  groupedData: { year: number, data: Budget[] }[] = [];

  ngOnInit() {
    this.getBudgetByUserId();
  }

  constructor(private router: Router, private dialog: MatDialog, private snackBar: MatSnackBar, private budgetService: BudgetService) { }

  navigateToBudgetDetails(budget: Budget) {
    const formattedDate = budget.date.toISOString().split('T')[0];
    this.router.navigate(['/budget', formattedDate, budget.budgetId]);
  }

  sumOfBudget(budget: Budget) {
    let sum = budget.budgetLineItems
      .reduce((sum, current) => sum + current.value, 0);
    return sum;
  }

  getBudgetByUserId() {
    // TODO Get userId
    this.dataSource = [];
    this.budgetService.getBudgetByUserId('1').subscribe(response => {
      for (let i = 0; i < response.length; i++) {
        this.dataSource.push({
          budgetId: response[i].budgetId,
          userId: response[i].userId,
          date: new Date(response[i].date),
          budgetLineItems: response[i].budgetLineItems,
          user: response[i].user
        })
      }
      this.groupDataByYear();
    });
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

  deleteBudget(budget: Budget) {
    const index = this.dataSource.findIndex(b => b === budget);
    if (index !== -1) {
      this.dataSource.splice(index, 1);
      this.snackBar.open('Budget deleted successfully', 'Close', {
        duration: 3000,
        panelClass: ['success']
      });
      this.groupDataByYear();
    }
  }

  updateBudget(budget: Budget) {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.width = '300px';

    const dialogRef = this.dialog.open(CreateBudgetDialogComponent, dialogConfig);

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        const newDate = result;
        let newBudgetYear = newDate.getFullYear();
        let newBudgetMonth = newDate.getMonth();
        const existingBudget = this.dataSource.find(budget => {
          const budgetYear = budget.date.getFullYear();
          const budgetMonth = budget.date.getMonth();
          return budgetYear === newBudgetYear && budgetMonth === newBudgetMonth;
        });
        if (existingBudget) {
          this.snackBar.open('Budget already exists for the selected month and year.', 'Close', {
            duration: 3000,
            panelClass: ['error']
          });
        }
        else {
          budget.date = result;
          this.snackBar.open('Budget updated successfully', 'Close', {
            duration: 3000,
            panelClass: ['success']
          });
          this.groupDataByYear();
        }
      };
    });
  }

  createNewBudget() {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.width = '300px';

    const dialogRef = this.dialog.open(CreateBudgetDialogComponent, dialogConfig);

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        const newDate = result;
        let newBudgetYear = newDate.getFullYear();
        let newBudgetMonth = newDate.getMonth();
        const existingBudget = this.dataSource.find(budget => {
          const budgetYear = budget.date.getFullYear();
          const budgetMonth = budget.date.getMonth();
          return budgetYear === newBudgetYear && budgetMonth === newBudgetMonth;
        });

        if (existingBudget) {
          this.snackBar.open('Budget already exists for the selected month and year.', 'Close', {
            duration: 3000,
            panelClass: ['error']
          });
        } else {
          let newBudget: Budget = 
          {
            budgetId: '1',
            userId: '1',
            user: {
              userId: '1', firstName: 'test', lastName: 'test'
            },
            budgetLineItems: [],
            date: newDate
          }
          this.dataSource.push(newBudget);
          this.snackBar.open('Budget created successfully', 'Close', {
            duration: 3000,
            panelClass: ['success']
          });
          this.groupDataByYear();
        }
      }
    });
  }
}
