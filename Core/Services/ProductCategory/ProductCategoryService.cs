using InternetService.DAL.Context.Repositories;
using InternetService.DAL.Models;
using InternetService.DAL.Models.Dtos;

namespace InternetService.Core.Services
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductCategoryService(IProductCategoryRepository productCategoryRepository, ICategoryRepository categoryRepository)
        {
            _productCategoryRepository = productCategoryRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<bool> Create(Product_Category productCategory)
        {
            return await _productCategoryRepository.Create(productCategory);
        }

        public async Task<Product_Category> GetByCategoryAndProduct(int categoryId, int productId)
        {
            return await _productCategoryRepository.Get(categoryId, productId);
        }

        public async Task<List<ProductCategoryDto>> GetAllDetailedBy(int categoryId = 0, int productId = 0)
        {
            return await _productCategoryRepository.GetAllDetailedBy(categoryId, productId);
        }

        public async Task<bool> DeleteBy(int categoryId = 0, int productId = 0)
        {
            return await _productCategoryRepository.DeleteBy(categoryId, productId);
        }

        public async Task<bool> UpdateStock(int categoryId, int productId, int quantity)
        {
            var productCategory = await _productCategoryRepository.Get(categoryId, productId);
            if (productCategory != null)
            {
                productCategory.StockQuantity = quantity;

                return await _productCategoryRepository.Update(productCategory);
            }

            return false;
        }

        public async Task<bool> ChangeProductWithNewCategory(int productId, int oldCategory, int newCategory)
        {
            if (await _categoryRepository.GetById(newCategory) == null)
            {
                return false;
            }

            var product = await _productCategoryRepository.Get(oldCategory, productId);
            if (product != null)
            {
                var quantity = product.StockQuantity;

                await _productCategoryRepository.DeleteBy(product.CategoryId, product.ProductId);

                await _productCategoryRepository.Create(new Product_Category
                {
                    CategoryId = newCategory,
                    ProductId = productId,
                    StockQuantity = quantity
                });

                return true;
            }
            
            return false;
        }
    }
}
