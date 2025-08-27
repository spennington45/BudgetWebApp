using budgetWebApp.Server.Interfaces;
using budgetWebApp.Server.Models;

namespace budgetWebApp.Server.Repositories
{
    public class RecurringExpenseRepository : IRecurringExpenseRepository
    {
        private readonly BudgetContext _context;

        public RecurringExpenseRepository(BudgetContext context)
        {
            _context = context;
        }

        public Task<RecurringExpense> AddRecurringExpenseAsync(RecurringExpense recurringExpense)
        {
            throw new NotImplementedException();
        }

        public Task DeleteRecurringExpenseAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<RecurringExpense>> GetRecurringExpensesByUserIdAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<RecurringExpense> UpdateRecurringExpense(RecurringExpense recurringExpense)
        {
            throw new NotImplementedException();
        }
    }
}
