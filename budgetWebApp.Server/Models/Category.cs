using System;
using System.Collections.Generic;

namespace budgetWebApp.Server.Models;

public partial class Category
{
    public long CategoryId { get; set; }

    public string? CategoryName { get; set; }

    public virtual ICollection<BudgetLineItem> BudgetLineItems { get; set; } = new List<BudgetLineItem>();

    public virtual ICollection<RecurringExpense> RecurringExpenses { get; set; } = new List<RecurringExpense>();
}
