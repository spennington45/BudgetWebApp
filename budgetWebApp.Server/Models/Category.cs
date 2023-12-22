using System;
using System.Collections.Generic;

namespace budgetWebApp.Server.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public virtual ICollection<BudgetLineItem> BudgetLineItems { get; set; } = new List<BudgetLineItem>();
}
