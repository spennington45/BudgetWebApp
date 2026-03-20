using AutoMapper;
using budgetWebApp.Server.Interfaces;
using budgetWebApp.Server.Models;
using budgetWebApp.Server.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace budgetWebApp.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : AuthenticatedController
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserController(ILogger<UserController> logger, IUserRepository repository, IMapper mapper)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _userRepository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("GetUserById/{id}")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<User>> GetUserById(long id)
        {
            _logger.LogInformation($"Fetching user with ID {id}");

            var user = await _userRepository.GetUserByUserIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning($"User with ID {id} not found.");
                return NotFound();
            }

            var ownershipResult = ValidateOwnership(user.UserId);
            if (ownershipResult != null)
                return ownershipResult;

            return Ok(user);
        }

        [HttpPost("AddUser")]
        [ProducesResponseType(typeof(User), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<User>> AddUser([FromBody] UserDto user)
        {
            if (user == null || string.IsNullOrWhiteSpace(user.Email))
            {
                _logger.LogWarning("Invalid user creation request.");
                return BadRequest("Invalid user data.");
            }
            var newUser = _mapper.Map<User>(user);
            var createdUser = await _userRepository.AddUserAsync(newUser);
            if (createdUser == null)
            {
                _logger.LogError("Failed to create user.");
                return BadRequest("Could not create user.");
            }

            _logger.LogInformation($"User created successfully with ID {createdUser.UserId}.");
            return Ok(createdUser);
        }

        [HttpPut("UpdateUser")]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<User>> UpdateUser([FromBody] UserDto user)
        {
            if (user == null || user.UserId <= 0)
            {
                _logger.LogWarning("Invalid user update request.");
                return BadRequest("Invalid user data.");
            }

            var ownershipResult = ValidateOwnership(user.UserId);
            if (ownershipResult != null)
                return ownershipResult;

            var existingUser = await _userRepository.GetUserByUserIdAsync(user.UserId);
            if (existingUser == null)
                return NotFound();

            existingUser.Email = user.Email;
            existingUser.Name = user.Name;

            var updatedUser = await _userRepository.UpdateUserAsync(existingUser);
            if (updatedUser == null)
            {
                _logger.LogWarning($"User with ID {user.UserId} not found.");
                return NotFound();
            }

            _logger.LogInformation($"User {user.UserId} updated successfully.");
            return Ok(updatedUser);
        }

        [HttpPost("auth/google")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleTokenRequest request)
        {
            var http = new HttpClient();
            var response = await http.GetAsync($"https://oauth2.googleapis.com/tokeninfo?id_token={request.IdToken}");
            if (!response.IsSuccessStatusCode)
                return Unauthorized();

            var payload = JsonSerializer.Deserialize<GooglePayload>(await response.Content.ReadAsStringAsync());

            // TODO: create user in DB, issue JWT/cookie, etc.

            return Ok(new { token = "your-app-jwt-here" });
        }
    }
}