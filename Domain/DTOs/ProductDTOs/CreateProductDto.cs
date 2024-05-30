using Domain.Enums;

namespace Domain.DTOs.ProductDTOs;

public class CreateProductDto
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public Status Status { get; set; }
}