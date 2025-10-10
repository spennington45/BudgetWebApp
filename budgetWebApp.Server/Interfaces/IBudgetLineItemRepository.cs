using budgetWebApp.Server.Models;

namespace budgetWebApp.Server.Interfaces
{
    public interface IBudgetLineItemRepository
    {
        Task<BudgetLineItem> AddBudgetLineItemAsync(BudgetLineItem lineItem);

        Task<BudgetLineItem> UpdateBudgetLineItemAsync(BudgetLineItem lineItem);

        Task<bool> DeleteBudgetLineItemAsync(long id);

        Task<BudgetLineItem> GetBudgetLineItemByLineItemIdAsync(long id);

        Task<List<BudgetLineItem>> GetBudgetLineItemsByBudgetIdAsync(long id);
    }
}
