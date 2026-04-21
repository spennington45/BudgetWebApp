using AutoMapper;
using budgetWebApp.Server.Interfaces;
using budgetWebApp.Server.Models.DTOs;
using budgetWebApp.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace budgetWebApp.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PlaidController : AuthenticatedController
    {
        private readonly PlaidAuthService _plaidAuth;
        private readonly PlaidService _plaidService;
        private readonly IMapper _mapper;
        private readonly IPlaidRepository _plaidRepository;

        public PlaidController(PlaidAuthService plaidAuth, PlaidService plaidService, IMapper mapper, IPlaidRepository plaidRepository)
        {
            _plaidAuth = plaidAuth;
            _plaidService = plaidService;
            _mapper = mapper;
            _plaidRepository = plaidRepository;
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
            if (link == null || link.Accounts == null)
                return BadRequest("Invalid Plaid link request.");

            var ownershipResult = ValidateOwnership(link.UserId);
            if (ownershipResult != null)
                return ownershipResult;

            var newItem = await _plaidRepository.AddPlaidItemAndAccountsTransactionAsync(link);

            return Ok(newItem);
        }

        [HttpPost("syncPladItemByItemId")]
        public async Task<IActionResult> SyncPladItemByItemId([FromBody] PlaidItemSyncDto itemSync)
        {
            if (itemSync == null || itemSync.pladItemId == string.Empty)
                return BadRequest();

            var ownershipResult = ValidateOwnership(itemSync.userId);
            if (ownershipResult != null)
                return ownershipResult;

            var existingItem = await _plaidRepository.GetPlaidItemByItemId(itemSync.pladItemId);
            if (existingItem == null)
                return NotFound();

            await _plaidRepository.SyncTransactionsForItemAsync(existingItem.PlaidItemId);
            return Ok();
        }

        [HttpPost("syncPladItemsByUserId")]
        public async Task<IActionResult> SyncPladItemsByUserId([FromBody] PlaidItemSyncDto itemSync)
        {
            if (itemSync == null)
                return BadRequest();

            var ownershipResult = ValidateOwnership(itemSync.userId);
            if (ownershipResult != null)
                return ownershipResult;

            var existingItems = await _plaidRepository.GetPladItemsByUserId(itemSync.userId);
            if (existingItems == null)
                return NotFound();

            foreach (var item in existingItems)
                await _plaidRepository.SyncTransactionsForItemAsync(item.PlaidItemId);

            return Ok();
        }
    }
}
