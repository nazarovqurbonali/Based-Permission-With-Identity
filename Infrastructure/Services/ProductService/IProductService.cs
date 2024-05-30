using Domain.DTOs.ProductDTOs;
using Domain.Responses;

namespace Infrastructure.Services.ProductService;

public interface IProductService
{
    Task<Response<List<GetProductDto>>> GetProductsAsync();
    Task<Response<GetProductDto>> GetProductByIdAsync(int productId);
    Task<Response<string>> CreateProductAsync(CreateProductDto product);
    Task<Response<string>> UpdateProductAsync(UpdateProductDto product);
    Task<Response<bool>> DeleteProductAsync(int productId);
}