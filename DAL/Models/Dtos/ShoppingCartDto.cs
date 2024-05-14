using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetService.DAL.Models.Dtos
{
    public class ShoppingCartDto
    {
        public List<ShoppingCartProductDto> Products { get; set; }
    }
}
