using InternetService.DAL.Models;

namespace InternetService.DAL.Context.Repositories
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAll();
        Task<Product?> GetById(int id);
        Task<(bool isSaved, Product newProduct)> Create(Product product);
        Task<bool> Save();
        Task<bool> Delete(Product product);
    }
}
