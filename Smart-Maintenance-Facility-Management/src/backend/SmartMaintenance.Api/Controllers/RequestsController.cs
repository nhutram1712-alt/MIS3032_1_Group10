using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartMaintenance.Application.Abstractions;
using SmartMaintenance.Application.Requests;
using SmartMaintenance.Domain.Enums;

namespace SmartMaintenance.Api.Controllers;

[ApiController]
[Route("api/requests")]
public sealed class RequestsController : ControllerBase
{
    private readonly IRequestService _requestService;

    public RequestsController(IRequestService requestService)
    {
        _requestService = requestService;
    }

    [HttpPost]
    [Authorize(Roles = UserRoles.Requester)]
    [ProducesResponseType(typeof(RequestResponse), StatusCodes.Status201Created)]
    public async Task<ActionResult<RequestResponse>> Create([FromBody] CreateRequestPayload request, CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        if (!int.TryParse(userIdClaim, out var requesterId))
            return Unauthorized();

        var created = await _requestService.CreateAsync(request, requesterId, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, created);
    }

    [HttpGet]
    [Authorize(Roles = UserRoles.Requester + "," + UserRoles.FacilityManager)]
    [ProducesResponseType(typeof(IReadOnlyList<RequestResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<RequestResponse>>> List(CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        if (!int.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        var role = User.FindFirstValue(ClaimTypes.Role) ?? User.FindFirstValue("role") ?? string.Empty;
        var items = await _requestService.ListAsync(userId, role, cancellationToken);
        return Ok(items);
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = UserRoles.Requester + "," + UserRoles.FacilityManager)]
    [ProducesResponseType(typeof(RequestResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<RequestResponse>> GetById(int id, CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        if (!int.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        var role = User.FindFirstValue(ClaimTypes.Role) ?? User.FindFirstValue("role") ?? string.Empty;
        var item = await _requestService.GetByIdAsync(id, userId, role, cancellationToken);
        return Ok(item);
    }

    [HttpPatch("{id:int}/status")]
    [Authorize(Roles = UserRoles.FacilityManager)]
    [ProducesResponseType(typeof(RequestResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<RequestResponse>> UpdateStatus(int id, [FromBody] UpdateRequestStatusPayload request, CancellationToken cancellationToken)
    {
        var updated = await _requestService.UpdateStatusAsync(id, request, cancellationToken);
        return Ok(updated);
    }

    [HttpGet("{id:int}/history")]
    [Authorize(Roles = UserRoles.FacilityManager)]
    [ProducesResponseType(typeof(RequestHistoryResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<RequestHistoryResponse>> GetHistory(int id, CancellationToken cancellationToken)
    {
        var history = await _requestService.GetHistoryAsync(id, cancellationToken);
        return Ok(history);
    }
}
