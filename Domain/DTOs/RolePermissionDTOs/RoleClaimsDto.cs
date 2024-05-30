namespace Domain.DTOs.RolePermissionDTOs;

public class RoleClaimsDto
{
    public string Type { get; set; }
    public string Value { get; set; }
    public bool Selected { get; set; }

   
    public RoleClaimsDto(string type, string value, bool selected)
    {
        Type = type;
        Value = value;
        Selected = selected;
    }

    public RoleClaimsDto(string type, string value)
    {
        Type = type;
        Value = value;
    } 
}