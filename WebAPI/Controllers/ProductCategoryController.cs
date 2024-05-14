using InternetService.Core;
using InternetService.Core.Services;
using InternetService.DAL.Models.Dtos;
using InternetService.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace InternetService.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategoryController : ControllerBase
    {
        private readonly IProductCategoryService _productCategoryService;

        public ProductCategoryController(IProductCategoryService productCategoryService)
        {
            _productCategoryService = productCategoryService;
        }

        [HttpPost("UpdateStock")]
        public async Task<ActionResult> UpdateStock(int categoryId, int productId, int quantity)
        {
            if (categoryId < 0)
            {
                return BadRequest(Constants.ErrorMessages.CategoryDoesNotExists);
            }

            if (productId < 0)
            {
                return BadRequest(Constants.ErrorMessages.ProductDoesNotExist);
            }

            return Ok(await _productCategoryService.UpdateStock(categoryId, productId, quantity));
        }

        [HttpPost("ChangeProductCategory")]
        public async Task<ActionResult> ChangeProductCategory(int productId, int oldCategory, int newCategory)
        {
            return Ok(await _productCategoryService.ChangeProductWithNewCategory(productId, oldCategory, newCategory));
        }

        [HttpGet("GetAllDetailed")]
        public async Task<ActionResult<List<ProductCategoryDto>>> GetAll()
        {
            return Ok(await _productCategoryService.GetAllDetailedBy());
        }

    }
}
