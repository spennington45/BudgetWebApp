using AutoMapper;
using budgetWebApp.Server.Interfaces;
using budgetWebApp.Server.Models;
using budgetWebApp.Server.Models.DTOs;
using budgetWebApp.Server.Services;

namespace budgetWebApp.Server.Repositories
{
    public class PlaidRepository : IPlaidRepository
    {
        private readonly BudgetContext _context;
        private readonly IMapper _mapper;
        private readonly PlaidAuthService _plaidAuth;

        public PlaidRepository(BudgetContext context, IMapper mapper, PlaidAuthService plaidAuthService)
        {
            _context = context;
            _mapper = mapper;
            _plaidAuth = plaidAuthService;
        }

        public async Task<PlaidAccount> AddPlaidAccountAsync(PlaidAccount accounts)
        {
            var newAccount = await _context.PlaidAccounts.AddAsync(accounts);
            await _context.SaveChangesAsync();
            return newAccount.Entity;
        }

        public async Task<List<PlaidAccount>> AddPlaidAccountsAsync(List<PlaidAccount> accounts)
        {
            await _context.PlaidAccounts.AddRangeAsync(accounts);
            await _context.SaveChangesAsync();
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

                var accounts = _mapper.Map<List<PlaidAccount>>(link.Accounts);

                foreach (var acc in accounts)
                    acc.PlaidItemId = newItem.Entity.PlaidItemId;

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

    }
}
