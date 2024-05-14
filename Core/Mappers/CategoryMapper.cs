using AutoMapper;
using InternetService.DAL.Models;
using InternetService.DAL.Models.Dtos;

namespace InternetService.Core
{
    public class CategoryMapper : Profile
    {
        public CategoryMapper()
        {
            CreateMap<Category, CategoryDto>()
                .ForMember(d => d.Description, m => m.MapFrom(s => s.Description))
                .ForMember(d => d.Name, m => m.MapFrom(s => s.Name));

            CreateMap<CategoryDto, Category>()
                .ForMember(d => d.Description, m => m.MapFrom(s => string.IsNullOrEmpty(s.Description) ? $"Description - {s.Name}" : s.Description))
                .ForMember(d => d.Name, m => m.MapFrom(s => s.Name))
                .ForMember(d => d.Id, m => m.MapFrom(s => s.Id));
        }
    }
}
