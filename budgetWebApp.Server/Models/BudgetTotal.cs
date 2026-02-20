using System.Text.Json.Serialization;

namespace budgetWebApp.Server.Models;

public partial class BudgetTotal
{
    public long BudgetTotalId { get; set; }

    public long UserId { get; set; }

    public decimal TotalValue { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public virtual User? User { get; set; } = null!;
}
