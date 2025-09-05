using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace budgetWebApp.Server.Models;

public partial class Budget
{
    public long BudgetId { get; set; }

    public DateTime? Date { get; set; }

    public long UserId { get; set; }

    public virtual ICollection<BudgetLineItem> BudgetLineItems { get; set; } = new List<BudgetLineItem>();

    [JsonIgnore]
    public virtual User User { get; set; } = null!;
}
