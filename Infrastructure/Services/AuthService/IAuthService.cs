using Domain.DTOs.AuthDTOs;
using Domain.Responses;

namespace Infrastructure.Services.AuthService;

public interface IAuthService
{
    Task<Response<string>> Register(RegisterDto model);
    Task<Response<string>> Login(LoginDto model);
}