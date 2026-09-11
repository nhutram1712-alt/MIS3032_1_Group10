using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartMaintenance.Application.Abstractions;
using SmartMaintenance.Application.Iot;
using SmartMaintenance.Domain.Enums;

namespace SmartMaintenance.Api.Controllers;

[ApiController]
[Route("api/iot-alerts")]
public sealed class IotAlertsController : ControllerBase
{
    private readonly IIotService _iotService;

    public IotAlertsController(IIotService iotService)
    {
        _iotService = iotService;
    }

    [HttpGet]
    [Authorize(Roles = UserRoles.FacilityManager + "," + UserRoles.Technician)]
    [ProducesResponseType(typeof(IReadOnlyList<IotAlertResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<IotAlertResponse>>> List([FromQuery] int? assetId, CancellationToken cancellationToken)
    {
        var items = await _iotService.ListAlertsAsync(assetId, cancellationToken);
        return Ok(items);
    }
}
