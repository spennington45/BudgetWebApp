using budgetWebApp.Server.Interfaces;
using budgetWebApp.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace budgetWebApp.Server.Repositories
{
    public class BudgetRepository : IBudgetRepository
    {
        private readonly BudgetContext _context;

        public BudgetRepository(BudgetContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Budget>> GetBudgetsAsync()
        {
            return await _context.Budgets.ToListAsync();
        }

        public async Task<Budget> GetBudgetByBudgetIdAsync(long id)
        {
            return await _context.Budgets.FirstOrDefaultAsync(budget => budget.BudgetId == id);
        }

        public async Task<Budget> AddBudgetAsync(Budget budget)
        {
            var newBudget = await _context.Budgets.AddAsync(budget);
            await _context.SaveChangesAsync();
            return newBudget.Entity;
        }

        public async Task<Budget> UpdateBudget(Budget budget)
        {
            var updatedBudget = _context.Budgets.Update(budget);
            await _context.SaveChangesAsync();
            return updatedBudget.Entity;
        }

        public async Task DeleteBudgetAsync(long id)
        {
            var budget = await GetBudgetByBudgetIdAsync(id);
            if (budget != null)
            {
                _context.Budgets.Remove(budget); 
                await _context.SaveChangesAsync();
            }
        }
    }
}
