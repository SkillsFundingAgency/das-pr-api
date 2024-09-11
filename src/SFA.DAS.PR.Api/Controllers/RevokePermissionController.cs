using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.PR.Api.Authorization;

namespace SFA.DAS.PR.Api.Controllers;

[ExcludeFromCodeCoverage]
[Route("permissions")]
[ApiController]
public class RevokePermissionController : ControllerBase
{
    [Route("revoke")]
    [HttpPost]
    [Authorize(Policy = Policies.Integration)]
    [ProducesResponseType(StatusCodes.Status304NotModified)]
    public IActionResult Revoke()
    {
        /// This endpoint is used by recruit, but we do not want to support the endpoint going forward
        /// Hence just to avoid recruit process from failing we are returning 304 response
        return new StatusCodeResult(StatusCodes.Status304NotModified);
    }
}
