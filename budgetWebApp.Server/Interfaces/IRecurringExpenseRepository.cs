using budgetWebApp.Server.Models;

namespace budgetWebApp.Server.Interfaces
{
    public interface IRecurringExpenseRepository
    {
        Task<IEnumerable<RecurringExpense>> GetRecurringExpensesByUserIdAsync(long id);

        Task<RecurringExpense> AddRecurringExpenseAsync(RecurringExpense recurringExpense);

        Task<RecurringExpense> UpdateRecurringExpense(RecurringExpense recurringExpense);

        Task DeleteRecurringExpenseAsync(long id);
    }
}
