namespace Domain.DTOs.AuthDTOs;

public class LoginDto
{
    public required string UserName { get; set; }
    public required string Password { get; set; }
}