using budgetWebApp.Server.Interfaces;
using budgetWebApp.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace budgetWebApp.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BudgetLineItemController : ControllerBase
    {
        private readonly ILogger<BudgetLineItemController> _logger;
        private readonly IBudgetLineItemRepository _lineItemRepository;

        public BudgetLineItemController(ILogger<BudgetLineItemController> logger, IBudgetLineItemRepository repository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _lineItemRepository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet("GetBudgetLineItemsByBudgetId/{id}")]
        [ProducesResponseType(typeof(BudgetLineItem), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<BudgetLineItem>>> GetBudgetLineItemsByBudgetId(long id)
        {
            _logger.LogInformation($"Fetching budget line item with for budget ID {id}");

            var lineItems = await _lineItemRepository.GetBudgetLineItemsByBudgetIdAsync(id);
            if (lineItems == null)
            {
                _logger.LogWarning($"Budget line items for budget ID {id} not found.");
                return NotFound();
            }

            return Ok(lineItems);
        }

        [HttpGet("GetBudgetLineItemById/{id}")]
        [ProducesResponseType(typeof(BudgetLineItem), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BudgetLineItem>> GetBudgetLineItemById(long id)
        {
            _logger.LogInformation($"Fetching budget line item with ID {id}");

            var lineItem = await _lineItemRepository.GetBudgetLineItemByLineItemIdAsync(id);
            if (lineItem == null)
            {
                _logger.LogWarning($"Budget line item with ID {id} not found.");
                return NotFound();
            }

            return Ok(lineItem);
        }

        [HttpPost("AddBudgetLineItem")]
        [ProducesResponseType(typeof(BudgetLineItem), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BudgetLineItem>> AddBudgetLineItem([FromBody] BudgetLineItem lineItem)
        {
            if (lineItem == null || lineItem.BudgetId <= 0)
            {
                _logger.LogWarning("Invalid budget line item creation request.");
                return BadRequest("Invalid line item data.");
            }

            var createdItem = await _lineItemRepository.AddBudgetLineItemAsync(lineItem);
            if (createdItem == null)
            {
                _logger.LogError("Failed to create budget line item.");
                return BadRequest("Could not create line item.");
            }

            _logger.LogInformation($"Budget line item created with ID {createdItem.BugetLineItemId}.");
            return Ok(createdItem);
        }

        [HttpPut("UpdateBudgetLineItem")]
        [ProducesResponseType(typeof(BudgetLineItem), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BudgetLineItem>> UpdateBudgetLineItem([FromBody] BudgetLineItem lineItem)
        {
            if (lineItem == null || lineItem.BugetLineItemId <= 0)
            {
                _logger.LogWarning("Invalid budget line item update request.");
                return BadRequest("Invalid line item data.");
            }

            var updatedItem = await _lineItemRepository.UpdateBudgetLineItemAsync(lineItem);
            if (updatedItem == null)
            {
                _logger.LogWarning($"Budget line item with ID {lineItem.BugetLineItemId} not found.");
                return NotFound();
            }

            _logger.LogInformation($"Budget line item {lineItem.BugetLineItemId} updated successfully.");
            return Ok(updatedItem);
        }

        [HttpDelete("DeleteBudgetLineItem/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteBudgetLineItem(long id)
        {
            var success = await _lineItemRepository.DeleteBudgetLineItemAsync(id);
            if (!success)
            {
                _logger.LogWarning($"Budget line item with ID {id} not found or could not be deleted.");
                return NotFound();
            }

            _logger.LogInformation($"Budget line item with ID {id} deleted successfully.");
            return Ok();
        }
    }
}