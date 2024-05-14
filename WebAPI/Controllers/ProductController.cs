using InternetService.Core.Services;
using InternetService.DAL.Models;
using InternetService.DAL.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace InternetService.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return Ok(await _productService.GetAllAsync());
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            if (id < 1)
            {
                return BadRequest("Please provide correct value");
            }
            var product = await _productService.GetById(id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost("Create")]
        public async Task<ActionResult<Product>> Create(ProductDto productDto)
        {
            return Ok(await _productService.Create(productDto));
        }

        [HttpPost("/Import")]
        public async Task<ActionResult<Product>> Import([FromBody] List<ProductImportDataDto> data)
        {
            return Ok(await _productService.Import(data));
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(ProductDto productDto)
        {
            if (productDto.Id < 1)
            {
                return BadRequest("Please provide correct value");
            }

            if (!await _productService.Update(productDto))
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

            if (!await _productService.Delete(id))
            {
                throw new Exception("Something went wrong");
            }
            return Ok();
        }
        [HttpPost]
        [Route("ImportFromJson")]
        public IActionResult ImportProductsFromJson()
        {

            string jsonFilePath = "C:\\Users\\akost\\source\\repos\\InternetServicesProject\\Products.json";

            var products = ReadProductsFromJson(jsonFilePath);

            return Ok(products);
        }

        private List<Product> ReadProductsFromJson(string jsonFilePath)
        {
            string jsonData = System.IO.File.ReadAllText(jsonFilePath);
            var products = JsonConvert.DeserializeObject<List<Product>>(jsonData);
            return products;
        }
    }

}

