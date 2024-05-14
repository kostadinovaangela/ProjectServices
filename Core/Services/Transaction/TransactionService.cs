using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using InternetService.DAL.Models;
using InternetService.DAL.Models.Dtos;
using Microsoft.Extensions.Logging;

namespace InternetService.Core.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IProductCategoryService _productCategoryService;
        private readonly ILogger<TransactionService> _logger;

        public TransactionService(IProductCategoryService productCategoryService, ILogger<TransactionService> logger)
        {
            _productCategoryService = productCategoryService;
            _logger = logger;
        }

        public async Task<decimal> CalculateTotalAmount(ShoppingCartDto shoppingCart)
        {
            decimal totalAmount = 0;
            var processedProductCategories = new List<(int categoryId, int productId)>();

            var productsFromSameCategory = shoppingCart.Products
                .GroupBy(f => f.CategoryId)
                .Where(g => g.Count() > 1)
                .Select(g => new
                {
                    CategoryId = g.Key,
                    Products = g.Select(f => new
                    {
                        ProductId = f.ProductId,
                        Quantity = f.Quantity

                    }).ToList(),

                });

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                // handle products from the same category and apply to them 5% discount
                foreach (var categoryGrouped in productsFromSameCategory)
                {
                    foreach (var tuple in categoryGrouped.Products)
                    {
                        var productCategoryDetails = (await _productCategoryService.GetAllDetailedBy(categoryGrouped.CategoryId,
                            tuple.ProductId)).FirstOrDefault();

                        if (productCategoryDetails != null && productCategoryDetails.StockQuantity > tuple.Quantity)
                        {
                            totalAmount += tuple.Quantity * (productCategoryDetails.Product.Price * 0.95m);

                            await _productCategoryService.UpdateStock(categoryGrouped.CategoryId, tuple.ProductId, productCategoryDetails.StockQuantity -= tuple.Quantity);

                            processedProductCategories.Add((categoryGrouped.CategoryId, tuple.ProductId));
                        }
                        else
                        {
                            throw new Exception($"{Constants.ErrorMessages.OutOfStock} {productCategoryDetails?.Product?.Name}");
                        }
                    }
                }

                var products = new List<ShoppingCartProductDto>();
                if (processedProductCategories.Any())
                {

                    products = shoppingCart.Products.Where(f => processedProductCategories.Any(proccessed =>
                        proccessed.categoryId != f.CategoryId && proccessed.productId != f.ProductId)).ToList();
                }
                else
                {
                    products = shoppingCart.Products;
                }


                //handle rest of products from different categories
                foreach (var shoppingCartProduct in products)
                {
                    var productCategoryDetails = (await _productCategoryService.GetAllDetailedBy(shoppingCartProduct.CategoryId,
                        shoppingCartProduct.ProductId)).FirstOrDefault();

                    if (productCategoryDetails != null && productCategoryDetails.StockQuantity > shoppingCartProduct.Quantity)
                    {

                        //handle single quantity
                        if (shoppingCartProduct.Quantity == 1)
                        {
                            totalAmount += productCategoryDetails.Product.Price;
                        }
                        // handle multi quantity and give on first item discount of 5%
                        else if (shoppingCartProduct.Quantity > 1)
                        {
                            var discountAmount = productCategoryDetails.Product.Price * 0.95m;
                            totalAmount += discountAmount + ((shoppingCartProduct.Quantity - 1) * productCategoryDetails.Product.Price);
                        }

                        await _productCategoryService.UpdateStock(shoppingCartProduct.CategoryId, shoppingCartProduct.ProductId,
                            productCategoryDetails.StockQuantity -= shoppingCartProduct.Quantity);
                    }
                    else
                    {
                        throw new Exception($"{Constants.ErrorMessages.OutOfStock} {productCategoryDetails?.Product?.Name}");
                    }
                }

                scope.Complete();
            }
            catch (Exception e)
            {
                _logger.LogError($"Calculation error {e.Message}");
            }

            return totalAmount;
        }
    }
}
