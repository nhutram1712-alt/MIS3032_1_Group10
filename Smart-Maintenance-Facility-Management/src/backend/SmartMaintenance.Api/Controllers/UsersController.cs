using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartMaintenance.Application.Abstractions;
using SmartMaintenance.Application.Users;
using SmartMaintenance.Domain.Enums;

namespace SmartMaintenance.Api.Controllers;

[ApiController]
[Route("api/users")]
public sealed class UsersController : ControllerBase
{
    private readonly IUserService _users;

    public UsersController(IUserService users)
    {
        _users = users;
    }

    [HttpGet]
    [Authorize(Roles = UserRoles.FacilityManager + "," + UserRoles.Admin)]
    [ProducesResponseType(typeof(IReadOnlyList<UserSummary>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<UserSummary>>> List([FromQuery] string? role, CancellationToken cancellationToken)
    {
        var isAdmin = User.IsInRole(UserRoles.Admin);
        if (!isAdmin && string.IsNullOrWhiteSpace(role))
            return BadRequest(new { error = "role query is required for FacilityManager." });

        var items = await _users.ListAsync(role, cancellationToken);
        return Ok(items);
    }

    [HttpPost]
    [Authorize(Roles = UserRoles.Admin)]
    [ProducesResponseType(typeof(UserSummary), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserSummary>> Create([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        if (!int.TryParse(userIdClaim, out var actorUserId))
            return Unauthorized();

        var created = await _users.CreateAsync(request, actorUserId, cancellationToken);
        return StatusCode(StatusCodes.Status201Created, created);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = UserRoles.Admin)]
    [ProducesResponseType(typeof(UserSummary), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserSummary>> Update(int id, [FromBody] UpdateUserRequest request, CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
        if (!int.TryParse(userIdClaim, out var actorUserId))
            return Unauthorized();

        var updated = await _users.UpdateAsync(id, request, actorUserId, cancellationToken);
        return Ok(updated);
    }
}
