using InternetService.DAL.Models;
using InternetService.DAL.Models.Dtos;

namespace InternetService.DAL.Context.Repositories
{
    public interface IProductCategoryRepository
    {
        Task<bool> Create(Product_Category productCategory);
        Task<Product_Category> GetByCategoryId(int categoryId);
        Task<List<Product_Category>> GetAllByProductId(int productId);
        Task<List<ProductCategoryDto>> GetAllDetailedBy(int categoryId, int productId);
        Task<bool> DeleteBy(int categoryId, int productId);
        Task<Product_Category>Get(int categoryId, int productId);
        Task<bool> Update(Product_Category productCategory);
    }
}
