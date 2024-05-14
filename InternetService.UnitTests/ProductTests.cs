using InternetService.Core.Services;
using InternetService.DAL.Context.Repositories;
using InternetService.DAL.Models;
using InternetService.DAL.Models.Dtos;
using Moq;
using System.ComponentModel.DataAnnotations;
using InternetService.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using AutoMapper;
using InternetService.Core;
using Microsoft.Extensions.Logging;

namespace InternetService.UnitTests
{
    public class ProductTests
    {
        [Test]
        public void Create_Should_Fail_Name_Is_Null()
        {
            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<ProductService>>();
            var categoryServiceMock = new Mock<ICategoryService>();
            var productRepositoryMock = new Mock<IProductRepository>();
            var productCategoryServiceMock = new Mock<IProductCategoryService>();


            var productService = new ProductService(
                mapperMock.Object,
                loggerMock.Object,
                categoryServiceMock.Object,
                productRepositoryMock.Object,
                productCategoryServiceMock.Object
            );

            var newProduct = new ProductDto
            {
                Description = "Test product",
                Price = 10.99m
            };

            var message = Assert.ThrowsAsync<ValidationException>(() => productService.Create(newProduct));
            Assert.AreEqual(Constants.ErrorMessages.NameRequired, message.Message);

        }

        [Test]
        public void Create_Should_Fail_Price_Is_Negative()
        {
            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<ProductService>>();
            var categoryServiceMock = new Mock<ICategoryService>();
            var productRepositoryMock = new Mock<IProductRepository>();
            var productCategoryServiceMock = new Mock<IProductCategoryService>();


            var productService = new ProductService(
                mapperMock.Object,
                loggerMock.Object,
                categoryServiceMock.Object,
                productRepositoryMock.Object,
                productCategoryServiceMock.Object
            );

            var newProduct = new ProductDto
            {
                Name = "test",
                Description = "Test product",
                Price = -2
            };

            var message = Assert.ThrowsAsync<ValidationException>(() => productService.Create(newProduct));
            Assert.AreEqual(Constants.ErrorMessages.PriceIsNegativeNumber, message.Message);

        }

        [Test]
        public async Task Create_Should_Success()
        {
            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<ProductService>>();
            var categoryServiceMock = new Mock<ICategoryService>();
            var productRepositoryMock = new Mock<IProductRepository>();
            var productCategoryServiceMock = new Mock<IProductCategoryService>();


            var productService = new ProductService(
                mapperMock.Object,
                loggerMock.Object,
                categoryServiceMock.Object,
                productRepositoryMock.Object,
                productCategoryServiceMock.Object
            );

            var newProduct = new ProductDto
            {
                Name = "test",
                Description = "Test product",
                Price = 2,
                Categories = new List<int>()
            };


            var mappedProduct = new Product()
            {
                Name = "test",
                Description = "Test product",
                Price = 2
            };

            mapperMock.Setup(mapper => mapper.Map<Product>(newProduct)).Returns(mappedProduct);

            mappedProduct.Id = 1;

       
            productRepositoryMock.Setup(f => f.Create(mappedProduct)).ReturnsAsync((true, mappedProduct));

            var result = await  productService.Create(newProduct);
            Assert.True(result);
        }
    }
}