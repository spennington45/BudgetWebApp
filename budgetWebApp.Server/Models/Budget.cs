using System;
using System.Collections.Generic;

namespace budgetWebApp.Server.Models;

public partial class Budget
{
    public long BudgetId { get; set; }

    public int Year { get; set; }

    public int Month { get; set; }

    public long UserId { get; set; }

    public virtual ICollection<BudgetLineItem> BudgetLineItems { get; set; } = new List<BudgetLineItem>();

    public virtual User User { get; set; } = null!;
}
