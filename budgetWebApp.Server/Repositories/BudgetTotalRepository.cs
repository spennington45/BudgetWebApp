using budgetWebApp.Server.Interfaces;
using budgetWebApp.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace budgetWebApp.Server.Repositories
{
    public class BudgetTotalRepository : IBudgetTotalRepository
    {
        private readonly BudgetContext _context;

        public BudgetTotalRepository(BudgetContext context)
        {
            _context = context;
        }

        public async Task<BudgetTotal> AddBudgetTotalAsync(BudgetTotal budgetTotal)
        {
            var newBudgetTotal = await _context.BudgetTotals.AddAsync(budgetTotal);
            await _context.SaveChangesAsync();
            return newBudgetTotal.Entity;
        }

        public Task<BudgetTotal> GetBudgetTotalByUserIdAsync(long id)
        {
            return _context.BudgetTotals.FirstOrDefaultAsync(b => b.UserId == id);
        }

        public async Task<BudgetTotal> UpdateBudgetTotalAsync(BudgetTotal budgetTotal)
        {
            var updatedBudgetTotal = _context.BudgetTotals.Update(budgetTotal);
            await _context.SaveChangesAsync();
            return updatedBudgetTotal.Entity;
        }
    }
}
