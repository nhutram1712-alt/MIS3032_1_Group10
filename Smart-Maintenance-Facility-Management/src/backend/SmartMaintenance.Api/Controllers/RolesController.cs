using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartMaintenance.Domain.Enums;

namespace SmartMaintenance.Api.Controllers;

[ApiController]
[Route("api/roles")]
public sealed class RolesController : ControllerBase
{
    /// <summary>US-01-04 — fixed permission matrix for MVP (read-only).</summary>
    private static readonly Dictionary<string, string[]> PermissionsByRole = new(StringComparer.Ordinal)
    {
        [UserRoles.Requester] = ["ViewAsset", "CreateRequest", "TrackRequest"],
        [UserRoles.Technician] = ["ViewWorkOrder", "UpdateWorkOrder", "ViewIoTAlert"],
        [UserRoles.FacilityManager] = ["ManageAsset", "ManageRequest", "ManageWorkOrder", "ViewIoTData"],
        [UserRoles.Admin] = ["ManageUsers", "ManageRoles", "ManageIoTMapping"]
    };

    [HttpGet("permissions")]
    [Authorize(Roles = UserRoles.Admin)]
    [ProducesResponseType(typeof(Dictionary<string, string[]>), StatusCodes.Status200OK)]
    public ActionResult<Dictionary<string, string[]>> GetPermissions() => Ok(PermissionsByRole);
}
