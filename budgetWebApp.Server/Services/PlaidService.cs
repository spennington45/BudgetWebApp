using budgetWebApp.Server.Models;
using Going.Plaid;

namespace budgetWebApp.Server.Services
{
    public class PlaidService
    {
        private readonly HttpClient _httpClient;
        private readonly BudgetContext _context;
        private readonly PlaidClient _plaid;

        public PlaidService(HttpClient httpClient, BudgetContext budgetContext, PlaidClient plaidClient)
        {
            _httpClient = httpClient;
            _context = budgetContext;
            _plaid = plaidClient;
        }
    }
}
