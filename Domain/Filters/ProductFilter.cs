using Domain.Enums;

namespace Domain.Filters;

public class ProductFilter : PaginationFilter
{
    public string? Name { get; set; }
    public Status? Status { get; set; }
}