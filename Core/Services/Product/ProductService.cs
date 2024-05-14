using AutoMapper;
using InternetService.Core.Services.Category;
using System.Transactions;
using InternetService.DAL.Context.Repositories;
using InternetService.DAL.Models;
using InternetService.DAL.Models.Dtos;
using Microsoft.Extensions.Logging;

namespace InternetService.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<ProductService> _logger;
        private readonly ICategoryService _categoryService;
        private readonly IProductRepository _productRepository;
        private readonly IProductCategoryService _productCategoryService;

        public ProductService(IMapper mapper, ILogger<ProductService> logger, ICategoryService categoryService, IProductRepository productRepository, IProductCategoryService productCategoryService)
        {
            _mapper = mapper;
            _logger = logger;
            _categoryService = categoryService;
            _productRepository = productRepository;
            _productCategoryService = productCategoryService;
        }


        public async Task<List<Product>> GetAllAsync()
        {
            return await _productRepository.GetAll();
        }

        public async Task<Product?> GetById(int id)
        {
            return await _productRepository.GetById(id);
        }

        public async Task<bool> Create(ProductDto productDto)
        {
            productDto.ValidateRequiredFields();

            var product = _mapper.Map<Product>(productDto);
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var newProduct = (await _productRepository.Create(product));
                if (newProduct.isSaved)
                {
                    foreach (var caregoryId in productDto.Categories)
                    {
                        var category = await _categoryService.GetById(caregoryId);
                        if (category != null)
                        {
                            await _productCategoryService.Create(new Product_Category
                            {
                                CategoryId = category.Id,
                                ProductId = newProduct.newProduct.Id,
                                StockQuantity = Constants.DefaultStockQuantity
                            });
                        }

                    }
                }
                scope.Complete();
            }
            catch (Exception e)
            {
                scope.Dispose();
                _logger.LogError($"Failed to create - {e}");
                return false;
            }
            return true;
        }

        public async Task<bool> Update(ProductDto productDto)
        {
            productDto.ValidateRequiredFields();

            var existingProduct = await _productRepository.GetById((int)productDto.Id);
            if (existingProduct == null)
            {
                throw new Exception(Constants.ErrorMessages.ProductDoesNotExist);
            }

            _mapper.Map(productDto, existingProduct);

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                foreach (var categoryId in productDto.Categories)
                {
                    if ((await _categoryService.GetById(categoryId)) != null)
                    {
                        if ((await _productCategoryService.GetByCategoryAndProduct(categoryId, (int)productDto.Id)) == null)
                        {
                            await _productCategoryService.Create(new Product_Category
                            {
                                CategoryId = categoryId,
                                ProductId = existingProduct.Id,
                                StockQuantity = Constants.DefaultStockQuantity
                            });
                        }
                    }
                }

                if (await _productRepository.Save())
                {
                    scope.Complete();
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to update - {e}");
                scope.Dispose();
            }

            return false;
        }

        public async Task<bool> Delete(int id)
        {
            var existingProduct = await _productRepository.GetById(id);
            if (existingProduct == null)
            {
                throw new Exception(Constants.ErrorMessages.ProductDoesNotExist);
            }

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                await _productRepository.Delete(existingProduct);
                await _productCategoryService.DeleteBy(productId: existingProduct.Id);
                scope.Complete();

                return true;
            }
            catch (Exception e)
            {
                scope.Complete();
                _logger.LogError($"Failed to update - {e}");
                return false;
            }
        }

        public async Task<bool> Import(List<ProductImportDataDto> data)
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                foreach (var importedProduct in data)
                {
                    var newProduct = await _productRepository.Create(new Product
                    {
                        Description = $"Description - {importedProduct.Name}",
                        Name = importedProduct.Name,
                        Price = importedProduct.Price
                    });

                    foreach (var categoryName in importedProduct.Categories)
                    {
                        var category = await _categoryService.GetCategoryByName(categoryName);
                        if (category == null)
                        {
                            var categoryDto = new CategoryDto()
                            {
                                Name = categoryName,
                            };

                            var result = await _categoryService.Create(categoryDto);
                            if (result.isSaved)
                            {
                                category = result.newCategory;
                            }
                        }

                        if (newProduct.isSaved)
                        {
                            var productCategory = new Product_Category
                            {
                                ProductId = newProduct.newProduct.Id,
                                CategoryId = category.Id,
                                StockQuantity = importedProduct.Quantity,
                            };

                            await _productCategoryService.Create(productCategory);
                        }
                    }
                }
                scope.Complete();

            }
            catch (Exception e)
            {
                scope.Dispose();
                _logger.LogError("Something went wrong");
                return false;
            }

            return true;
        }
    }
}
