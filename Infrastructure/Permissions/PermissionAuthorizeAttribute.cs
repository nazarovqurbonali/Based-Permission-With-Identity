using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Permissions;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class PermissionAuthorizeAttribute : AuthorizeAttribute
{
    public PermissionAuthorizeAttribute(string requiredPermission)
    {
        AuthenticationSchemes = "Bearer";
        Policy = requiredPermission;
    }
}