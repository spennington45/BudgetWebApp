using AutoMapper;
using budgetWebApp.Server.Models;
using budgetWebApp.Server.Models.DTOs;
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
        private readonly IMapper _mapper;

        public PlaidController(PlaidAuthService plaidAuth, PlaidService plaidService, IMapper mapper)
        {
            _plaidAuth = plaidAuth;
            _plaidService = plaidService;
            _mapper = mapper;
        }

        [HttpPost("create-link-token")]
        public async Task<IActionResult> CreateLinkToken([FromBody] string userId)
        {
            var linkToken = await _plaidAuth.CreateLinkTokenAsync(userId);
            return Ok(new { link_token = linkToken });
        }

        [HttpPost("link")]
        public async Task<IActionResult> LinkPlaidAccount([FromBody] PlaidLinkRequestDto link)
        {
            var exchange = await _plaidAuth.GetAccessTokenAsync(link.PublicToken);

            var item = _mapper.Map<PlaidItem>(link);

            item.ItemId = exchange.ItemId;
            item.AccessToken = exchange.AccessToken;

            // TODO Add repo call to save data

            var accounts = _mapper.Map<List<PlaidAccount>>(link.Accounts);

            foreach (var acc in accounts)
                acc.PlaidItemId = item.PlaidItemId;

            // TODO Add repo call to save data

            return Ok();
        }
    }
}
