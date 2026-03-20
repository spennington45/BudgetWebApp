namespace budgetWebApp.Server.Models.DTOs
{
    public class BudgetDto
    {
        public long BudgetId { get; set; }
        public DateTime Date { get; set; }
        public long UserId { get; set; }
    }
}
