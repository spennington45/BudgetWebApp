using budgetWebApp.Server.Interfaces;
using budgetWebApp.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace budgetWebApp.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class BudgetController : ControllerBase
    {
        private readonly ILogger<BudgetController> _logger;
        private readonly IBudgetRepository _budgetRepository;
        private readonly IUserRepository _userRepository;

        public BudgetController(ILogger<BudgetController> logger, IBudgetRepository repository, IUserRepository userRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _budgetRepository = repository ?? throw new ArgumentNullException(nameof(repository));
            _userRepository = userRepository;
        }

        [HttpGet("GetBudgetByUserId/{id}")]
        [ProducesResponseType(typeof(IEnumerable<Budget>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Budget>>> GetBudgetByUserId(long id)
        {
            _logger.LogInformation($"Requesting budgets for user ID {id}");

            var budgets = await _budgetRepository.GetBudgetsByUserIdAsync(id);
            if (budgets == null || !budgets.Any())
            {
                _logger.LogWarning($"No budgets found for user ID {id}");
                return NotFound();
            }

            return Ok(budgets);
        }

        [HttpGet("GetBudgetByBudgetId/{id}")]
        [ProducesResponseType(typeof(IEnumerable<Budget>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Budget>> GetBudgetByBudgetId(long id)
        {
            _logger.LogInformation($"Requesting budgets for user ID {id}");

            var budgets = await _budgetRepository.GetBudgetByBudgetIdAsync(id);
            if (budgets == null || budgets.BudgetId <= 0)
            {
                _logger.LogWarning($"No budgets found for user ID {id}");
                return NotFound();
            }

            return Ok(budgets);
        }

        [HttpPut("UpdateBudget")]
        [ProducesResponseType(typeof(Budget), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Budget>> UpdateBudget([FromBody] Budget budget)
        {
            if (budget == null || budget.BudgetId <= 0)
            {
                _logger.LogWarning("Invalid budget update request.");
                return BadRequest("Invalid budget data.");
            }

            var updatedBudget = await _budgetRepository.UpdateBudgetAsync(budget);
            if (updatedBudget == null)
            {
                _logger.LogWarning($"Budget with ID {budget.BudgetId} not found.");
                return NotFound();
            }

            _logger.LogInformation($"Budget {budget.BudgetId} updated successfully.");
            return Ok(updatedBudget);
        }

        [HttpPost("AddBudget")]
        [ProducesResponseType(typeof(Budget), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Budget>> AddBudget([FromBody] Budget budget)
        {
            if (budget == null || budget.UserId <= 0)
            {
                _logger.LogWarning("Invalid budget creation request.");
                return BadRequest("Invalid budget data.");
            }

            budget.User = await _userRepository.GetUserByUserIdAsync(budget.UserId);

            var createdBudget = await _budgetRepository.AddBudgetAsync(budget);
            if (createdBudget == null)
            {
                _logger.LogError("Failed to create budget.");
                return BadRequest("Could not create budget.");
            }

            _logger.LogInformation($"Budget created successfully with ID {createdBudget.BudgetId}.");
            return Ok(createdBudget);
        }

        [HttpDelete("DeleteBudget/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteBudget(long id)
        {
            var success = await _budgetRepository.DeleteBudgetAsync(id);
            if (!success)
            {
                _logger.LogWarning($"Budget with ID {id} not found or could not be deleted.");
                return NotFound();
            }

            _logger.LogInformation($"Budget with ID {id} deleted successfully.");
            return Ok();
        }
    }
}
