using budgetWebApp.Server.Models;

namespace budgetWebApp.Server.Interfaces
{
    public interface IBudgetRepository
    {
        Task<IEnumerable<Budget>> GetBudgetsAsync();

        Task<Budget> GetBudgetByBudgetIdAsync(long id);

        Task<Budget> AddBudgetAsync(Budget budget);

        Task<Budget> UpdateBudget(Budget budget);

        Task DeleteBudgetAsync(long id);
    }
}
