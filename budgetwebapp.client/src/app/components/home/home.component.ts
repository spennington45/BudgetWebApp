import { Component, OnInit, ViewChild, ChangeDetectorRef } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { Budget, User } from '../../models';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { CreateBudgetDialogComponent } from '../create-budget-dialog/create-budget-dialog.component';
import { MatSnackBar } from '@angular/material/snack-bar';
import { BudgetService } from '../../services/budget.service';
import { PlaidService } from '../../services/plaid.service';
import { AuthService } from '../../services/auth.service';
import { HttpClient } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {
  @ViewChild(MatSort) sort: MatSort = new MatSort();

  loading = true;
  dataSource: Budget[] = [];
  displayedColumns: string[] = ['date', 'total', 'actions'];
  groupedData: { year: number, data: MatTableDataSource<Budget> }[] = [];
  currentUser: User | null = null;

  constructor(
    private cdr: ChangeDetectorRef,
    private router: Router,
    private dialog: MatDialog,
    private snackBar: MatSnackBar,
    private budgetService: BudgetService,
    private plaidService: PlaidService,
    private http: HttpClient,
    private authService: AuthService
  ) {}

  ngOnInit() {
    this.authService.currentUser$.subscribe((user: User | null) => {
      this.currentUser = user;

      if (user) {
        this.getBudgetByUserId();
      } else {
        this.loading = false;
      }
    });
  }

  navigateToBudgetDetails(budget: Budget) {
    const formattedDate = budget.date.toISOString().split('T')[0];
    this.router.navigate(['/budget', formattedDate, budget.budgetId]);
  }

  sumOfBudget(budget: Budget) {
    return budget.budgetLineItems.reduce((sum, current) => sum + current.value, 0);
  }

  async getBudgetByUserId() {
    this.loading = true;
    this.dataSource = [];

    if (!this.currentUser) {
      this.loading = false;
      return;
    }

    try {
      const response = await this.budgetService.getBudgetByUserId(this.currentUser.userId);

      if (response && response.length > 0) {
        this.dataSource = response.map(item => ({
          budgetId: item.budgetId,
          userId: item.userId,
          date: new Date(item.date),
          budgetLineItems: item.budgetLineItems,
          user: item.user
        }));

        this.groupDataByYear();
      } else {
        this.groupedData = [];
      }

    } catch (err) {
      console.error("Error retrieving budgets", err);
    }

    this.loading = false;
    this.cdr.detectChanges();
  }

  launchPlaid() {
    this.http.post<{ link_token: string }>('/Plaid/create-link-token', {})
      .subscribe(response => {
        this.plaidService.openPlaidLink(response.link_token, (publicToken) => {
          this.http.post('/Plaid/exchange', { publicToken }).subscribe();
        });
      });
  }

  groupDataByYear() {
    const groupedByYear: { [year: number]: Budget[] } = {};

    this.dataSource.forEach(budget => {
      const year = budget.date.getFullYear();
      if (!groupedByYear[year]) groupedByYear[year] = [];
      groupedByYear[year].push(budget);
    });

    this.groupedData = Object.keys(groupedByYear)
      .map(year => ({
        year: parseInt(year),
        data: new MatTableDataSource(groupedByYear[parseInt(year)])
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
    dialogConfig.data = { existingBudgets: this.dataSource };

    const dialogRef = this.dialog.open(CreateBudgetDialogComponent, dialogConfig);

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        budget.date = result;
        this.snackBar.open('Budget updated successfully', 'Close', {
          duration: 3000,
          panelClass: ['success']
        });
        this.groupDataByYear();
      }
    });
  }

  createNewBudget() {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.width = '500px';
    dialogConfig.data = { existingBudgets: this.dataSource };

    const dialogRef = this.dialog.open(CreateBudgetDialogComponent, dialogConfig);

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.getBudgetByUserId(); // reload from server
        this.snackBar.open('Budget created successfully', 'Close', {
          duration: 3000,
          panelClass: ['success']
        });
      }
    });
  }
}