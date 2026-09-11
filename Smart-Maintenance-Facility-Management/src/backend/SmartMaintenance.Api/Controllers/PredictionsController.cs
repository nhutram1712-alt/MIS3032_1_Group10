using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartMaintenance.Application.Abstractions;
using SmartMaintenance.Application.Predictions;
using SmartMaintenance.Domain.Enums;

namespace SmartMaintenance.Api.Controllers;

[ApiController]
[Route("api/predictions")]
public sealed class PredictionsController : ControllerBase
{
    private readonly IPredictionService _predictionService;

    public PredictionsController(IPredictionService predictionService)
    {
        _predictionService = predictionService;
    }

    [HttpGet]
    [Authorize(Roles = UserRoles.FacilityManager)]
    [ProducesResponseType(typeof(IReadOnlyList<PredictionResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<PredictionResponse>>> List([FromQuery] string? sort, CancellationToken cancellationToken)
    {
        var items = await _predictionService.ListAllAsync(sort, cancellationToken);
        return Ok(items);
    }
}
