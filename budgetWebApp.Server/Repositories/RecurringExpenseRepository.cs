using budgetWebApp.Server.Interfaces;
using budgetWebApp.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace budgetWebApp.Server.Repositories
{
    public class RecurringExpenseRepository : IRecurringExpenseRepository
    {
        private readonly BudgetContext _context;

        public RecurringExpenseRepository(BudgetContext context)
        {
            _context = context;
        }

        public async Task<RecurringExpense> AddRecurringExpenseAsync(RecurringExpense recurringExpense)
        {
            var newRecurringExpense = await _context.RecurringExpenses.AddAsync(recurringExpense);
            await _context.SaveChangesAsync();
            return newRecurringExpense.Entity;
        }

        public async Task<bool> DeleteRecurringExpenseAsync(long id)
        {
            var expence = await GetRecurringExpensesByRecurringExpenseIdAsync(id);
            if (expence != null)
            {
                _context.Remove(expence);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<RecurringExpense>> GetRecurringExpensesByUserIdAsync(long id)
        {
            return await _context.RecurringExpenses.Where(x => x.UserId == id)
                .Include(x => x.Category)
                .Include(x => x.SourceType)
                .ToListAsync();
        }

        public async Task<RecurringExpense> UpdateRecurringExpense(RecurringExpense recurringExpense)
        {
            var updatedRecurringExpense = _context.RecurringExpenses.Update(recurringExpense);
            await _context.SaveChangesAsync();
            return updatedRecurringExpense.Entity;
        }

        public async Task<RecurringExpense> GetRecurringExpensesByRecurringExpenseIdAsync(long id)
        {
            return await _context.RecurringExpenses.FirstOrDefaultAsync(x => x.RecurringExpenseId == id);
        }
    }
}
