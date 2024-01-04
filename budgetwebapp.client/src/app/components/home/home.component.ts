import { Component, OnInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Budget } from '../../models';

import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {
  @ViewChild(MatSort) sort: MatSort = new MatSort();

  dataSource: MatTableDataSource<Budget> = new MatTableDataSource<Budget>();
  displayedColumns: string[] = ['budgetId', 'userId', 'displayDate', 'user'];

  ngOnInit() {
    const budgets: Budget[] = [
      { budgetId: '1', userId: '1', date: new Date('2022-01-01'), budgetLineItems: [], user: { userId: "1", firstName: "user1", lastName: "user1" } },
      { budgetId: '2', userId: '2', date: new Date('2023-01-01'), budgetLineItems: [], user: { userId: "2", firstName: "user2", lastName: "user1" } },
    ];

    this.dataSource = new MatTableDataSource(budgets);
    this.dataSource.sort = this.sort;

    // Add a default sort by date (most recent first)
    this.dataSource.sortingDataAccessor = (item, property) => {
      switch (property) {
        case 'displayDate':
          return item.date instanceof Date ? item.date : new Date(item.date);
        default:
          return item.hasOwnProperty(property) ? item[property] : item['budgetId'];
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
