import { Component, OnInit, ViewChild, ChangeDetectorRef, ChangeDetectionStrategy } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { Budget, User } from '../../models';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { CreateBudgetDialogComponent } from '../create-budget-dialog/create-budget-dialog.component';
import { MatSnackBar } from '@angular/material/snack-bar';
import { BudgetService } from '../../services/budget.service';
import { PlaidService } from '../../services/plaid.service';
import { AuthService } from '../../services/auth.service';
import { MatTableDataSource } from '@angular/material/table';
import {
  ApexTitleSubtitle, ApexAxisChartSeries, ApexChart, ApexDataLabels, ApexLegend, ApexNonAxisChartSeries, ApexStroke, ApexXAxis, ApexYAxis, ApexTooltip
} from 'ng-apexcharts';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
  changeDetection: ChangeDetectionStrategy.Eager,
  standalone: false
})
export class HomeComponent implements OnInit {
  @ViewChild(MatSort) sort: MatSort = new MatSort();

  loading = true;
  dataSource: Budget[] = [];
  displayedColumns: string[] = ['month', 'total', 'actions'];
  groupedData: { year: number, data: MatTableDataSource<Budget> }[] = [];
  currentUser: User | null = null;
  public incomeExpenseTrendSeries: ApexAxisChartSeries = [];
  public incomeExpenseTrendChart: ApexChart = {
    type: 'area',
    height: 350,
    toolbar: {
      show: false
    }
  };

  public incomeExpenseTrendXAxis: ApexXAxis = {
    categories: []
  };

  public incomeExpenseTrendStroke: ApexStroke = {
    curve: 'smooth'
  };

  public incomeExpenseTrendLegend: ApexLegend = {
    position: 'bottom'
  };

  public expenseCategorySeries: ApexNonAxisChartSeries = [];
  public expenseCategoryLabels: string[] = [];

  public expenseCategoryChart: ApexChart = {
    type: 'donut',
    height: 350
  };

  public expenseCategoryLegend: ApexLegend = {
    position: 'bottom'
  };

  public monthlyBudgetSeries: ApexAxisChartSeries = [];
  public monthlyBudgetChart: ApexChart = {
    type: 'area',
    height: 350,
    toolbar: {
      show: false
    }
  };

  public monthlyBudgetXAxis: ApexXAxis = {
    categories: []
  };

  public monthlyBudgetStroke: ApexStroke = {
    curve: 'smooth'
  };

  public monthlyBudgetYAxis: ApexYAxis = {
    labels: {
      formatter: (value: number) =>
        `$${value.toLocaleString(undefined, {
          minimumFractionDigits: 0,
          maximumFractionDigits: 2
        })}`
    }
  };

  public incomeExpenseTrendDataLabels: ApexDataLabels = {
    enabled: false,
    formatter: (value: number) =>
      `$${value.toLocaleString(undefined, {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2
      })}`
  };

  public incomeExpenseTrendTooltip: ApexTooltip = {
    y: {
      formatter: (value: number) =>
        `$${value.toLocaleString(undefined, {
          minimumFractionDigits: 2,
          maximumFractionDigits: 2
        })}`
    }
  };

  public monthlyBudgetDataLabels: ApexDataLabels = {
    enabled: false,
    formatter: (value: number) =>
      `$${value.toLocaleString(undefined, {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2
      })}`
  };

  public monthlyBudgetTooltip: ApexTooltip = {
    y: {
      formatter: (value: number) =>
        `$${value.toLocaleString(undefined, {
          minimumFractionDigits: 2,
          maximumFractionDigits: 2
        })}`
    }
  };

  public pieTooltip: ApexTooltip = {
    y: {
      formatter: (value: number) =>
        `$${value.toLocaleString(undefined, {
          minimumFractionDigits: 2,
          maximumFractionDigits: 2
        })}`
    }
  };

  public incomeExpenseTrendYAxis: ApexYAxis = {
    labels: {
      formatter: (value: number) =>
        `$${value.toLocaleString(undefined, {
          minimumFractionDigits: 2,
          maximumFractionDigits: 2
        })}`
    }
  };

