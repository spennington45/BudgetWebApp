using budgetWebApp.Server.Models;

namespace budgetWebApp.Server.Interfaces
{
    public interface IBudgetRepository
    {
        IEnumerable<Budget> GetBudgets();

        Budget GetBudgetByBudgetId(long id);
    }
}
