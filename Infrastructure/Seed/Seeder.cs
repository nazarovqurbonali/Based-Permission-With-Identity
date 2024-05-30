using Domain.Constants;
using Domain.DTOs.RolePermissionDTOs;
using Infrastructure.Helpers;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Seed;

public class Seeder(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
{
    public async Task Initial()
    {
        await SeedRole();
        await AddAdminPermissions();
        await AddUserPermissions();
        await DefaultUsers();
    }


    #region SeedRole

    private async Task SeedRole()
    {
        try
        {
            var newRoles = new List<IdentityRole>()
            {
                new(Roles.SuperAdmin),
                new(Roles.Admin),
                new(Roles.User)
            };

            var existing = roleManager.Roles.ToList();
            foreach (var role in newRoles)
            {
                if (existing.Exists(e => e.Name == role.Name) == false)
                {
                    await roleManager.CreateAsync(role);
                }
            }
        }
        catch (Exception e)
        {
            //ignored
        }
    }

    #endregion

    #region DefaultUsers

    private async Task DefaultUsers()
    {
        try
        {
            //super-admin
            var existingSuperAdmin = await userManager.FindByNameAsync("SuperAdmin");
            if (existingSuperAdmin is null)
            {
                var superAdmin = new IdentityUser()
                {
                    Email = "superadmin@gmail.com",
                    PhoneNumber = "123456780",
                    UserName = "SuperAdmin"
                };
                await userManager.CreateAsync(superAdmin, "1234");
                await userManager.AddToRoleAsync(superAdmin, Roles.SuperAdmin);
            }


            //admin
            var existingAdmin = await userManager.FindByNameAsync("Admin");
            if (existingAdmin is null)
            {
                var admin = new IdentityUser()
                {
                    Email = "admin@gmail.com",
                    PhoneNumber = "123456780",
                    UserName = "Admin"
                };
                await userManager.CreateAsync(admin, "1234");
                await userManager.AddToRoleAsync(admin, Roles.Admin);
            }

            //user
            var existingUser = await userManager.FindByNameAsync("User");
            if (existingUser is null)
            {
                var user = new IdentityUser()
                {
                    Email = "user@gmail.com",
                    PhoneNumber = "123456780",
                    UserName = "User"
                };
                await userManager.CreateAsync(user, "1234");
                await userManager.AddToRoleAsync(user, Roles.User);
            }

            await SeedClaimsForSuperAdmin();
        }
        catch (Exception e)
        {
            //ignored;
        }
    }

    #endregion

    #region SeedClaimsForSuperAdmin

    private async Task SeedClaimsForSuperAdmin()
    {
        try
        {
            var adminRole = await roleManager.FindByNameAsync("SuperAdmin");
            if (adminRole == null) return;
            var roleClaims = new List<RoleClaimsDto>();
            roleClaims.GetPermissions(typeof(Domain.Constants.Permissions));
            var existingClaims = await roleManager.GetClaimsAsync(adminRole);
            foreach (var claim in roleClaims)
            {
                if (existingClaims.Any(c => c.Value == claim.Value) == false)
                    await roleManager.AddPermissionClaim(adminRole, claim.Value);
            }
        }
        catch (Exception ex)
        {
            // ignored
        }
    }

    #endregion


    private async Task AddUserPermissions()
    {
        //add claims
        var userRole = await roleManager.FindByNameAsync(Roles.User);
        if (userRole == null) return;
        var userClaims = new List<RoleClaimsDto>()
        {
            new("Permissions", Domain.Constants.Permissions.Products.View),
        };

        var existingClaim = await roleManager.GetClaimsAsync(userRole);
        foreach (var claim in userClaims)
        {
            if (!existingClaim.Any(x => x.Type == claim.Type && x.Value == claim.Value))
            {
                await roleManager.AddPermissionClaim(userRole, claim.Value);
            }
        }
    }

    private async Task AddAdminPermissions()
    {
        //add claims
        var adminRole = await roleManager.FindByNameAsync(Roles.Admin);
        if (adminRole == null) return;
        var userClaims = new List<RoleClaimsDto>()
        {
            new("Permissions", Domain.Constants.Permissions.Products.View),
            new("Permissions", Domain.Constants.Permissions.Products.Create),
            new("Permissions", Domain.Constants.Permissions.Products.Edit),
        };

        var existingClaim = await roleManager.GetClaimsAsync(adminRole);
        foreach (var claim in userClaims)
        {
            if (!existingClaim.Any(x => x.Type == claim.Type && x.Value == claim.Value))
            {
                await roleManager.AddPermissionClaim(adminRole, claim.Value);
            }
        }
    }
}