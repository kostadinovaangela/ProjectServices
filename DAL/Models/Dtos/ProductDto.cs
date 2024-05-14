using System.ComponentModel.DataAnnotations.Schema;

namespace InternetService.DAL.Models.Dtos
{
    public class ProductDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public List<int> Categories {get; set; }

    }
}
