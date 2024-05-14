using InternetService.DAL.Models;
using InternetService.DAL.Models.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AutoMapper;

namespace InternetService.DAL.Context.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _dbContext;

        public CategoryRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Models.Category>> GetAll()
        {
            return await _dbContext.Categories.ToListAsync();
        }

        public async Task<Models.Category?> GetById(int id)
        {
            return await _dbContext.Categories.FindAsync(id);
        }

        public async Task<(bool isSaved, Models.Category newCategory)> Create(Models.Category category)
        {
            var result = (await _dbContext.Categories.AddAsync(category)).Entity;
            return (await Save(), result);
        }

        public async Task<bool> Save()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> Delete(Models.Category category)
        {
            _dbContext.Categories.Remove(category);
            return await Save();
        }

        public async Task<List<CategoryProductsDto>> GetCategoryWithProducts(int id)
        {
            var categoriesQuery =
                from category in _dbContext.Categories
                join pc in _dbContext.Product_Category
                    on category.Id equals pc.CategoryId into categoryProducts
                where category.Id == id
                select new CategoryProductsDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description,
                    Products = (
                        from productCategory in categoryProducts
                        join product in _dbContext.Products
                            on productCategory.ProductId equals product.Id
                        select new ProductDto
                        {
                            Id = product.Id,
                            Name = product.Name,
                            Description = product.Description,
                            Price = product.Price,
                        }
                    ).ToList(),
                };

            return await categoriesQuery.ToListAsync();
        }

        public async Task<Models.Category?> GetByName(string name)
        {
            var query = from category in _dbContext.Categories
                where category.Name.ToUpper() == name.ToUpper()
                select category;

            return await query.FirstOrDefaultAsync();

        }
    }
}
