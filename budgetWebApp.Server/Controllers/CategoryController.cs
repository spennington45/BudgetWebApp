using budgetWebApp.Server.Interfaces;
using budgetWebApp.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace budgetWebApp.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ILogger<CategoryController> logger, ICategoryRepository repository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _categoryRepository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet("GetAllCategories")]
        [ProducesResponseType(typeof(IEnumerable<Category>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Category>>> GetAllCategories()
        {
            _logger.LogInformation("Fetching all categories");

            var categories = await _categoryRepository.GetCategorysAsync();
            if (categories == null || !categories.Any())
            {
                _logger.LogWarning("No categories found.");
                return NotFound();
            }

            return Ok(categories);
        }

        [HttpGet("GetCategoryById/{id}")]
        [ProducesResponseType(typeof(Category), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Category>> GetCategoryById(long id)
        {
            _logger.LogInformation($"Fetching category with ID {id}");

            var category = await _categoryRepository.GetCategoryByCategoryIdAsync(id);
            if (category == null)
            {
                _logger.LogWarning($"Category with ID {id} not found.");
                return NotFound();
            }

            return Ok(category);
        }

        [HttpPost("AddCategory")]
        [ProducesResponseType(typeof(Category), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Category>> AddCategory([FromBody] Category category)
        {
            if (category == null || string.IsNullOrWhiteSpace(category.CategoryName))
            {
                _logger.LogWarning("Invalid category creation request.");
                return BadRequest("Invalid category data.");
            }

            var createdCategory = await _categoryRepository.AddCategoryAsync(category);
            if (createdCategory == null)
            {
                _logger.LogError("Failed to create category.");
                return BadRequest("Could not create category.");
            }

            _logger.LogInformation($"Category created successfully with ID {createdCategory.CategoryId}.");
            return Ok(createdCategory);
        }

        [HttpPut("UpdateCategory")]
        [ProducesResponseType(typeof(Category), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Category>> UpdateCategory([FromBody] Category category)
        {
            if (category == null || category.CategoryId <= 0)
            {
                _logger.LogWarning("Invalid category update request.");
                return BadRequest("Invalid category data.");
            }

            var existingCategory = await _categoryRepository.GetCategoryByCategoryIdAsync(category.CategoryId);
            if (existingCategory == null)
            {
                _logger.LogWarning($"Category with ID {category.CategoryId} not found.");
                return NotFound();
            }

            existingCategory.CategoryName = category.CategoryName;

            var updatedCategory = await _categoryRepository.UpdateCategory(existingCategory);
            if (updatedCategory == null)
            {
                _logger.LogWarning($"Category with ID {category.CategoryId} not found.");
                return NotFound();
            }

            _logger.LogInformation($"Category {category.CategoryId} updated successfully.");
            return Ok(updatedCategory);
        }

        [HttpDelete("DeleteCategory/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCategory(long id)
        {
            var success = await _categoryRepository.DeleteCategoryAsync(id);
            if (!success)
            {
                _logger.LogWarning($"Category with ID {id} not found or could not be deleted.");
                return NotFound();
            }

            _logger.LogInformation($"Category with ID {id} deleted successfully.");
            return Ok();
        }
    }
}