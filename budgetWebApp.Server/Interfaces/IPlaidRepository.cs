using budgetWebApp.Server.Models;

namespace budgetWebApp.Server.Interfaces
{
    public interface IPlaidRepository
    {
        Task<PlaidItem> AddPlaidItemAsync(PlaidItem account);

        Task<PlaidItem> AddPlaidAccountsAsync(List<PlaidAccount> accounts);

        Task<PlaidAccount> AddPlaidAccountAsync(PlaidAccount item);
    }
}
