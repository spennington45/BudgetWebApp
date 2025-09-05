using budgetWebApp.Server.Models;

namespace budgetWebApp.Server.Interfaces
{
    public interface IBudgetRepository
    {
        Task<IEnumerable<Budget>> GetBudgetsByUserIdAsync(long id);

        Task<Budget> GetBudgetByBudgetIdAsync(long id);

        Task<Budget> AddBudgetAsync(Budget budget);

        Task<Budget> UpdateBudgetAsync(Budget budget);

        Task<bool> DeleteBudgetAsync(long id);
    }
}
