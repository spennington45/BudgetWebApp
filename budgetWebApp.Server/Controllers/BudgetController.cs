using budgetWebApp.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace budgetWebApp.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BudgetController
    {
        private readonly ILogger<BudgetController> _logger;

        public BudgetController(ILogger<BudgetController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Budget> Get()
        {
            return GetTestData();
        }

        [HttpGet("GetBudgetByUserId/{id}")]
        [ProducesResponseType<Budget>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<Budget>> GetBudgetByUserId(long id)
        {
            return GetTestData().Where(x => x.UserId == id).ToList();
        }

        [HttpGet("GetBudgetByBudgetId/{id}")]
        [ProducesResponseType<Budget>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<Budget> GetBudgetByBudgetId(long id)
        {
            return GetTestData().FirstOrDefault(x => x.BudgetId == id);
        }

        private IEnumerable<Budget> GetTestData()
        {
            return new List<Budget>
            {
                new Budget
                {
                    BudgetId = 1,
                    Date = DateTime.Now,
                    UserId = 1,
                    User = new User
                    {
                        UserId = 1,
                        FirstName = "Test",
                        LastName = "Test",
                    },
                    BudgetLineItems = new List<BudgetLineItem> 
                    {
                        new BudgetLineItem
                        {
                            BudgetId = 1,
                            BugetLineItemId = 1,
                            CatigoryId = 1,
                            Catigory = new Category
                            {
                                CategoryId = 1,
                                CategoryName = "Test",
                            }
                        }
                    }
                },
                new Budget
                {
                    BudgetId = 2,
                    Date = DateTime.Now.AddMonths(-1),
                    UserId = 1,
                    User = new User
                    {
                        UserId = 1,
                        FirstName = "Test",
                        LastName = "Test",
                    },
                    BudgetLineItems = new List<BudgetLineItem>
                    {
                        new BudgetLineItem
                        {
                            BudgetId = 2,
                            BugetLineItemId = 2,
                            CatigoryId = 1,
                            Catigory = new Category
                            {
                                CategoryId = 2,
                                CategoryName = "Test2",
                            }
                        }
                    }
                }
            };
        }
    }
}
