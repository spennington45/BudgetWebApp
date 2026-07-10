import { Component, OnInit } from '@angular/core';
import {
  ApexAxisChartSeries,
  ApexChart,
  ApexDataLabels,
  ApexLegend,
  ApexNonAxisChartSeries,
  ApexPlotOptions,
  ApexTooltip,
  ApexXAxis,
  ApexYAxis,
  ApexTitleSubtitle
} from 'ng-apexcharts';
import { ChangeDetectorRef } from '@angular/core';
import { PlaidService } from '../../services/plaid.service';
import { AuthService } from '../../services/auth.service';
import { PlaidAccount, User } from '../../models';

@Component({
  selector: 'app-account-dashboard',
  templateUrl: './account-dashboard.component.html',
  styleUrls: ['./account-dashboard.component.css'],
  standalone: false
})
export class AccountDashboardComponent implements OnInit {

  accounts: PlaidAccount[] = [];
  currentUser: User | null = null;

  displayedColumns: string[] = [
    'name',
    'type',
    'subtype',
    'currentBalance',
    'availableBalance'
  ];

  totalAssets = 0;
  totalDebt = 0;
  netWorth = 0;
  totalCash = 0;

  public accountBalanceSeries: ApexAxisChartSeries = [];
  public accountBalanceChart: ApexChart = {
    type: 'bar',
    height: 400
  };

  public accountBalancePlotOptions: ApexPlotOptions = {
    bar: {
      horizontal: true,
      distributed: true
    }
  };

  public accountBalanceXAxis: ApexXAxis = {
    categories: []
  };

  public accountBalanceYAxis: ApexYAxis = {
    labels: {
      formatter: (value: number) =>
        `$${value.toLocaleString(undefined, {
          minimumFractionDigits: 2,
          maximumFractionDigits: 2
        })}`
    }
  };

  public accountBalanceTooltip: ApexTooltip = {
    y: {
      formatter: (value: number) =>
        `$${value.toLocaleString(undefined, {
          minimumFractionDigits: 2,
          maximumFractionDigits: 2
        })}`
    }
  };

  public accountBalanceDataLabels: ApexDataLabels = {
    enabled: true,
    formatter: (value: number) =>
      `$${value.toLocaleString(undefined, {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2
      })}`
  };

  public accountBalanceTitle: ApexTitleSubtitle = {
    text: 'Account Balances',
    align: 'center'
  };

  public accountTypeSeries: ApexNonAxisChartSeries = [];

  public accountTypeLabels: string[] = [];

  public accountTypeChart: ApexChart = {
    type: 'donut',
    height: 400
  };

  public accountTypeLegend: ApexLegend = {
    position: 'bottom'
  };

  public accountTypeTooltip: ApexTooltip = {
    y: {
      formatter: (value: number) =>
        `$${value.toLocaleString(undefined, {
          minimumFractionDigits: 2,
          maximumFractionDigits: 2
        })}`
    }
  };

  public accountTypeTitle: ApexTitleSubtitle = {
    text: 'Account Type Distribution',
    align: 'center'
  };

  constructor(
    private plaidService: PlaidService,
    private authService: AuthService,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit(): void {
    this.authService.currentUser$.subscribe(user => {
      if (user) {
        this.currentUser = user;
        this.loadAccounts(this.currentUser.userId);
      }
    });
  }

  loadAccounts(userId: number): void {
    this.plaidService.getAccountsByUserId(userId)
      .subscribe(accounts => {
        this.accounts = accounts;

        this.calculateSummary();
        this.buildCharts();

        this.cdr.detectChanges();
      });
  }

  calculateSummary(): void {

    this.totalAssets = this.accounts
      .filter(a => (a.currentBalance ?? 0) > 0)
      .reduce((sum, a) => sum + (a.currentBalance ?? 0), 0);

    this.totalDebt = this.accounts
      .filter(a => (a.currentBalance ?? 0) < 0)
      .reduce((sum, a) => sum + Math.abs(a.currentBalance ?? 0), 0);

    this.totalCash = this.accounts
      .filter(a => a.type === 'depository')
      .reduce((sum, a) => sum + (a.currentBalance ?? 0), 0);

    this.netWorth = this.totalAssets - this.totalDebt;
  }

  buildCharts(): void {

    this.accountBalanceXAxis = {
      categories: this.accounts.map(a => a.name)
    };

    this.accountBalanceSeries = [
      {
        name: 'Balance',
        data: this.accounts.map(a => a.currentBalance ?? 0)
      }
    ];

    const accountTypeTotals = new Map<string, number>();

    this.accounts.forEach(account => {
      const key = account.type ?? 'Unknown';

      accountTypeTotals.set(
        key,
        (accountTypeTotals.get(key) || 0) +
        Math.abs(account.currentBalance ?? 0)
      );
    });

    this.accountTypeLabels = [...accountTypeTotals.keys()];
    this.accountTypeSeries = [...accountTypeTotals.values()];
  }
}