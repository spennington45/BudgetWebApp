using System;
using System.Collections.Generic;

namespace budgetWebApp.Server.Models;

public partial class BudgetLineItem
{
    public long BugetLineItemId { get; set; }

    public int CatigoryId { get; set; }

    public double? Value { get; set; }

    public long BudgetId { get; set; }

    public virtual Budget Budget { get; set; } = null!;

    public virtual Category Catigory { get; set; } = null!;
}
