using System;
using System.Collections.Generic;

namespace budgetWebApp.Server.Models;

public partial class PlaidItem
{
    public long PlaidItemId { get; set; }

    public long UserId { get; set; }

    public string ItemId { get; set; } = null!;

    public string AccessToken { get; set; } = null!;

    public string? InstitutionName { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<PlaidAccount> PlaidAccounts { get; set; } = new List<PlaidAccount>();

    public virtual User User { get; set; } = null!;
}
