import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';


@Component({
  selector: 'app-budget-details',
  templateUrl: './budget-details.component.html',
  styleUrl: './budget-details.component.css'
})
export class BudgetDetailsComponent implements OnInit {
  budgetId: string = "";
  date: string = "";

  constructor(private route: ActivatedRoute) { }

  formattedDate: string = "";

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.date = params['date'];
      this.budgetId = params['id'];

      const [year, month, day] = this.date.split('-').map(Number);
      const parsed = new Date(year, month - 1, day);

      this.formattedDate = parsed.toLocaleString('default', {
        month: 'long',
        year: 'numeric'
      });
    });
  }
}
