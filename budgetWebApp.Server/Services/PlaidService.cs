using budgetWebApp.Server.Models;

namespace budgetWebApp.Server.Services
{
    public class PlaidService
    {
        private readonly HttpClient _httpClient;
        private readonly BudgetContext _context;

        public PlaidService(HttpClient httpClient, BudgetContext budgetContext)
        {
            _httpClient = httpClient;
            _context = budgetContext;
        }

        // TODO This is a WIP Probably will move much of this to repos 
        /*public async Task SyncTransactionsForItemAsync(long plaidItemId)
        {
            var item = await _context.PlaidItems
                .Include(i => i.PlaidAccounts)
                .FirstAsync(i => i.PlaidItemId == plaidItemId);

            var cursorRow = await _context.PlaidSyncCursor
                .FirstOrDefaultAsync(c => c.PlaidItemId == plaidItemId);

            var cursor = cursorRow?.Cursor;

            var request = new TransactionsSyncRequest
            {
                AccessToken = item.AccessToken,
                Cursor = cursor
            };

            bool hasMore = true;

            while (hasMore)
            {
                var response = await _plaid.TransactionsSyncAsync(request);

                // Handle ADDED transactions
                foreach (var added in response.Added)
                    await UpsertBudgetLineItemAsync(added, item);

                // Handle MODIFIED transactions
                foreach (var modified in response.Modified)
                    await UpsertBudgetLineItemAsync(modified, item);

                // Handle REMOVED transactions
                foreach (var removed in response.Removed)
                    await RemoveBudgetLineItemAsync(removed.TransactionId);

                // Update cursor
                request.Cursor = response.NextCursor;
                hasMore = response.HasMore;
            }

            // Save cursor
            if (cursorRow == null)
            {
                _context.PlaidSyncCursor.Add(new PlaidSyncCursor
                {
                    PlaidItemId = plaidItemId,
                    Cursor = request.Cursor,
                    UpdatedAt = DateTime.UtcNow
                });
            }
            else
            {
                cursorRow.Cursor = request.Cursor;
                cursorRow.UpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
        }

        private async Task UpsertBudgetLineItemAsync(PlaidTransaction txn, PlaidItem item)
        {
            var existing = await _context.BudgetLineItems
                .FirstOrDefaultAsync(t => t.TransactionId == txn.TransactionId);

            // Find the PlaidAccount row for this transaction
            var account = item.PlaidAccounts
                .First(a => a.AccountId == txn.AccountId);

            // Determine the month’s BudgetId
            var budgetId = await _budgetService
                .GetOrCreateBudgetForMonth(item.UserId, txn.Date);

            if (existing == null)
            {
                var newItem = new BudgetLineItem
                {
                    TransactionId = txn.TransactionId,
                    PendingTransactionId = txn.PendingTransactionId,
                    Date = txn.Date,
                    Value = txn.Amount,
                    Name = txn.Name,
                    MerchantName = txn.MerchantName,
                    Pending = txn.Pending,
                    CategoryId = MapPlaidCategory(txn.Category),
                    PlaidAccountId = account.PlaidAccountId,
                    UserId = item.UserId,
                    SourceTypeId = 1, // 1 = Plaid
                    BudgetId = budgetId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.BudgetLineItems.Add(newItem);
            }
            else
            {
                existing.Value = txn.Amount;
                existing.Pending = txn.Pending;
                existing.Date = txn.Date;
                existing.MerchantName = txn.MerchantName;
                existing.Name = txn.Name;
                existing.CategoryId = MapPlaidCategory(txn.Category);
                existing.UpdatedAt = DateTime.UtcNow;
                existing.BudgetId = budgetId;
            }

            await _context.SaveChangesAsync();
        }

        private async Task RemoveBudgetLineItemAsync(string transactionId)
        {
            var existing = await _context.BudgetLineItems
                .FirstOrDefaultAsync(t => t.TransactionId == transactionId);

            if (existing != null)
            {
                _context.BudgetLineItems.Remove(existing);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<long> GetOrCreateBudgetForMonth(long userId, DateTime date)
        {
            var year = date.Year;
            var month = date.Month;

            var budget = await _context.Budget
                .FirstOrDefaultAsync(b => b.UserId == userId &&
                                          b.Year == year &&
                                          b.Month == month);

            if (budget != null)
                return budget.BudgetId;

            var newBudget = new Budget
            {
                UserId = userId,
                Year = year,
                Month = month,
                CreatedAt = DateTime.UtcNow
            };

            _context.Budget.Add(newBudget);
            await _context.SaveChangesAsync();

            return newBudget.BudgetId;
        }

        private long MapPlaidCategory(IList<string>? plaidCategory)
        {
            if (plaidCategory == null || plaidCategory.Count == 0)
                return DefaultCategoryId; // e.g. "Uncategorized"

            var name = plaidCategory.Last(); // Most specific category

            var category = _context.Category
                .FirstOrDefault(c => c.CategoryName == name);

            if (category != null)
                return category.CategoryId;

            // Auto-create new categories if desired
            var newCat = new Category { CategoryName = name };
            _context.Category.Add(newCat);
            _context.SaveChanges();

            return newCat.CategoryId;
        }*/
    }
}
