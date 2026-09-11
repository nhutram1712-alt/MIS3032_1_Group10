using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartMaintenance.Api.Security;
using SmartMaintenance.Application.Abstractions;
using SmartMaintenance.Application.Iot;

namespace SmartMaintenance.Api.Controllers;

[ApiController]
[Route("api/iot")]
public sealed class IotController : ControllerBase
{
    private readonly IIotService _iotService;

    public IotController(IIotService iotService)
    {
        _iotService = iotService;
    }

    /// <summary>US-05-03 — Ingest IoT metrics from Gateway (API Key).</summary>
    [HttpPost("ingest")]
    [AllowAnonymous]
    [IotGatewayAuthorize]
    [ProducesResponseType(typeof(IotIngestResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<IotIngestResponse>> Ingest([FromBody] IotIngestRequest request, CancellationToken cancellationToken)
    {
        var saved = await _iotService.IngestAsync(request, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, saved);
    }
}
