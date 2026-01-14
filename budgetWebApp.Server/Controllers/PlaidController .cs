using budgetWebApp.Server.Helpers;
using budgetWebApp.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace budgetWebApp.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PlaidController : ControllerBase
    {
        private readonly PlaidAuthService _plaidAuth;
        private readonly PlaidService _plaidService;
        private TokenRequest _tokenRequest;

        public PlaidController(PlaidAuthService plaidAuth, PlaidService plaidService)
        {
            _plaidAuth = plaidAuth;
            _plaidService = plaidService;
            _tokenRequest = new TokenRequest();
        }

        [HttpPost("exchange")]
        public async Task<IActionResult> ExchangeToken([FromBody] string publicToken)
        {
            var accessToken = await _plaidAuth.GetAccessTokenAsync(publicToken);
            return Ok(accessToken);
        }

        [HttpPost("create-link-token")]
        public async Task<IActionResult> CreateLinkToken([FromBody] string userId)
        {
            var linkToken = await _plaidAuth.CreateLinkTokenAsync(userId);
            return Ok(new { link_token = linkToken });
        }
    }
}
