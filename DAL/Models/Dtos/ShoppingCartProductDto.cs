using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetService.DAL.Models.Dtos
{
    public class ShoppingCartProductDto
    {
        public int ProductId { get; set; }

        public int CategoryId { get; set; }

        public int Quantity { get; set; }

    }
}
