using System.Reflection;
using System.Security.Claims;
using Domain.DTOs.RolePermissionDTOs;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Helpers;

public static class ClaimsHelper
{
    public static void GetPermissions(this List<RoleClaimsDto> allPermissions, Type policy)
    {
        var nestedTypes = policy.GetNestedTypes(BindingFlags.Public);
        if (nestedTypes.Length > 0)
        {
            foreach (var nested in nestedTypes)
            {
                FieldInfo[] fields = nested.GetFields(BindingFlags.Static | BindingFlags.Public);

                foreach (FieldInfo fi in fields)
                {
                    allPermissions.Add(new RoleClaimsDto("Permissions", fi.GetValue(null).ToString()));
                }
            }
        }
        else
        {
            FieldInfo[] fields = policy.GetFields(BindingFlags.Static | BindingFlags.Public);

            foreach (FieldInfo fi in fields)
            {
                allPermissions.Add(new RoleClaimsDto(fi.GetValue(null).ToString(), "Permissions"));
            }
        }
    }

    public static async Task AddPermissionClaim(this RoleManager<IdentityRole> roleManager, IdentityRole role, string permission)
    {
        var allClaims = await roleManager.GetClaimsAsync(role);
        if (!allClaims.Any(a => a.Type == "Permission" && a.Value == permission))
        {
            await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
        }
    }

 
}