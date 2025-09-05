using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace budgetWebApp.Server.Models;

public partial class RecurringExpense
{
    public long RecurringExpenseId { get; set; }

    public string? Label { get; set; }

    public long CategoryId { get; set; }

    public long? SourceTypeId { get; set; }

    public decimal Value { get; set; }

    public long UserId { get; set; }

    [JsonIgnore]
    public virtual Category Category { get; set; } = null!;

    [JsonIgnore]
    public virtual SourceType? SourceType { get; set; }

    [JsonIgnore]
    public virtual User User { get; set; } = null!;
}
