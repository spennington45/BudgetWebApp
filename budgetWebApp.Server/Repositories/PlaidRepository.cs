using AutoMapper;
using budgetWebApp.Server.Interfaces;
using budgetWebApp.Server.Models;
using budgetWebApp.Server.Models.DTOs;
using budgetWebApp.Server.Services;
using Going.Plaid;
using Going.Plaid.Accounts;
using Going.Plaid.Entity;
using Going.Plaid.Transactions;
using Microsoft.EntityFrameworkCore;

namespace budgetWebApp.Server.Repositories
{
    public class PlaidRepository : IPlaidRepository
    {
        private readonly BudgetContext _context;
        private readonly IMapper _mapper;
        private readonly PlaidAuthService _plaidAuth;
        private readonly IBudgetRepository _budgetRepository;
        private readonly PlaidClient _plaid;
        private readonly ILogger<PlaidRepository> _logger;

        public PlaidRepository(BudgetContext context, IMapper mapper, PlaidAuthService plaidAuthService, IBudgetRepository budgetRepository, PlaidClient plaidClient, ILogger<PlaidRepository> logger)
        {
            _context = context;
            _mapper = mapper;
            _plaidAuth = plaidAuthService;
            _budgetRepository = budgetRepository;
            _plaid = plaidClient;
            _logger = logger;
        }

        public async Task<PlaidAccount> AddPlaidAccountAsync(PlaidAccount accounts)
        {
            var newAccount = await _context.PlaidAccounts.AddAsync(accounts);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Added Plaid Account {accounts.AccountId}");
            return newAccount.Entity;
        }

        public async Task<List<PlaidAccount>> AddPlaidAccountsAsync(List<PlaidAccount> accounts)
        {
            await _context.PlaidAccounts.AddRangeAsync(accounts);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Added Plaid Accounts: {accounts.Count()}");
            return accounts;
        }

        public async Task<PlaidItem> AddPlaidItemAndAccountsTransactionAsync(PlaidLinkRequestDto link)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var exchange = await _plaidAuth.GetAccessTokenAsync(link.PublicToken);

                var item = _mapper.Map<PlaidItem>(link);

                item.ItemId = exchange.ItemId;
                item.AccessToken = exchange.AccessToken;

                var newItem = await _context.PlaidItems.AddAsync(item);
                await _context.SaveChangesAsync();

                var accountsResponse = await _plaid.AccountsGetAsync(
                    new AccountsGetRequest
                    {
                        AccessToken = exchange.AccessToken
                    });

                var accounts = _mapper.Map<List<PlaidAccount>>(link.Accounts);

                foreach (var acc in accounts)
                {
                    acc.PlaidItemId = newItem.Entity.PlaidItemId;

                    var plaidAccount = accountsResponse.Accounts
                        .FirstOrDefault(a => a.AccountId == acc.AccountId);

                    if (plaidAccount != null)
                    {
                        acc.CurrentBalance =
                            plaidAccount.Balances?.Current != null
                                ? (decimal?)plaidAccount.Balances.Current
                                : null;

                        acc.AvailableBalance =
                            plaidAccount.Balances?.Available != null
                                ? (decimal?)plaidAccount.Balances.Available
                                : null;
                    }
                }

                await _context.PlaidAccounts.AddRangeAsync(accounts);
                await _context.SaveChangesAsync();

                newItem.Entity.PlaidAccounts = accounts;

                await transaction.CommitAsync();

                return newItem.Entity;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<PlaidItem> AddPlaidItemAsync(PlaidItem item)
        {
            var newItem = await _context.PlaidItems.AddAsync(item);
            await _context.SaveChangesAsync();
            return newItem.Entity; ;
        }

        public async Task SyncTransactionsForItemAsync(long plaidItemId)
        {
            _logger.LogInformation($"Syncing Transactions for item {plaidItemId}");

            var item = await _context.PlaidItems
                .Include(i => i.PlaidAccounts)
                .FirstAsync(i => i.PlaidItemId == plaidItemId);

            await UpdateAccountBalancesAsync(item);

            var cursorRow = await _context.PlaidSyncCursors
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
                _logger.LogInformation($"Transactions Added {response.Added.Count()}");
                _logger.LogInformation($"Transactions Modified {response.Modified.Count()}");
                _logger.LogInformation($"Transactions Removed {response.Removed.Count()}");

                foreach (var added in response.Added)
                    await UpsertBudgetLineItemAsync(added, item);

                foreach (var modified in response.Modified)
                    await UpsertBudgetLineItemAsync(modified, item);

                foreach (var removed in response.Removed)
                    await RemoveBudgetLineItemAsync(removed.TransactionId);

                request.Cursor = response.NextCursor;
                hasMore = response.HasMore;
            }

