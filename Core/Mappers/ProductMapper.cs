using AutoMapper;
using InternetService.DAL.Models;
using InternetService.DAL.Models.Dtos;

namespace InternetService.Core
{
    public class ProductMapper : Profile
    {
        public ProductMapper()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<ProductDto, Product>();
        }
    }
}
