namespace Domain.DTOs.RolePermissionDTOs;

public class PermissionDto
{
    public required string RoleId { get; set; }
    public  List<RoleClaimsDto>? RoleClaim { get; set; }
}