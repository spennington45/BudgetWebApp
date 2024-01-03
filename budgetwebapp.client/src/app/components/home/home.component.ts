import { Component, OnInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Budget } from '../../app.module';

import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {
  @ViewChild(MatSort) sort: MatSort;

  dataSource: MatTableDataSource<Budget>;
  displayedColumns: string[] = ['budgetId', 'userId', 'displayDate', 'user']; // Add 'displayDate'

  ngOnInit() {
    const budgets: Budget[] = [
      { budgetId: '1', userId: 'user1', date: new Date('2022-01-01'), budgetLineItems: [], user: { /* ... */ } },
      { budgetId: '2', userId: 'user2', date: new Date('2023-01-01'), budgetLineItems: [], user: { /* ... */ } },
      // Add more budgets as needed
    ];

    this.dataSource = new MatTableDataSource(budgets);
    this.dataSource.sort = this.sort;

    // Add a default sort by date (most recent first)
    this.dataSource.sortingDataAccessor = (item, property) => {
      switch (property) {
        case 'displayDate': return new Date(item.date);
        default: return item[property];
      }
    };
    this.dataSource.sort.direction = 'desc';
    this.dataSource.sort.active = 'displayDate';
  }

  constructor(private router: Router) { }

  navigateToBudgetDetails(budget: Budget) {
    const formattedDate = budget.date.toISOString().split('T')[0];
    this.router.navigate(['/budget', formattedDate, budget.budgetId]);
  }
}
