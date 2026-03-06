using System;
using System.Collections.Generic;

namespace budgetWebApp.Server.Models;

public partial class PlaidAccount
{
    public long PlaidAccountId { get; set; }

    public long PlaidItemId { get; set; }

    public string AccountId { get; set; } = null!;

    public string? Name { get; set; }

    public string? Mask { get; set; }

    public string? Type { get; set; }

    public string? Subtype { get; set; }

    public string? OfficialName { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual PlaidItem PlaidItem { get; set; } = null!;
}
