using budgetWebApp.Server.Interfaces;
using budgetWebApp.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace budgetWebApp.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class BudgetTotalController : ControllerBase
    {
        private readonly ILogger<BudgetTotalController> _logger;
        private readonly IBudgetTotalRepository _budgetTotalRepository;

        public BudgetTotalController(ILogger<BudgetTotalController> logger, IBudgetTotalRepository repository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _budgetTotalRepository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet("GetBudgetTotalByUserId/{id}")]
        [ProducesResponseType(typeof(BudgetTotal), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BudgetTotal>> GetBudgetTotalByUserId(long id)
        {
            _logger.LogInformation($"Requesting budget total for user ID {id}");

            var budgetTotal = await _budgetTotalRepository.GetBudgetTotalByUserIdAsync(id);
            if (budgetTotal == null)
            {
                _logger.LogWarning($"No budget total found for user ID {id}");
                return NotFound();
            }

            return Ok(budgetTotal);
        }

        [HttpPut("UpdateBudgetTotal")]
        [ProducesResponseType(typeof(BudgetTotal), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BudgetTotal>> UpdateBudgetTotal([FromBody] BudgetTotal budgetTotal)
        {
            if (budgetTotal == null || budgetTotal.BudgetTotalId <= 0)
            {
                _logger.LogWarning("Invalid budget total update request.");
                return BadRequest("Invalid budget total data.");
            }

            var updatedBudgetTotal = await _budgetTotalRepository.UpdateBudgetTotalAsync(budgetTotal);
            if (updatedBudgetTotal == null)
            {
                _logger.LogWarning($"Budget total with ID {budgetTotal.BudgetTotalId} not found.");
                return NotFound();
            }

            _logger.LogInformation($"Budget total {budgetTotal.BudgetTotalId} updated successfully.");
            return Ok(updatedBudgetTotal);
        }

        [HttpPost("AddBudgetTotal")]
        [ProducesResponseType(typeof(BudgetTotal), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BudgetTotal>> AddBudgetTotal([FromBody] BudgetTotal budgetTotal)
        {
            if (budgetTotal == null || budgetTotal.UserId <= 0)
            {
                _logger.LogWarning("Invalid budget total creation request.");
                return BadRequest("Invalid budget total data.");
            }

            var createdBudgetTotal = await _budgetTotalRepository.AddBudgetTotalAsync(budgetTotal);
            if (createdBudgetTotal == null)
            {
                _logger.LogError("Failed to create budget total.");
                return BadRequest("Could not create budget total.");
            }

            _logger.LogInformation($"Budget total created successfully with ID {createdBudgetTotal.BudgetTotalId}.");
            return Ok(createdBudgetTotal);
        }
    }
}