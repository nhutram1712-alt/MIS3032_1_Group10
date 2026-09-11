using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartMaintenance.Application.Abstractions;
using SmartMaintenance.Application.Assets;
using SmartMaintenance.Application.Iot;
using SmartMaintenance.Application.Predictions;
using SmartMaintenance.Domain.Enums;

namespace SmartMaintenance.Api.Controllers;

[ApiController]
[Route("api/assets")]
public sealed class AssetsController : ControllerBase
{
    private readonly IAssetService _assetService;
    private readonly IIotService _iotService;
    private readonly IPredictionService _predictionService;

    public AssetsController(
        IAssetService assetService,
        IIotService iotService,
        IPredictionService predictionService)
    {
        _assetService = assetService;
        _iotService = iotService;
        _predictionService = predictionService;
    }

    [HttpPost]
    [Authorize(Roles = UserRoles.FacilityManager)]
    [ProducesResponseType(typeof(AssetResponse), StatusCodes.Status201Created)]
    public async Task<ActionResult<AssetResponse>> Create([FromBody] CreateAssetRequest request, CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        if (!int.TryParse(userIdClaim, out var facilityManagerId))
            return Unauthorized();

        var created = await _assetService.CreateAsync(request, facilityManagerId, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, created);
    }

    [HttpGet]
    [Authorize(Roles = UserRoles.Requester + "," + UserRoles.FacilityManager + "," + UserRoles.Technician + "," + UserRoles.Admin)]
    [ProducesResponseType(typeof(IReadOnlyList<AssetResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<AssetResponse>>> List([FromQuery] string? location, CancellationToken cancellationToken)
    {
        var items = await _assetService.ListAsync(location, cancellationToken);
        return Ok(items);
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = UserRoles.Requester + "," + UserRoles.FacilityManager + "," + UserRoles.Technician + "," + UserRoles.Admin)]
    [ProducesResponseType(typeof(AssetResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AssetResponse>> GetById(int id, CancellationToken cancellationToken)
    {
        var item = await _assetService.GetByIdAsync(id, cancellationToken);
        return Ok(item);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = UserRoles.FacilityManager)]
    [ProducesResponseType(typeof(AssetResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<AssetResponse>> Update(int id, [FromBody] UpdateAssetRequest request, CancellationToken cancellationToken)
    {
        var updated = await _assetService.UpdateAsync(id, request, cancellationToken);
        return Ok(updated);
    }

    [HttpPatch("{id:int}/status")]
    [Authorize(Roles = UserRoles.FacilityManager)]
    [ProducesResponseType(typeof(AssetResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<AssetResponse>> UpdateStatus(int id, [FromBody] UpdateAssetStatusRequest request, CancellationToken cancellationToken)
    {
        var updated = await _assetService.UpdateStatusAsync(id, request, cancellationToken);
        return Ok(updated);
    }

    [HttpGet("{id:int}/iot-data")]
    [Authorize(Roles = UserRoles.FacilityManager + "," + UserRoles.Technician)]
    [ProducesResponseType(typeof(IReadOnlyList<IotDataItem>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<IotDataItem>>> GetIotData(int id, CancellationToken cancellationToken)
    {
        var data = await _iotService.GetByAssetAsync(id, cancellationToken);
        return Ok(data);
    }

    [HttpGet("{id:int}/prediction")]
    [Authorize(Roles = UserRoles.FacilityManager + "," + UserRoles.Technician)]
    [ProducesResponseType(typeof(PredictionResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<PredictionResponse>> GetPrediction(int id, CancellationToken cancellationToken)
    {
        var prediction = await _predictionService.GetLatestAsync(id, cancellationToken);
        return Ok(prediction);
    }
}
