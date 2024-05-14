using InternetService.DAL.Models.Dtos;

namespace InternetService.DAL.Context.Repositories
{
    public interface ICategoryRepository
    {
        Task<List<Models.Category>> GetAll();
        Task<Models.Category?> GetById(int id);
        Task<(bool isSaved, Models.Category newCategory)> Create(Models.Category category);
        Task<bool> Save();
        Task<bool> Delete(Models.Category category);
        Task<List<CategoryProductsDto>> GetCategoryWithProducts(int id);
        Task<Models.Category?> GetByName(string name);
    }
}
