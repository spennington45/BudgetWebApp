namespace budgetWebApp.Server.Models.DTOs
{
    public class BudgetLineItemDto
    {
        public long BudgetLineItemId { get; set; }

        public string? Label { get; set; }

        public long BudgetId { get; set; }

        public long CategoryId { get; set; }

        public decimal Value { get; set; }

        public long? SourceTypeId { get; set; }
    }
}
