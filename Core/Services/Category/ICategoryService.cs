using InternetService.DAL.Models.Dtos;

namespace InternetService.Core.Services
{
    public interface ICategoryService
    {
        Task<List<DAL.Models.Category>> GetAllAsync();
        Task<DAL.Models.Category?> GetById(int id);
        Task<(bool isSaved, DAL.Models.Category newCategory)> Create(CategoryDto categoryDto);
        Task<bool> Update(CategoryDto categoryDto);
        Task<bool> Delete(int id);
        Task<List<CategoryProductsDto>> GetCategoryWithProducts(int id);
        Task<DAL.Models.Category?> GetCategoryByName(string name);
    }
}
