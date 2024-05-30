using System.Net;
using Domain.DTOs.ProductDTOs;
using Domain.Entities;
using Domain.Enums;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.ProductService;

public class ProductService(DataContext context, ILogger<ProductService> logger) : IProductService
{
    public async Task<Response<List<GetProductDto>>> GetProductsAsync()
    {
        try
        {
            logger.LogInformation("Starting method GetProductsAsync in time: {DateTimeNow}", DateTime.Now);
            var products = await context.Products.Select(x => new GetProductDto()
            {
                Name = x.Name,
                Id = x.Id,
                Status = x.Status.ToString(),
                Description = x.Description,
                CreatedAt = x.CreatedAt
            }).ToListAsync();
            logger.LogInformation("Finished method GetProductsAsync in time: {DateTimeNow}", DateTime.Now);

            return new Response<List<GetProductDto>>(products);
        }
        catch (Exception e)
        {
            logger.LogError("Error in method GetProductsAsync at time: {DateTimeNow}, error: {EMessage}", DateTime.Now,
                e.Message);
            return new Response<List<GetProductDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<GetProductDto>> GetProductByIdAsync(int productId)
    {
        try
        {
            logger.LogInformation("Starting method GetProductByIdAsync in time: {DateTimeNow}", DateTime.Now);
            var existingProduct = await context.Products.Where(x => x.Id == productId).Select(x =>
                new GetProductDto()
                {
                    Name = x.Name,
                    Id = x.Id,
                    Status = x.Status.ToString(),
                    Description = x.Description,
                    CreatedAt = x.CreatedAt
                }).FirstOrDefaultAsync();
            if (existingProduct == null)
            {
                logger.LogWarning("Not found product with Id={Id}, at time {DateTimeNow}", productId, DateTime.Now);
                return new Response<GetProductDto>(HttpStatusCode.BadRequest, "Product not found");
            }

            logger.LogInformation("Finished method GetProductByIdAsync in time: {DateTimeNow}", DateTime.Now);
            return new Response<GetProductDto>(existingProduct);
        }
        catch (Exception e)
        {
            logger.LogError("Error in method GetProductByIdAsync at time: {DateTimeNow}, error: {EMessage}",
                DateTime.Now, e.Message);
            return new Response<GetProductDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<string>> CreateProductAsync(CreateProductDto product)
    {
        try
        {
            logger.LogInformation("Starting method CreateProductAsync in time: {DateTimeNow}", DateTime.Now);
            var existingProduct = await context.Products.AnyAsync(x => x.Name == product.Name);
            if (existingProduct)
            {
                logger.LogWarning("Product already exists with Name={Name}, at time {DateTimeNow}", product.Name,
                    DateTime.Now);
                return new Response<string>(HttpStatusCode.BadRequest, "Product already exists");
            }

            var newProduct = new Product
            {
                Name = product.Name,
                Description = product.Description,
                CreatedAt = DateTimeOffset.UtcNow,
                Status = Status.Active
            };
            await context.Products.AddAsync(newProduct);
            await context.SaveChangesAsync();

            logger.LogInformation("Finished method CreateProductAsync in time: {DateTimeNow}", DateTime.Now);
            return new Response<string>("Successfully created product");
        }
        catch (Exception e)
        {
            logger.LogError("Error in method CreateProductAsync at time: {DateTimeNow}, error: {EMessage}",
                DateTime.Now, e.Message);
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<string>> UpdateProductAsync(UpdateProductDto product)
    {
        try
        {
            logger.LogInformation("Starting method UpdateProductAsync in time: {DateTimeNow}", DateTime.Now);
            var existingProduct = await context.Products.Where(x => x.Id == product.Id)
                .ExecuteUpdateAsync(x => x
                    .SetProperty(c => c.Name, product.Name)
                    .SetProperty(c => c.Description, product.Description)
                    .SetProperty(c => c.Status, product.Status));
            if (existingProduct == 0)
            {
                logger.LogWarning("Not found product with Id={Id}, at time {DateTimeNow}", product.Id, DateTime.Now);
                return new Response<string>(HttpStatusCode.BadRequest, $"Not found product with Id={product.Id}");
            }

            logger.LogInformation("Finished method UpdateProductAsync in time: {DateTimeNow}", DateTime.Now);
            return new Response<string>("Successfully updated product");
        }
        catch (Exception e)
        {
            logger.LogError("Error in method UpdateProductAsync at time: {DateTimeNow}, error: {EMessage}",
                DateTime.Now, e.Message);
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<bool>> DeleteProductAsync(int productId)
    {
        try
        {
            logger.LogInformation("Starting method DeleteProductAsync in time: {DateTimeNow}", DateTime.Now);
            var existingProduct = await context.Products.Where(x => x.Id == productId).ExecuteDeleteAsync();

            if (existingProduct == 0)
            {
                logger.LogWarning("Not found product with Id={Id}, at time {DateTimeNow}", productId, DateTime.Now);
                return new Response<bool>(HttpStatusCode.BadRequest, "Product not found");
            }

            logger.LogInformation("Finished method DeleteProductAsync in time: {DateTimeNow}", DateTime.Now);
            return new Response<bool>(true);
        }
        catch (Exception e)
        {
            logger.LogError("Error in method DeleteProductAsync at time: {DateTimeNow}, error: {EMessage}",
                DateTime.Now, e.Message);
            return new Response<bool>(HttpStatusCode.InternalServerError, e.Message);
        }
    }
}