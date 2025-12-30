using System;
using System.Collections.Generic;

namespace budgetWebApp.Server.Models;

public partial class RecurringExpense
{
    public long RecurringExpenseId { get; set; }

    public string? Label { get; set; }

    public long CategoryId { get; set; }

    public long? SourceTypeId { get; set; }

    public decimal Value { get; set; }

    public long UserId { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual SourceType? SourceType { get; set; }

    public virtual User User { get; set; } = null!;
}
