using budgetWebApp.Server.Helpers;
using budgetWebApp.Server.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using Azure;

namespace budgetWebApp.Server.Controllers
{
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
