namespace budgetWebApp.Server.Models.DTOs
{
    public class BudgetLineItemDto
    {
        public long BudgetLineItemId { get; set; }

        public long BudgetId { get; set; }

        public string TransactionId { get; set; } = null!;

        public string? PendingTransactionId { get; set; }

        public DateOnly Date { get; set; }

        public decimal Value { get; set; }

        public string? Name { get; set; }

        public string? MerchantName { get; set; }

        public bool Pending { get; set; }

        public long CategoryId { get; set; }

        public long PlaidAccountId { get; set; }

        public long UserId { get; set; }

        public long SourceTypeId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
