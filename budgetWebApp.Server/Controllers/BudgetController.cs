using budgetWebApp.Server.Helpers;
using budgetWebApp.Server.Interfaces;
using budgetWebApp.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace budgetWebApp.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BudgetController
    {
        private readonly ILogger<BudgetController> _logger;
        private readonly IBudgetRepository _budgetRepository;

        public BudgetController(ILogger<BudgetController> logger, IBudgetRepository budgetRepository)
        {
            _logger = logger;
            _budgetRepository = budgetRepository;
        }

        [HttpGet]
        public IEnumerable<Budget> Get()
        {
            return TestDataHelper.GetTestData();
        }

        [HttpGet("GetBudgetByUserId/{id}")]
        [ProducesResponseType<Budget>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<Budget>> GetBudgetByUserId(long id)
        {
            _logger.LogInformation($"Request budget for user id {id}");
            return TestDataHelper.GetTestData().Where(x => x.UserId == id).ToList();
        }

        [HttpGet("GetBudgetLineItemsByBudgetId/{id}")]
        [ProducesResponseType<Budget>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<BudgetLineItem>> GetBudgetLineItemsByBudgetId(long id)
        {
            _logger.LogInformation($"Request budget for budget id {id}");
            return TestDataHelper.GetTestData().FirstOrDefault(x => x.BudgetId == id)!.BudgetLineItems.ToList();
        }

        [HttpGet("Test")]
        public IEnumerable<Budget> Test()
        {
            return _budgetRepository.GetBudgets();
        }

        [HttpGet("GetSourceTypes")]
        [ProducesResponseType<SourceType>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<SourceType>> GetSourceTypes()
        {
            _logger.LogInformation("Request for source types");
            return TestDataHelper.GetTestData()
                .SelectMany(b => b.BudgetLineItems)
                .Select(li => li.SourceType)
                .GroupBy(st => st.SourceTypeId)
                .Select(g => g.First())
                .ToList();
        }

        [HttpPost("CreateBudgetLineItem")]
        [ProducesResponseType(typeof(BudgetLineItem), StatusCodes.Status200OK)]
        public ActionResult<BudgetLineItem> CreateBudgetLineItem([FromBody] BudgetLineItem newItem)
        {
            _logger.LogInformation("Creating new line item");
            // Assume BudgetId comes from the client, or is inferred server-side
            return newItem;
        }


        [HttpPut("UpdateBudgetLineItem/{id}")]
        [ProducesResponseType(typeof(BudgetLineItem), StatusCodes.Status200OK)]
        public ActionResult<BudgetLineItem> UpdateBudgetLineItem(long id, [FromBody] BudgetLineItem updatedItem)
        {
            _logger.LogInformation($"Updating line item {id}");
            // Simulate update
            return updatedItem;
        }

        [HttpDelete("DeleteBudgetLineItem/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult DeleteBudgetLineItem(long id)
        {
            _logger.LogInformation($"Deleting line item {id}");
            // Simulate delete
            return Ok();
        }

    }
}
