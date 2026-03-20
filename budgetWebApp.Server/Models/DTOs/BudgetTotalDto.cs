namespace budgetWebApp.Server.Models.DTOs
{
    public class BudgetTotalDto
    {
        public long BudgetTotalId { get; set; }

        public long UserId { get; set; }

        public decimal TotalValue { get; set; }
    }
}
