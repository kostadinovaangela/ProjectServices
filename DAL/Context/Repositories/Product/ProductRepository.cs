using Microsoft.EntityFrameworkCore;

namespace InternetService.DAL.Context.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _dbContext;

        public ProductRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Models.Product>> GetAll()
        {
            return await _dbContext.Products.ToListAsync();
        }

        public async Task<Models.Product?> GetById(int id)
        {
           return await _dbContext.Products.FindAsync(id);
        }

        public async Task<(bool, Models.Product)> Create(Models.Product product)
        {
            var result = (await _dbContext.Products.AddAsync(product)).Entity;
            return (await Save(), result);
        }

        public async Task<bool> Save()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> Delete(Models.Product product)
        {
             _dbContext.Products.Remove(product);
             return await Save();
        }
    }
}
