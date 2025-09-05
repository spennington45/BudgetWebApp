using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace budgetWebApp.Server.Models;

public partial class BudgetTotal
{
    public long BudgetTotalId { get; set; }

    public long UserId { get; set; }

    public decimal TotalValue { get; set; }

    [JsonIgnore]
    public virtual User User { get; set; } = null!;
}
