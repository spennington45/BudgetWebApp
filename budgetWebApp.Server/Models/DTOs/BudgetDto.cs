namespace budgetWebApp.Server.Models.DTOs
{
    public class BudgetDto
    {
        public long BudgetId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public long UserId { get; set; }
    }
}
