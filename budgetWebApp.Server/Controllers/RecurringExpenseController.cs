using budgetWebApp.Server.Interfaces;
using budgetWebApp.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace budgetWebApp.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class RecurringExpenseController : AuthenticatedController
    {
        private readonly ILogger<RecurringExpenseController> _logger;
        private readonly IRecurringExpenseRepository _recurringExpenseRepository;

        public RecurringExpenseController(ILogger<RecurringExpenseController> logger, IRecurringExpenseRepository repository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _recurringExpenseRepository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet("GetRecurringExpensesByUserId/{id}")]
        [ProducesResponseType(typeof(IEnumerable<RecurringExpense>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<RecurringExpense>>> GetRecurringExpensesByUserId(long id)
        {
            _logger.LogInformation($"Fetching recurring expenses for user ID {id}");

            var expenses = await _recurringExpenseRepository.GetRecurringExpensesByUserIdAsync(id);
            if (expenses == null || !expenses.Any())
            {
                _logger.LogWarning($"No recurring expenses found for user ID {id}");
                return NotFound();
            }

            var ownershipResult = ValidateOwnership(expenses.FirstOrDefault().UserId);
            if (ownershipResult != null)
                return ownershipResult;

            return Ok(expenses);
        }

        [HttpGet("GetRecurringExpenseById/{id}")]
        [ProducesResponseType(typeof(RecurringExpense), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RecurringExpense>> GetRecurringExpenseById(long id)
        {
            _logger.LogInformation($"Fetching recurring expense with ID {id}");

            var expense = await _recurringExpenseRepository.GetRecurringExpensesByRecurringExpenseIdAsync(id);
            if (expense == null)
            {
                _logger.LogWarning($"Recurring expense with ID {id} not found.");
                return NotFound();
            }
            var ownershipResult = ValidateOwnership(expense.UserId);
            if (ownershipResult != null)
                return ownershipResult;

            return Ok(expense);
        }

        [HttpPost("AddRecurringExpense")]
        [ProducesResponseType(typeof(RecurringExpense), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RecurringExpense>> AddRecurringExpense([FromBody] RecurringExpense expense)
        {
            if (expense == null || expense.UserId <= 0)
            {
                _logger.LogWarning("Invalid recurring expense creation request.");
                return BadRequest("Invalid recurring expense data.");
            }

            var ownershipResult = ValidateOwnership(expense.UserId);
            if (ownershipResult != null)
                return ownershipResult;
            expense.User = null;
            expense.Category = null;
            expense.SourceType = null;

            var createdExpense = await _recurringExpenseRepository.AddRecurringExpenseAsync(expense);
            if (createdExpense == null)
            {
                _logger.LogError("Failed to create recurring expense.");
                return BadRequest("Could not create recurring expense.");
            }

            _logger.LogInformation($"Recurring expense created with ID {createdExpense.RecurringExpenseId}.");
            return Ok(createdExpense);
        }

        [HttpPut("UpdateRecurringExpense")]
        [ProducesResponseType(typeof(RecurringExpense), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<RecurringExpense>> UpdateRecurringExpense([FromBody] RecurringExpense expense)
        {
            if (expense == null || expense.RecurringExpenseId <= 0)
            {
                _logger.LogWarning("Invalid recurring expense update request.");
                return BadRequest("Invalid recurring expense data.");
            }

            var existingExpense = await _recurringExpenseRepository.GetRecurringExpensesByRecurringExpenseIdAsync(expense.RecurringExpenseId);
            var ownershipResult = ValidateOwnership(existingExpense.UserId);
            if (ownershipResult != null)
                return ownershipResult;

            existingExpense.Label = expense.Label;
            existingExpense.Value = expense.Value;
            existingExpense.CategoryId = expense.CategoryId;
            existingExpense.SourceTypeId = expense.SourceTypeId;

            var updatedExpense = await _recurringExpenseRepository.UpdateRecurringExpense(existingExpense);
            if (updatedExpense == null)
            {
                _logger.LogWarning($"Recurring expense with ID {expense.RecurringExpenseId} not found.");
                return NotFound();
            }

            _logger.LogInformation($"Recurring expense {expense.RecurringExpenseId} updated successfully.");
            return Ok(updatedExpense);
        }

        [HttpDelete("DeleteRecurringExpense/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteRecurringExpense(long id)
        {
            var existingExpense = await _recurringExpenseRepository.GetRecurringExpensesByRecurringExpenseIdAsync(id);
            var ownershipResult = ValidateOwnership(existingExpense.UserId);
            if (ownershipResult != null)
                return ownershipResult;

            var success = await _recurringExpenseRepository.DeleteRecurringExpenseAsync(id);
            if (!success)
            {
                _logger.LogWarning($"Recurring expense with ID {id} not found or could not be deleted.");
                return NotFound();
            }

            _logger.LogInformation($"Recurring expense with ID {id} deleted successfully.");
            return Ok();
        }
    }
}