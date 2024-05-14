using InternetService.DAL.Models;
using InternetService.DAL.Models.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AutoMapper;

namespace InternetService.DAL.Context.Repositories
{
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public ProductCategoryRepository(AppDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<bool> Create(Product_Category productCategory)
        {
            await _dbContext.Product_Category.AddAsync(productCategory);
            return await Save();

        }

        public async Task<Product_Category> GetByCategoryId(int categoryId)
        {
            var query = from pc in _dbContext.Product_Category
                where pc.CategoryId == categoryId
                select pc;

            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<Product_Category>> GetAllByProductId(int productId)
        {
            var query = from pc in _dbContext.Product_Category
                where pc.ProductId == productId
                select pc;

            return await query.ToListAsync();
        }

        public async Task<List<ProductCategoryDto>> GetAllDetailedBy(int categoryId, int productId)
        {
            var query = from pc in _dbContext.Product_Category
                join product in _dbContext.Products
                    on pc.ProductId equals product.Id into productGroup
                from productLeftJoin in productGroup.DefaultIfEmpty()
                join category in _dbContext.Categories
                    on pc.CategoryId equals category.Id into categoryGroup
                from categoryLeftJoin in categoryGroup.DefaultIfEmpty()
                where (categoryId == 0 || pc.CategoryId == categoryId) &&
                      (productId == 0 || pc.ProductId == productId)
                select new ProductCategoryDto
                {
                    Product = _mapper.Map<Models.Product, ProductDto>(productLeftJoin),
                    Category = _mapper.Map<Models.Category, CategoryDto>(categoryLeftJoin),
                    StockQuantity = (int)pc.StockQuantity
                };

            return await query.ToListAsync();
        }

        public async Task<bool> DeleteBy(int categoryId, int productId)
        {
            var query = _dbContext.Product_Category.AsQueryable();
            
            if (categoryId != 0)
            {
                query = query.Where(pc => pc.CategoryId == categoryId);
            }

            if (productId != 0)
            {
                query = query.Where(pc => pc.ProductId == productId);
            }
            
            _dbContext.Product_Category.RemoveRange(await query.ToListAsync());

            return await Save();
        }

        public async Task<Product_Category> Get(int categoryId, int productId)
        {
            var query = from pc in _dbContext.Product_Category
                where pc.CategoryId == categoryId && pc.ProductId == productId
                select pc;

            return await query.FirstOrDefaultAsync();
        }

        public async Task<bool> Update(Product_Category productCategory)
        {
            _dbContext.Update(productCategory);
            return await Save();
        }

        public async Task<bool> Save()
        {
            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
