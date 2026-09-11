using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartMaintenance.Application.Abstractions;
using SmartMaintenance.Application.Iot;
using SmartMaintenance.Domain.Enums;

namespace SmartMaintenance.Api.Controllers;

[ApiController]
[Route("api/iot-mappings")]
public sealed class IotMappingsController : ControllerBase
{
    private readonly IIotService _iotService;

    public IotMappingsController(IIotService iotService)
    {
        _iotService = iotService;
    }

    [HttpPost]
    [Authorize(Roles = UserRoles.Admin)]
    [ProducesResponseType(typeof(IotMappingResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IotMappingResponse>> Create([FromBody] CreateIotMappingRequest request, CancellationToken cancellationToken)
    {
        var created = await _iotService.CreateMappingAsync(request, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, created);
    }

    [HttpGet]
    [Authorize(Roles = UserRoles.Admin)]
    [ProducesResponseType(typeof(IReadOnlyList<IotMappingResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<IotMappingResponse>>> List(CancellationToken cancellationToken)
    {
        var items = await _iotService.ListMappingsAsync(cancellationToken);
        return Ok(items);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = UserRoles.Admin)]
    [ProducesResponseType(typeof(IotMappingResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IotMappingResponse>> Update(int id, [FromBody] UpdateIotMappingRequest request, CancellationToken cancellationToken)
    {
        var updated = await _iotService.UpdateMappingAsync(id, request, cancellationToken);
        return Ok(updated);
    }
}
