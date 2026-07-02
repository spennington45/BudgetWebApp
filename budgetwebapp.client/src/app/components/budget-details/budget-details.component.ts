import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';


@Component({
  selector: 'app-budget-details',
  templateUrl: './budget-details.component.html',
  styleUrl: './budget-details.component.css',
  changeDetection: ChangeDetectionStrategy.Eager,
  standalone: false
})
export class BudgetDetailsComponent implements OnInit {
  budgetId: string = "";
  date: string = "";

  constructor(private route: ActivatedRoute) { }

  formattedDate: string = "";

  ngOnInit() {
    this.route.params.subscribe(params => {
      const month = Number(params['month']);
      const year = Number(params['year']);

      this.budgetId = params['id'];

      const parsed = new Date(year, month - 1, 1);

      this.formattedDate = parsed.toLocaleString('default', {
        month: 'long',
        year: 'numeric'
      });
    });
  }
}
