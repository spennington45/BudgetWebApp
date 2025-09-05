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
            return await _context.BudgetLineItems.FirstOrDefaultAsync(x => x.BugetLineItemId == id);
        }

        public async Task<Models.BudgetLineItem> UpdateBudgetLineItemAsync(Models.BudgetLineItem lineItem)
        {
            var updatedLineItem = _context.BudgetLineItems.Update(lineItem);
            await _context.SaveChangesAsync();
            return updatedLineItem.Entity;
        }
    }
}
