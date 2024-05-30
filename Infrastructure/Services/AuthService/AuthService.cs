using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Domain.Constants;
using Domain.DTOs.AuthDTOs;
using Domain.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services.AuthService;

public class AuthService(IConfiguration configuration, UserManager<IdentityUser> userManager,RoleManager<IdentityRole> roleManager) : IAuthService
{
    public async Task<Response<string>> Register(RegisterDto model)
    {
        try
        {
            var result = await userManager.FindByNameAsync(model.UserName);
            if (result != null) return new Response<string>(HttpStatusCode.BadRequest, "Such a user already exists!");
            var user = new IdentityUser()
            {
                UserName = model.UserName,
                Email = model.Email,
            };

            await userManager.CreateAsync(user, model.Password);
            await userManager.AddToRoleAsync(user, Roles.User);


            return new Response<string>($"Done.  Your registered by id {user.Id}");
        }
        catch (Exception e)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<string>> Login(LoginDto model)
    {
        try
        {
            var user = await userManager.FindByNameAsync(model.UserName);
            if (user != null)
            {
                var result = await userManager.CheckPasswordAsync(user, model.Password);
                if (result)
                {
                    return new Response<string>(await GenerateJwtToken(user));
                }
            }

            return new Response<string>(HttpStatusCode.BadRequest, "Your username or password is incorrect!!!");
        }
        catch (Exception e)
        {
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    private async Task<string> GenerateJwtToken(IdentityUser user)
    {
        var key = Encoding.UTF8.GetBytes(configuration["JWT:Key"]!);
        var securityKey = new SymmetricSecurityKey(key);
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new List<Claim>()
        {
            new(JwtRegisteredClaimNames.Sid, user.Id),
            new(JwtRegisteredClaimNames.Name, user.UserName!),
            new(JwtRegisteredClaimNames.Email, user.Email!),
        };
        //add roles
        
        var roles = await userManager.GetRolesAsync(user);
        
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
            var roleClaims = await roleManager.GetClaimsAsync((await roleManager.FindByNameAsync(role))!);
            foreach (var roleClaim in roleClaims)
            {
                claims.Add(new Claim("Permissions", roleClaim.Value));
            }
        }
        var token = new JwtSecurityToken(
            issuer: configuration["JWT:Issuer"],
            audience: configuration["JWT:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials
        );

        var securityTokenHandler = new JwtSecurityTokenHandler();
        var tokenString = securityTokenHandler.WriteToken(token);
        return tokenString;
    }
}