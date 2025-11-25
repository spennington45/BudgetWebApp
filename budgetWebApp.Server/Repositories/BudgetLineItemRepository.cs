using budgetWebApp.Server.Interfaces;
using budgetWebApp.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace budgetWebApp.Server.Repositories
{
    public class BudgetLineItemRepository : IBudgetLineItemRepository
    {
        private readonly BudgetContext _context;

        public BudgetLineItemRepository(BudgetContext context)
        {
            _context = context;
        }

        public async Task<Models.BudgetLineItem> AddBudgetLineItemAsync(Models.BudgetLineItem lineItem)
        {
            lineItem.Category = null;
            lineItem.SourceType = null;
            var newLineItem = await _context.BudgetLineItems.AddAsync(lineItem);
            await _context.SaveChangesAsync();
            return newLineItem.Entity;
        }

        public async Task<bool> DeleteBudgetLineItemAsync(long id)
        {
            var lineItem = await GetBudgetLineItemByLineItemIdAsync(id);
            if (lineItem != null)
            {
                _context.BudgetLineItems.Remove(lineItem);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<Models.BudgetLineItem> GetBudgetLineItemByLineItemIdAsync(long id)
        {
            return await _context.BudgetLineItems.FirstOrDefaultAsync(x => x.BudgetLineItemId == id);
        }

        public async Task<List<BudgetLineItem>> GetBudgetLineItemsByBudgetIdAsync(long id)
        {
            return await _context.BudgetLineItems.Where(x => x.BudgetId == id)
                .Include(x => x.Category)
                .Include(x => x.SourceType)
                .ToListAsync();
        }

        public async Task<Models.BudgetLineItem> UpdateBudgetLineItemAsync(Models.BudgetLineItem lineItem)
        {
            var updatedLineItem = _context.BudgetLineItems.Update(lineItem);
            await _context.SaveChangesAsync();
            return updatedLineItem.Entity;
        }
    }
}
