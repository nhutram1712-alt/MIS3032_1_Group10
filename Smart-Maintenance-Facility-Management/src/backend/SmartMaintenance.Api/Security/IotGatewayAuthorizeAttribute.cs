using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SmartMaintenance.Api.Security;

public sealed class IotGatewayAuthorizeAttribute : Attribute, IAsyncAuthorizationFilter
{
    public Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var config = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
        var expected = config["Iot:GatewayApiKey"];
        if (string.IsNullOrWhiteSpace(expected)
            || !context.HttpContext.Request.Headers.TryGetValue("X-Api-Key", out var provided)
            || !string.Equals(provided.ToString(), expected, StringComparison.Ordinal))
        {
            context.Result = new UnauthorizedObjectResult(new { error = "Invalid or missing gateway API key." });
        }

        return Task.CompletedTask;
    }
}
