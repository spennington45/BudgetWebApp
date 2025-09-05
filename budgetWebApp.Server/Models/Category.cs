using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace budgetWebApp.Server.Models;

public partial class Category
{
    public long CategoryId { get; set; }

    public string? CategoryName { get; set; }

    [JsonIgnore]
    public virtual ICollection<BudgetLineItem> BudgetLineItems { get; set; } = new List<BudgetLineItem>();

    [JsonIgnore]
    public virtual ICollection<RecurringExpense> RecurringExpenses { get; set; } = new List<RecurringExpense>();
}
