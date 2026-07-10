using budgetWebApp.Server.Models;
using budgetWebApp.Server.Models.DTOs;

namespace budgetWebApp.Server.Interfaces
{
    public interface IPlaidRepository
    {
        Task<PlaidItem> AddPlaidItemAsync(PlaidItem account);

        Task<PlaidItem> AddPlaidItemAndAccountsTransactionAsync(PlaidLinkRequestDto account);

        Task<List<PlaidAccount>> AddPlaidAccountsAsync(List<PlaidAccount> link);

        Task<PlaidAccount> AddPlaidAccountAsync(PlaidAccount item);

        Task SyncTransactionsForItemAsync(long plaidItemId);

        Task<ICollection<PlaidItem>> GetPladItemsByUserId(long userId);

        Task<PlaidItem> GetPlaidItemByItemId(string itemId);

        Task<ICollection<PlaidAccount>> GetPlaidAccountsByUserId(long userId);
    }
}
