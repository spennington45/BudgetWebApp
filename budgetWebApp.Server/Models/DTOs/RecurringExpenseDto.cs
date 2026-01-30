namespace budgetWebApp.Server.Models.DTOs
{
    public class RecurringExpenseDto
    {
        public long RecurringExpenseId { get; set; }

        public string? Label { get; set; }

        public long CategoryId { get; set; }

        public long? SourceTypeId { get; set; }

        public decimal Value { get; set; }

        public long UserId { get; set; }
    }
}
