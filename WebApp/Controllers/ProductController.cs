using Domain.Constants;
using Domain.DTOs.ProductDTOs;
using Domain.Responses;
using Infrastructure.Permissions;
using Infrastructure.Services.ProductService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ProductController(IProductService productService) : ControllerBase
{

    [HttpGet("products")]
    [PermissionAuthorize(Permissions.Products.View)]
    public async Task<Response<List<GetProductDto>>> GetProducts()
    {
       return await productService.GetProductsAsync();
    }

    [HttpGet("{productId:int}")]
    [PermissionAuthorize(Permissions.Products.View)]
    public async Task<Response<GetProductDto>> GetProductById(int productId)
        => await productService.GetProductByIdAsync(productId);

    [HttpPost("create")]
    [PermissionAuthorize(Permissions.Products.Create)]
    public async Task<Response<string>> CreateProduct(CreateProductDto create)
        => await productService.CreateProductAsync(create);

    [HttpPut("update")]
    [PermissionAuthorize(Permissions.Products.Edit)]
    public async Task<Response<string>> UpdateProduct(UpdateProductDto updateProductDto)
        => await productService.UpdateProductAsync(updateProductDto);

    [HttpDelete("{productId:int}")]
    [PermissionAuthorize(Permissions.Products.Delete)]
    public async Task<Response<bool>> DeleteProduct(int productId)
        => await productService.DeleteProductAsync(productId);
}