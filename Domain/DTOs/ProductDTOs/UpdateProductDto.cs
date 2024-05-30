using Domain.Enums;

namespace Domain.DTOs.ProductDTOs;

public class UpdateProductDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public Status Status { get; set; }
    public string? Description { get; set; }
}