using System;
using System.Collections.Generic;

namespace budgetWebApp.Server.Models;

public partial class PlaidSyncCursor
{
    public long PlaidItemId { get; set; }

    public string? Cursor { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual PlaidItem PlaidItem { get; set; } = null!;
}
