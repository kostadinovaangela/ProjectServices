using System.ComponentModel.DataAnnotations;
using AutoMapper;
using InternetService.DAL.Context.Repositories;
using InternetService.DAL.Models.Dtos;
using Microsoft.Extensions.Logging;

namespace InternetService.Core.Services.Category
{
    public class CategoryService : ICategoryService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryService> _logger;
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(IMapper mapper, ILogger<CategoryService> logger, ICategoryRepository categoryRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _categoryRepository = categoryRepository;
        }

        public async Task<List<DAL.Models.Category>> GetAllAsync()
        {
            return await _categoryRepository.GetAll();
        }

        public async Task<DAL.Models.Category?> GetById(int id)
        {
            return await _categoryRepository.GetById(id);
        }

        public async Task<(bool isSaved, DAL.Models.Category newCategory)> Create(CategoryDto categoryDto)
        {
            if (string.IsNullOrEmpty(categoryDto.Name))
            {
                throw new ValidationException(Constants.ErrorMessages.NameRequired);
            }

            var category = _mapper.Map<DAL.Models.Category>(categoryDto);
            try
            {
                return await _categoryRepository.Create(category);
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to create - {e}");
                return (false, null);
            }
        }

        public async Task<bool> Update(CategoryDto categoryDto)
        {
            var existingCategory = await _categoryRepository.GetById((int)categoryDto.Id);
            if (existingCategory == null)
            {
                throw new Exception(Constants.ErrorMessages.CategoryDoesNotExists);
            }

            _mapper.Map(categoryDto, existingCategory);
            try
            {
                return await _categoryRepository.Save();
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to update - {e}");
                return false;
            }
        }

        public async Task<bool> Delete(int id)
        {
            var existingCategory = await _categoryRepository.GetById(id);
            if (existingCategory == null)
            {
                throw new Exception(Constants.ErrorMessages.CategoryDoesNotExists);
            }

            try
            {
                return await _categoryRepository.Delete(existingCategory);
            }
            catch (Exception e)
            {
                _logger.LogError($"Failed to update - {e}");

                return false;
            }
        }

        public async Task<List<CategoryProductsDto>> GetCategoryWithProducts(int id)
        {
            return await _categoryRepository.GetCategoryWithProducts(id);
        }

        public async Task<DAL.Models.Category?> GetCategoryByName(string name)
        {
            return await _categoryRepository.GetByName(name);
        }
    }
}
