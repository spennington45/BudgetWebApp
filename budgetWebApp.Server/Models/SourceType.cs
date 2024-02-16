using System;
using System.Collections.Generic;

namespace budgetWebApp.Server.Models;

public partial class SourceType
{
    public int SourceTypeId { get; set; }

    public string SourceName { get; set; } = null!;

    public virtual ICollection<BudgetLineItem> BudgetLineItems { get; set; } = new List<BudgetLineItem>();

    public virtual ICollection<RecurringExpense> RecurringExpenses { get; set; } = new List<RecurringExpense>();
}
