using Domain.DTOs.AuthDTOs;
using Domain.Responses;
using Infrastructure.Services.AuthService;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService service):ControllerBase
{
    [HttpPost("login")]
    public async Task<Response<string>> LoginAsync(LoginDto loginDto)
    {
      return  await service.Login(loginDto);
    }

    [HttpPost("register")]
    public async Task<Response<string>> RegisterAsync(RegisterDto registerDto)
        => await service.Register(registerDto);
}