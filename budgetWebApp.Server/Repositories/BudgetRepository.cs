using budgetWebApp.Server.Interfaces;
using budgetWebApp.Server.Models;

namespace budgetWebApp.Server.Repositories
{
    public class BudgetRepository : IBudgetRepository
    {
        private readonly BudgetContext _context;

        public BudgetRepository()
        {
            _context = new BudgetContext();
        }

        public IEnumerable<Budget> GetBudgets()
        {
            return _context.Budgets;
        }

        public Budget GetBudgetByBudgetId(long id)
        {
            return _context.Budgets.FirstOrDefault(budget => budget.BudgetId == id);
        }
    }
}
