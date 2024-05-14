using InternetService.DAL.Models;
using InternetService.DAL.Models.Dtos;

namespace InternetService.Core.Services
{
    public interface IProductCategoryService
    {
        Task<bool> Create(Product_Category productCategory);
        Task<Product_Category> GetByCategoryAndProduct(int categoryId, int productId);
        Task<List<ProductCategoryDto>>GetAllDetailedBy(int categoryId = 0, int productId = 0);
        Task<bool> DeleteBy(int categoryId = 0, int productId = 0);
        Task<bool> UpdateStock(int categoryId, int productId, int quantity);
        Task<bool> ChangeProductWithNewCategory(int productId, int oldCategory, int newCategory);
    }
}
