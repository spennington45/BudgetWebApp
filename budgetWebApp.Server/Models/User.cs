using System;
using System.Collections.Generic;

namespace budgetWebApp.Server.Models;

public partial class User
{
    public long UserId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public virtual ICollection<Budget> Budgets { get; set; } = new List<Budget>();
}