            if (cursorRow == null)
            {
                _context.PlaidSyncCursors.Add(new PlaidSyncCursor
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

        private async Task UpsertBudgetLineItemAsync(Transaction txn, PlaidItem item)
        {
            var existing = await _context.BudgetLineItems
                .FirstOrDefaultAsync(t => t.TransactionId == txn.TransactionId);

            var account = item.PlaidAccounts
                .First(a => a.AccountId == txn.AccountId);

            var plaidSourceType = await _context.SourceTypes.FirstOrDefaultAsync(st => st.SourceName == "Plaid");
            long plaidSourceTypeId;
            if (plaidSourceType == null)
            {
                var newSourceType = new SourceType
                {
                    SourceName = "Plaid"
                };
                _context.SourceTypes.Add(newSourceType);
                await _context.SaveChangesAsync();
                plaidSourceTypeId = newSourceType.SourceTypeId;
            }
            else
            {
                plaidSourceTypeId = plaidSourceType.SourceTypeId;
            }

            var budgetId = await GetOrCreateBudgetForMonth(item.UserId, (DateOnly)txn.Date);

            if (existing == null)
            {
                var isTransfer = await IsTransferAsync(txn, item.UserId);

                var newItem = new BudgetLineItem
                {
                    TransactionId = txn.TransactionId,
                    PendingTransactionId = txn.PendingTransactionId,
                    Date = (DateOnly)txn.Date,
                    Value = NormalizePlaidAmount((decimal)txn.Amount),
                    Name = txn.Name,
                    MerchantName = txn.MerchantName,
                    Pending = (bool)txn.Pending,
                    CategoryId = MapPlaidCategory((IList<string>?)txn.Category),
                    PlaidAccountId = account.PlaidAccountId,
                    UserId = item.UserId,
                    SourceTypeId = plaidSourceTypeId,
                    BudgetId = budgetId,
                    //IsTransfer = isTransfer,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _context.BudgetLineItems.Add(newItem);
            }
            else
            {
                existing.Value = NormalizePlaidAmount((decimal)txn.Amount);
                existing.Pending = (bool)txn.Pending;
                existing.Date = (DateOnly)txn.Date;
                existing.MerchantName = txn.MerchantName;
                existing.Name = txn.Name;
                existing.CategoryId = MapPlaidCategory((IList<string>?)txn.Category);
                existing.UpdatedAt = DateTime.UtcNow;
                existing.BudgetId = budgetId;
            }

            await _context.SaveChangesAsync();
        }

        private async Task<bool> IsTransferAsync(Transaction txn, long userId)
        {
            var accountIds = await _context.PlaidAccounts
                .Where(a => a.PlaidItem.UserId == userId)
                .Select(a => a.AccountId)
                .ToListAsync();

            var text = $"{txn.Name} {txn.MerchantName}"
                .ToLowerInvariant();

            bool containsTransferKeywords =
                text.Contains("transfer") ||
                text.Contains("xfer") ||
                text.Contains("payment");

            return accountIds.Contains(txn.AccountId) &&
                   containsTransferKeywords;
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

        private async Task<long> GetOrCreateBudgetForMonth(long userId, DateOnly date)
        {
            var year = date.Year;
            var month = date.Month;

            var budget = await _context.Budgets
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
            };

            _context.Budgets.Add(newBudget);
            await _context.SaveChangesAsync();

            return newBudget.BudgetId;
        }

        private long MapPlaidCategory(IList<string>? plaidCategory)
        {
            const string defaultCategoryName = "Uncategorized";

            var defaultCategory = _context.Categories
                .FirstOrDefault(c => c.CategoryName == defaultCategoryName);

            if (defaultCategory == null)
            {
                defaultCategory = new Models.Category { CategoryName = defaultCategoryName };
                _context.Categories.Add(defaultCategory);
                _context.SaveChanges();
            }

            if (plaidCategory == null || plaidCategory.Count == 0)
                return defaultCategory.CategoryId;

            var name = plaidCategory.Last();

            var category = _context.Categories
                .FirstOrDefault(c => c.CategoryName == name);

            if (category != null)
                return category.CategoryId;

            var newCat = new Models.Category { CategoryName = name };
            _context.Categories.Add(newCat);
            _context.SaveChanges();

            return newCat.CategoryId;
        }

        public async Task<ICollection<PlaidItem>> GetPladItemsByUserId(long userId)
        {
            return await _context.PlaidItems
                .Where(c => c.UserId == userId)
                .ToListAsync();
        }

        public async Task<PlaidItem> GetPlaidItemByItemId(string itemId)
        {
            return await _context.PlaidItems
                .FirstOrDefaultAsync(i => i.ItemId == itemId);
        }

        private decimal NormalizePlaidAmount(decimal amount)
        {
            if (amount > 0)
                return -amount;

            return Math.Abs(amount);
        }

        public async Task<ICollection<PlaidAccount>> GetPlaidAccountsByUserId(long userId)
        {
            return await _context.PlaidAccounts
                .Include(a => a.PlaidItem)
                .Where(a => a.PlaidItem.UserId == userId)
                .OrderBy(a => a.Type)
                .ThenBy(a => a.Name)
                .ToListAsync();
        }

        private async Task UpdateAccountBalancesAsync(PlaidItem item)
        {
            var request = new AccountsGetRequest
            {
                AccessToken = item.AccessToken
            };

            var response = await _plaid.AccountsGetAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning(
                    "Failed to update balances for Plaid Item {PlaidItemId}",
                    item.PlaidItemId);
                return;
            }

            foreach (var plaidAccount in response.Accounts)
            {
                var account = await _context.PlaidAccounts
                    .FirstOrDefaultAsync(a =>
                        a.PlaidItemId == item.PlaidItemId &&
                        a.AccountId == plaidAccount.AccountId);

                if (account == null)
                    continue;

                account.CurrentBalance =
                    plaidAccount.Balances?.Current != null
                        ? (decimal?)plaidAccount.Balances.Current
                        : null;

                account.AvailableBalance =
                    plaidAccount.Balances?.Available != null
                        ? (decimal?)plaidAccount.Balances.Available
                        : null;
            }

            await _context.SaveChangesAsync();
        }
    }
}
