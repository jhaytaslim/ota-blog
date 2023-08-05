using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Authorization;

public sealed class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(string permission) : base(policy: permission.ToString())
    {
    }
}