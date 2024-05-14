using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternetService.DAL.Models.Dtos;

namespace InternetService.Core.Services
{
    public interface ITransactionService
    {
        Task<decimal> CalculateTotalAmount(ShoppingCartDto shoppingCart);
    }
}
