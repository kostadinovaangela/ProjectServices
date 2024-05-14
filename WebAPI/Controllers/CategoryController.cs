using InternetService.Core.Services;
using InternetService.DAL.Models;
using InternetService.DAL.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace InternetService.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            return Ok(await _categoryService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            if (id < 1)
            {
                return BadRequest("Please provide correct value");
            }
            var category = await _categoryService.GetById(id);
            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpGet("GetCategoryWithProducts/{id}")]
        public async Task<ActionResult<CategoryProductsDto>> GetCategoryWithProducts(int id)
        {
            if (id < 1)
            {
                return BadRequest("Please provide correct value");
            }
            var category = await _categoryService.GetCategoryWithProducts(id);
            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpPost("Create")]
        public async Task<ActionResult<bool>> Create(CategoryDto categoryDto)
        {
            return Ok((await _categoryService.Create(categoryDto)).isSaved);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(CategoryDto categoryDto)
        {
            if (categoryDto.Id < 1)
            {
                return BadRequest("Please provide correct value");
            }

            if (!await _categoryService.Update(categoryDto))
            {
                throw new Exception("Something went wrong");
            }
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id < 1)
            {
                return BadRequest("Please provide correct value");
            }

            if (!await _categoryService.Delete(id))
            {
                throw new Exception("Something went wrong");
            }
            return Ok();
        }
    }
}
