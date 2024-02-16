using System;
using System.Collections.Generic;

namespace budgetWebApp.Server.Models;

public partial class RecurringExpense
{
    public long RecurringExpensesId { get; set; }

    public int CategoryId { get; set; }

    public double? Value { get; set; }

    public string? Label { get; set; }

    public int? SourceTypeId { get; set; }

    public long UserId { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual SourceType? SourceType { get; set; }

    public virtual User User { get; set; } = null!;
}
