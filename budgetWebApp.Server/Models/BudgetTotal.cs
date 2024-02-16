using System;
using System.Collections.Generic;

namespace budgetWebApp.Server.Models;

public partial class BudgetTotal
{
    public long BudgetTotalId { get; set; }

    public double TotalValue { get; set; }

    public long UserId { get; set; }

    public virtual User User { get; set; } = null!;
}
