using Application.Interfaces.IdentityProvider;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Application.Common.Authorization;

public class CustomAuthorizeHandler(ILogger<CustomAuthorizeHandler> logger
    , IServiceProvider serviceProvider)
    : AuthorizationHandler<AuthorizationKeyAttribute>
{
    private readonly ILogger<CustomAuthorizeHandler> _logger = logger;
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        AuthorizationKeyAttribute requirement)
    {
        _logger.LogInformation("Authorization handler requirement: {requirement}.", requirement);
        using var scope = _serviceProvider.CreateScope();
        var identityProvider = scope.ServiceProvider.GetRequiredService<IIdentityProvider>();
        if (!identityProvider.IsAuthenticated())
        {
            context.Fail();
        }
        var role = requirement.Roles;
        if (role is null)
        {
            context.Succeed(requirement);
        }
        else
        {
            var auth = identityProvider.IsInRole(role);
            if (auth)
            {
                context.Succeed(requirement);
            }
        }
        return Task.CompletedTask;
    }
}
