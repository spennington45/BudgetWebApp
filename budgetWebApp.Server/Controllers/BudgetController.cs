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
            _logger.LogInformation($"Request budget for user id {id}");
            return GetTestData().Where(x => x.UserId == id).ToList();
        }

        [HttpGet("GetBudgetLineItemsByBudgetId/{id}")]
        [ProducesResponseType<Budget>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<BudgetLineItem>> GetBudgetLineItemsByBudgetId(long id)
        {
            _logger.LogInformation($"Request budget for budget id {id}");
            return GetTestData().FirstOrDefault(x => x.BudgetId == id)!.BudgetLineItems.ToList();
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
                            CategoryId = 1,
                            Value = 100,
                            Category = new Category
                            {
                                CategoryId = 1,
                                CategoryName = "Test",
                            }
                        },
                        new BudgetLineItem
                        {
                            BudgetId = 1,
                            BugetLineItemId = 3,
                            CategoryId = 1,
                            Value = -150,
                            Category = new Category
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
                            CategoryId = 1,
                            Value = 100,
                            Category = new Category
                            {
                                CategoryId = 2,
                                CategoryName = "Test2",
                            }
                        },
                        new BudgetLineItem
                        {
                            BudgetId = 2,
                            BugetLineItemId = 4,
                            CategoryId = 1,
                            Value = 150,
                            Category = new Category
                            {
                                CategoryId = 2,
                                CategoryName = "Test2",
                            }
                        },
                        new BudgetLineItem
                        {
                            BudgetId = 2,
                            BugetLineItemId = 5,
                            CategoryId = 1,
                            Value = -75,
                            Category = new Category
                            {
                                CategoryId = 1,
                                CategoryName = "Test",
                            }
                        },
                    }
                },
                new Budget
                {
                    BudgetId = 3,
                    Date = DateTime.Now.AddYears(-1),
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
                            BudgetId = 3,
                            BugetLineItemId = 6,
                            CategoryId = 1,
                            Value = 100,
                            Category = new Category
                            {
                                CategoryId = 2,
                                CategoryName = "Test2",
                            }
                        },
                        new BudgetLineItem
                        {
                            BudgetId = 3,
                            BugetLineItemId = 7,
                            CategoryId = 1,
                            Value = 150,
                            Category = new Category
                            {
                                CategoryId = 1,
                                CategoryName = "Test",
                            }
                        },
                        new BudgetLineItem
                        {
                            BudgetId = 3,
                            BugetLineItemId = 8,
                            CategoryId = 1,
                            Value = -75,
                            Category = new Category
                            {
                                CategoryId = 2,
                                CategoryName = "Test2",
                            }
                        },
                    }
                }
            };
        }
    }
}
