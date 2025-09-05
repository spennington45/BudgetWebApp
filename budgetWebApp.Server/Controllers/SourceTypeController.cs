using budgetWebApp.Server.Interfaces;
using budgetWebApp.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace budgetWebApp.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SourceTypeController : ControllerBase
    {
        private readonly ILogger<SourceTypeController> _logger;
        private readonly ISourceTypeRepository _sourceTypeRepository;

        public SourceTypeController(ILogger<SourceTypeController> logger, ISourceTypeRepository repository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _sourceTypeRepository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet("GetAllSourceTypes")]
        [ProducesResponseType(typeof(IEnumerable<SourceType>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<SourceType>>> GetAllSourceTypes()
        {
            _logger.LogInformation("Fetching all source types");

            var sourceTypes = await _sourceTypeRepository.GetSourceTypesAsync();
            if (sourceTypes == null || !sourceTypes.Any())
            {
                _logger.LogWarning("No source types found.");
                return NotFound();
            }

            return Ok(sourceTypes);
        }

        [HttpGet("GetSourceTypeById/{id}")]
        [ProducesResponseType(typeof(SourceType), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SourceType>> GetSourceTypeById(long id)
        {
            _logger.LogInformation($"Fetching source type with ID {id}");

            var sourceType = await _sourceTypeRepository.GetSourceTypeBySourceTypeIdAsync(id);
            if (sourceType == null)
            {
                _logger.LogWarning($"Source type with ID {id} not found.");
                return NotFound();
            }

            return Ok(sourceType);
        }

        [HttpPost("AddSourceType")]
        [ProducesResponseType(typeof(SourceType), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SourceType>> AddSourceType([FromBody] SourceType sourceType)
        {
            if (sourceType == null || string.IsNullOrWhiteSpace(sourceType.SourceName))
            {
                _logger.LogWarning("Invalid source type creation request.");
                return BadRequest("Invalid source type data.");
            }

            var createdSourceType = await _sourceTypeRepository.AddSourceTypeAsync(sourceType);
            if (createdSourceType == null)
            {
                _logger.LogError("Failed to create source type.");
                return BadRequest("Could not create source type.");
            }

            _logger.LogInformation($"Source type created successfully with ID {createdSourceType.SourceTypeId}.");
            return Ok(createdSourceType);
        }

        [HttpPut("UpdateSourceType")]
        [ProducesResponseType(typeof(SourceType), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SourceType>> UpdateSourceType([FromBody] SourceType sourceType)
        {
            if (sourceType == null || sourceType.SourceTypeId <= 0)
            {
                _logger.LogWarning("Invalid source type update request.");
                return BadRequest("Invalid source type data.");
            }

            var updatedSourceType = await _sourceTypeRepository.UpdateSourceType(sourceType);
            if (updatedSourceType == null)
            {
                _logger.LogWarning($"Source type with ID {sourceType.SourceTypeId} not found.");
                return NotFound();
            }

            _logger.LogInformation($"Source type {sourceType.SourceTypeId} updated successfully.");
            return Ok(updatedSourceType);
        }

        [HttpDelete("DeleteSourceType/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteSourceType(long id)
        {
            var success = await _sourceTypeRepository.DeleteSourceTypeAsync(id);
            if (!success)
            {
                _logger.LogWarning($"Source type with ID {id} not found or could not be deleted.");
                return NotFound();
            }

            _logger.LogInformation($"Source type with ID {id} deleted successfully.");
            return Ok();
        }
    }
}