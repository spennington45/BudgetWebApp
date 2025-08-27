using budgetWebApp.Server.Models;

namespace budgetWebApp.Server.Interfaces
{
    public interface IBudgetTotalRepository
    {
        Task<BudgetTotal> GetBudgetTotalByUserIdAsync(long id);

        Task<BudgetTotal> UpdateBudgetTotalAsync(BudgetTotal budgetTotal);

        Task<BudgetTotal> AddBudgetTotalAsync(BudgetTotal budgetTotal);
    }
}
