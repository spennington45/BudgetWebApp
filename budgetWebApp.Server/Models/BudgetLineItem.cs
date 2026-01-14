using System.Text.Json.Serialization;

namespace budgetWebApp.Server.Models;

public partial class BudgetLineItem
{
    public long BudgetLineItemId { get; set; }

    public string? Label { get; set; }

    public long BudgetId { get; set; }

    public long CategoryId { get; set; }

    public decimal Value { get; set; }

    public long? SourceTypeId { get; set; }

    [JsonIgnore]
    public virtual Budget? Budget { get; set; } = null!;

    public virtual Category Category { get; set; } = null!;

    public virtual SourceType? SourceType { get; set; }
}
