using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.AuthDTOs;

public class RegisterDto
{
    public required string UserName { get; set; }
    public required string Email { get; set; }
    [DataType(DataType.Password)]
    public required string Password { get; set; }
    [Compare("Password"), DataType(DataType.Password)]
    public required string ConfirmPassword { get; set; }
}