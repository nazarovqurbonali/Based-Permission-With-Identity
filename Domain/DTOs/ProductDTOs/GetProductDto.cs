namespace Domain.DTOs.ProductDTOs;

public class GetProductDto
{
    public int Id { get; set; }
    public required string Name { get; set; } 
    public string? Description { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public required string Status { get; set; }
}