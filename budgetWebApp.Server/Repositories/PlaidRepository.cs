using budgetWebApp.Server.Interfaces;
using budgetWebApp.Server.Models;

namespace budgetWebApp.Server.Repositories
{
    public class PlaidRepository : IPlaidRepository
    {
        private readonly BudgetContext _context;

        public PlaidRepository(BudgetContext context)
        {
            _context = context;
        }
        public async Task<PlaidAccount> AddPlaidAccountAsync(PlaidAccount accounts)
        {
            var newAccount = await _context.PlaidAccounts.AddAsync(accounts);
            await _context.SaveChangesAsync();
            return newAccount.Entity;
        }

        public async Task<PlaidItem> AddPlaidAccountsAsync(List<PlaidAccount> accounts)
        {
            var newAccounts = await _context.PlaidAccounts.AddRangeAsync(accounts);
            await _context.SaveChangesAsync();
            return newAccounts.Entity;
        }

        public async Task<PlaidItem> AddPlaidItemAsync(PlaidItem item)
        {
            var newItem = await _context.PlaidItems.AddAsync(item);
            await _context.SaveChangesAsync();
            return newItem.Entity; ;
        }
    }
}