  public incomeExpenseTrendTitle: ApexTitleSubtitle = {
    text: 'Income vs Expenses Over Time',
    align: 'center'
  };

  public expenseCategoryTitle: ApexTitleSubtitle = {
    text: 'Expense Category Breakdown',
    align: 'center'
  };

  public monthlyBudgetTitle: ApexTitleSubtitle = {
    text: 'Net Budget Trend',
    align: 'center'
  };

  constructor(
    private cdr: ChangeDetectorRef,
    private router: Router,
    private dialog: MatDialog,
    private snackBar: MatSnackBar,
    private budgetService: BudgetService,
    private plaidService: PlaidService,
    private authService: AuthService
  ) { }

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
    this.router.navigate(['/budget', budget.year, budget.month, budget.budgetId]);
  }

  getMonthName(month: number): string {
    return new Date(2000, month - 1, 1).toLocaleString('en-US', { month: 'long' });
  }

  sumOfBudget(budget: Budget) {
    return budget.budgetLineItems.reduce((sum, current) => sum + current.value, 0);
  }

  getBudgetByUserId() {
    this.loading = true;
    this.dataSource = [];

    if (!this.currentUser) {
      this.loading = false;
      return;
    }

    this.budgetService.getBudgetByUserId(this.currentUser.userId)
      .subscribe({
        next: (response) => {
          if (response && response.length > 0) {
            this.dataSource = response.map(item => ({
              budgetId: item.budgetId,
              userId: item.userId,
              year: item.year,
              month: item.month,
              budgetLineItems: item.budgetLineItems,
              user: item.user
            }));

            this.groupDataByYear();
            this.buildDashboardCharts();
          } else {
            this.groupedData = [];
          }
        },
        error: (err) => {
          console.error("Error retrieving budgets", err);
        },
        complete: () => {
          this.loading = false;
          this.cdr.detectChanges();
        }
      });
  }

  launchPlaid() {
    if (!this.currentUser) {
      this.snackBar.open('No user logged in', 'Close', { duration: 5000 });
      return;
    }

    const userId = this.currentUser.userId.toString();

    this.plaidService.createLinkToken(userId).subscribe(res => {
      this.plaidService.openPlaidLink(res.link_token, (publicToken: string, metadata: any) => {

        const payload = {
          userId: userId,
          publicToken: publicToken,
          institutionName: metadata.institution?.name,
          accounts: metadata.accounts.map((a: any) => ({
            accountId: a.id,
            name: a.name,
            mask: a.mask,
            type: a.type,
            subtype: a.subtype,
            officialName: a.official_name
          }))
        };

        this.plaidService.addPlaidDataLink(payload).subscribe({
          next: () => {
            this.snackBar.open('Accounts linked successfully', 'Close', { duration: 5000 });
          },
          error: () => {
            this.snackBar.open('Failed to link account', 'Close', { duration: 5000 });
          }
        });
      });
    });
  }

  groupDataByYear() {
    const groupedByYear: { [year: number]: Budget[] } = {};

    this.dataSource.forEach(budget => {
      const year = budget.year;
      if (!groupedByYear[year]) groupedByYear[year] = [];
      groupedByYear[year].push(budget);
    });


    this.groupedData = Object.keys(groupedByYear)
      .map(year => ({
        year: parseInt(year),
        data: new MatTableDataSource(
          groupedByYear[parseInt(year)]
            .sort((a, b) => b.month - a.month)
        )
      }))
      .sort((a, b) => b.year - a.year);
  }

  deleteBudget(budget: Budget) {
    this.budgetService.deleteBudget(budget.budgetId).subscribe({
      next: () => {
        const index = this.dataSource.findIndex(b => b.budgetId === budget.budgetId);
        if (index !== -1) {
          this.dataSource.splice(index, 1);
        }

        this.snackBar.open('Budget deleted successfully', 'Close', {
          duration: 3000,
          panelClass: ['success']
        });

        this.dataSource.filter(x => x.budgetId != budget.budgetId);
        this.groupDataByYear();
      },
      error: err => {
        console.error('Failed to delete budget:', err);
        this.snackBar.open('Failed to delete budget', 'Close', {
          duration: 3000,
          panelClass: ['error']
        });
      }
    });
  }

  updateBudget(budget: Budget) {
    const dialogConfig = new MatDialogConfig();
    dialogConfig.width = '300px';
    dialogConfig.data = {
      existingBudgets: this.dataSource,
      budgetToEdit: budget
    };

    const dialogRef = this.dialog.open(CreateBudgetDialogComponent, dialogConfig);

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        Object.assign(budget, result);

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
        this.getBudgetByUserId();
        this.snackBar.open('Budget created successfully', 'Close', {
          duration: 3000,
          panelClass: ['success']
        });
      }
    });
  }

  getTotalOfAllBudgets(): number {
    return this.dataSource.reduce((overall, budget) => {
      return overall + this.sumOfBudget(budget);
    }, 0);
  }

  navigateToRecurringExpenses() {
    this.router.navigate(['/recurring-expenses']);
  }

  syncPlaidItems() {
    if (!this.currentUser) {
      this.snackBar.open('No user logged in', 'Close', { duration: 3000 });
      return;
    }

    const payload = {
      userId: this.currentUser.userId
    };

    this.plaidService.syncPlaidItemsByUserId(payload).subscribe({
      next: () => {
        this.snackBar.open('Plaid items synced successfully', 'Close', { duration: 3000 });
        this.getBudgetByUserId();
      },
      error: (err) => {
        console.error(err);
        this.snackBar.open('Failed to sync Plaid items', 'Close', { duration: 3000 });
      }
    });
  }

  buildDashboardCharts() {
    const sortedBudgets = [...this.dataSource].sort((a, b) => {
      if (a.year !== b.year) {
        return a.year - b.year;
      }

      return a.month - b.month;
    });

    const monthLabels: string[] = [];
    const incomeData: number[] = [];
    const expenseData: number[] = [];
    const netBudgetData: number[] = [];

    const expenseCategoryMap = new Map<string, number>();

    sortedBudgets.forEach(budget => {
      monthLabels.push(
        `${this.getMonthName(budget.month)} ${budget.year}`
      );

      let monthlyIncome = 0;
      let monthlyExpense = 0;

      budget.budgetLineItems.forEach(item => {
        if (item.value > 0) {
          monthlyIncome += item.value;
        } else {
          monthlyExpense += Math.abs(item.value);

          const categoryName =
            item.category?.categoryName ?? 'Unknown';

          expenseCategoryMap.set(
            categoryName,
            (expenseCategoryMap.get(categoryName) || 0) +
            Math.abs(item.value)
          );
        }
      });

      incomeData.push(monthlyIncome);
      expenseData.push(monthlyExpense);
      netBudgetData.push(monthlyIncome - monthlyExpense);
    });

    this.incomeExpenseTrendXAxis = {
      categories: monthLabels
    };

    this.incomeExpenseTrendSeries = [
      {
        name: 'Income',
        data: incomeData
      },
      {
        name: 'Expenses',
        data: expenseData
      }
    ];

    this.monthlyBudgetXAxis = {
      categories: monthLabels
    };

    this.monthlyBudgetSeries = [
      {
        name: 'Net Budget',
        data: netBudgetData
      }
    ];

    this.expenseCategoryLabels = [...expenseCategoryMap.keys()];
    this.expenseCategorySeries = [...expenseCategoryMap.values()];
  }

  getTotalIncome(): number {
    return this.dataSource
      .flatMap(x => x.budgetLineItems)
      .filter(x => x.value > 0 && !x.isTransfer)
      .reduce((sum, x) => sum + x.value, 0);
  }

  getTotalExpenses(): number {
    return this.dataSource
      .flatMap(x => x.budgetLineItems)
      .filter(x => x.value < 0 && !x.isTransfer)
      .reduce((sum, x) => sum + Math.abs(x.value), 0);
  }

  getSavings(): number {
    return this.getTotalIncome() - this.getTotalExpenses();
  }

  private formatCurrency(value: number): string {
    return `$${value.toLocaleString(undefined, {
      minimumFractionDigits: 2,
      maximumFractionDigits: 2
    })}`;
  }

  navigateToAccountDashboard() {
    this.router.navigate(['/account-dashboard']);
  }
}