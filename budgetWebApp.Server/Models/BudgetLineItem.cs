using System;
using System.Collections.Generic;

namespace budgetWebApp.Server.Models;

public partial class BudgetLineItem
{
    public long BudgetLineItemId { get; set; }

    public long BudgetId { get; set; }

    public string? TransactionId { get; set; }

    public string? PendingTransactionId { get; set; }

    public DateOnly Date { get; set; }

    public decimal Value { get; set; }

    public string? Name { get; set; }

    public string? MerchantName { get; set; }

    public bool Pending { get; set; }

    public long CategoryId { get; set; }

    public long? PlaidAccountId { get; set; }

    public long UserId { get; set; }

    public long SourceTypeId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Budget Budget { get; set; } = null!;

    public virtual Category Category { get; set; } = null!;

    public virtual PlaidAccount? PlaidAccount { get; set; }

    public virtual SourceType SourceType { get; set; } = null!;
}
