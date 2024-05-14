using InternetService.Core.Services;
using InternetService.DAL.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace InternetService.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public ShoppingCartController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost]
        [Route("CalculateShoppingCartTotal")]
        public async Task<ActionResult<decimal>> Calculate([FromBody] ShoppingCartDto shoppingCart)
        {
            return Ok(await _transactionService.CalculateTotalAmount(shoppingCart));
        }
    }
}
