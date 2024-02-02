using System;
using System.Collections.Generic;

namespace budgetWebApp.Server.Models;

public partial class BudgetLineItem
{
    public long BugetLineItemId { get; set; }

    public int CategoryId { get; set; }

    public double? Value { get; set; }

    public long BudgetId { get; set; }

    public string? Label { get; set; }

    public virtual Budget Budget { get; set; } = null!;

    public virtual Category Category { get; set; } = null!;
}
