using Microsoft.AspNetCore.Mvc;

namespace budgetWebApp.Server.Controllers
{
    public abstract class AuthenticatedController : ControllerBase
    {
        protected long? CurrentUserId
        {
            get
            {
                var claim = User.FindFirst("userId");
                return long.TryParse(claim?.Value, out var id) ? id : null;
            }
        }

        protected ActionResult ValidateOwnership(long ownerId)
        {
            var userId = CurrentUserId;
            if (userId is null)
                return Unauthorized();

            if (ownerId != userId)
                return Forbid();

            return null;
        }
    }
}
