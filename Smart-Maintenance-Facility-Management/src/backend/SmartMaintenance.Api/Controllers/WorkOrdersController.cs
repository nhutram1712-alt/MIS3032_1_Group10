using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartMaintenance.Application.Abstractions;
using SmartMaintenance.Application.WorkOrders;
using SmartMaintenance.Domain.Enums;

namespace SmartMaintenance.Api.Controllers;

[ApiController]
[Route("api/work-orders")]
public sealed class WorkOrdersController : ControllerBase
{
    private readonly IWorkOrderService _workOrderService;

    public WorkOrdersController(IWorkOrderService workOrderService)
    {
        _workOrderService = workOrderService;
    }

    [HttpPost]
    [Authorize(Roles = UserRoles.FacilityManager)]
    [ProducesResponseType(typeof(WorkOrderResponse), StatusCodes.Status201Created)]
    public async Task<ActionResult<WorkOrderResponse>> Create([FromBody] CreateWorkOrderPayload request, CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        if (!int.TryParse(userIdClaim, out var facilityManagerId))
            return Unauthorized();

        var created = await _workOrderService.CreateAsync(request, facilityManagerId, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, created);
    }

    [HttpGet]
    [Authorize(Roles = UserRoles.FacilityManager + "," + UserRoles.Technician)]
    [ProducesResponseType(typeof(IReadOnlyList<WorkOrderResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<WorkOrderResponse>>> List(CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        if (!int.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        var role = User.FindFirstValue(ClaimTypes.Role) ?? User.FindFirstValue("role") ?? string.Empty;
        var items = await _workOrderService.ListAsync(userId, role, cancellationToken);
        return Ok(items);
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = UserRoles.FacilityManager + "," + UserRoles.Technician)]
    [ProducesResponseType(typeof(WorkOrderDetailResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<WorkOrderDetailResponse>> GetById(int id, CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        if (!int.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        var role = User.FindFirstValue(ClaimTypes.Role) ?? User.FindFirstValue("role") ?? string.Empty;
        var item = await _workOrderService.GetByIdAsync(id, userId, role, cancellationToken);
        return Ok(item);
    }

    [HttpPatch("{id:int}")]
    [Authorize(Roles = UserRoles.FacilityManager + "," + UserRoles.Technician)]
    [ProducesResponseType(typeof(WorkOrderResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<WorkOrderResponse>> Patch(int id, [FromBody] PatchWorkOrderPayload request, CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        if (!int.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        var role = User.FindFirstValue(ClaimTypes.Role) ?? User.FindFirstValue("role") ?? string.Empty;
        var updated = await _workOrderService.PatchAsync(id, request, userId, role, cancellationToken);
        return Ok(updated);
    }
}
