using budgetWebApp.Server.Helpers;
using System.Text.Json;
using System.Text;

namespace budgetWebApp.Server.Services
{
    public class PlaidService
    {
        private readonly HttpClient _httpClient;

        public PlaidService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
    }
}
