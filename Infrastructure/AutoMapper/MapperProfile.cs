using AutoMapper;
using Domain.DTOs.ProductDTOs;
using Domain.Entities;

namespace Infrastructure.AutoMapper;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Product, GetProductDto>()
            .ForMember(x => x.Status, opt
                => opt.MapFrom(x => x.Status.ToString())).ReverseMap();
        CreateMap<Product, CreateProductDto>().ReverseMap();
        CreateMap<Product, UpdateProductDto>().ReverseMap();
    }
}