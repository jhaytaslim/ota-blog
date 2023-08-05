using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Authorization;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public PermissionAuthorizationHandler(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {

        string? email = context.User.Claims.FirstOrDefault(x => x.Type == "Email")?.Value;
        if (email == null)
        {
            return;
        }

        using IServiceScope scope = _serviceScopeFactory.CreateScope();

        IPermissionService permissionService = scope.ServiceProvider.GetRequiredService<IPermissionService>();


        List<string>? userPermissions = await permissionService.GetPermissionsAsync(email);
        if (userPermissions is null)
            return;

        List<string> requiredPermissions = requirement.Permission.Split(',').ToList();
        List<string> permissions = userPermissions.Intersect(requiredPermissions).ToList();

        if (permissions.Count > 0)
        {
            context.Succeed(requirement);
        }

        return;
    }
}