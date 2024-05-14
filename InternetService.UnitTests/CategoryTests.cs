using AutoMapper;
using InternetService.DAL.Context.Repositories;
using InternetService.DAL.Models.Dtos;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternetService.Core;
using InternetService.Core.Services.Category;
using InternetService.DAL.Models;

namespace InternetService.UnitTests
{
    public class CategoryTests
    {
        [Test]
        public void Create_Should_Fail_Name_Is_Null()
        {
            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<CategoryService>>();
            var categoryRepositoryMock = new Mock<ICategoryRepository>();

            var categoryService = new CategoryService(
                mapperMock.Object,
                loggerMock.Object,
                categoryRepositoryMock.Object
            );

            var newCategory = new CategoryDto()
            {
                Description = "test"
            };

            var message = Assert.ThrowsAsync<ValidationException>(() => categoryService.Create(newCategory));
            Assert.AreEqual(Constants.ErrorMessages.NameRequired, message.Message);

        }

        [Test]
        public void Delete_Should_Fail_Category_Does_Not_Exists()
        {
            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<CategoryService>>();
            var categoryRepositoryMock = new Mock<ICategoryRepository>();

            var categoryService = new CategoryService(
                mapperMock.Object,
                loggerMock.Object,
                categoryRepositoryMock.Object
            );

            var notValidId = 2;

            categoryRepositoryMock.Setup(r => r.GetById(notValidId)).ReturnsAsync(() => null);

            var message = Assert.ThrowsAsync<Exception>(() => categoryService.Delete(notValidId));
            Assert.AreEqual(Constants.ErrorMessages.CategoryDoesNotExists, message.Message);

        }

        [Test]
        public async Task Delete_Should_Success()
        {
            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<CategoryService>>();
            var categoryRepositoryMock = new Mock<ICategoryRepository>();

            var categoryService = new CategoryService(
                mapperMock.Object,
                loggerMock.Object,
                categoryRepositoryMock.Object
            );

            var fetchedCategory = new Category
            {
                Id = 1
            };

            categoryRepositoryMock.Setup(r => r.GetById(1)).ReturnsAsync(fetchedCategory);
            categoryRepositoryMock.Setup(r => r.Delete(fetchedCategory)).ReturnsAsync(() => true);

            var isDeleted = await categoryService.Delete(1);
            Assert.IsTrue(isDeleted);
        }
    }
}
