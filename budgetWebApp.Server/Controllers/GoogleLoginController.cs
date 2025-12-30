using budgetWebApp.Server.Helpers;
using budgetWebApp.Server.Interfaces;
using budgetWebApp.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace budgetWebApp.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GoogleLoginController : ControllerBase
    {
        private readonly ILogger<GoogleLoginController> _logger;
        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepository;

        public GoogleLoginController(ILogger<GoogleLoginController> logger, IConfiguration config, IUserRepository userRepository)
        {
            _logger = logger;
            _config = config;
            _userRepository = userRepository;
        }

        [HttpGet("ClientId")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> GetClientId()
        {
            _logger.LogInformation($"Fetching google client id");

            return Ok(GoogleLoginConfig.GoogleClientId);
        }

        [HttpPost("auth/google")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleTokenRequest request)
        {
            _logger.LogInformation("Received Google login request");

            if (string.IsNullOrWhiteSpace(request.IdToken))
            {
                _logger.LogWarning("Google login failed: missing ID token");
                return BadRequest("Missing token");
            }

            try
            {
                _logger.LogInformation("Validating Google ID token with Google API");
                var http = new HttpClient();
                var response = await http.GetAsync($"https://oauth2.googleapis.com/tokeninfo?id_token={request.IdToken}");
                if (!response.IsSuccessStatusCode)
                    return Unauthorized();

                var payload = JsonSerializer.Deserialize<GooglePayload>(await response.Content.ReadAsStringAsync());

                _logger.LogInformation(
                    "Google token validated successfully for user {Email}, GoogleId {GoogleId}",
                    payload.Email,
                    payload.Sub
                );

                var user = await _userRepository.GetUserByUserEmailAsync(payload.Email);

                if (user == null)
                {
                    _logger.LogInformation("User not found in DB. Creating new user.");

                    user = new User
                    {
                        ExternalId = payload.Sub,
                        Email = payload.Email,
                        Name = payload.Name,
                        Picture = payload.Picture,
                        CreatedAt = DateTime.UtcNow
                    };

                    await _userRepository.AddUserAsync(user);
                }
                else
                {
                    _logger.LogInformation("Existing user found: {Email}", user.Email);
                }


                var claims = new List<Claim>();

                if (!string.IsNullOrEmpty(payload.Sub))
                    claims.Add(new Claim(JwtRegisteredClaimNames.Sub, payload.Sub));

                if (!string.IsNullOrEmpty(payload.Email))
                    claims.Add(new Claim(JwtRegisteredClaimNames.Email, payload.Email));

                if (!string.IsNullOrEmpty(payload.Name))
                    claims.Add(new Claim("name", payload.Name));

                if (user != null)
                {
                    claims.Add(new Claim("externalId", user.ExternalId));
                    claims.Add(new Claim("userId", user.UserId.ToString()));
                }

                var key = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _config["Jwt:Issuer"],
                    audience: _config["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(1),
                    signingCredentials: creds
                );

                var jwt = new JwtSecurityTokenHandler().WriteToken(token);

                _logger.LogInformation(
                    "JWT issued for user {Email} with expiration {Expiration}",
                    payload.Email,
                    token.ValidTo
                );

                return Ok(new { token = jwt });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during Google login");
                return StatusCode(500, "Internal server error");
            }

        }
    }
}
