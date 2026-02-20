using System.Text.Json.Serialization;

namespace budgetWebApp.Server.Models;

public partial class User
{
    public long UserId { get; set; }

    public string ExternalId { get; set; } = null!;

    public string? Name { get; set; }

    public string? Picture { get; set; }

    public string Email { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    [JsonIgnore]
    public virtual ICollection<BudgetTotal> BudgetTotals { get; set; } = new List<BudgetTotal>();

    [JsonIgnore]
    public virtual ICollection<Budget> Budgets { get; set; } = new List<Budget>();

    [JsonIgnore]
    public virtual ICollection<RecurringExpense> RecurringExpenses { get; set; } = new List<RecurringExpense>();
}
