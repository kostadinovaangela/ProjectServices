using System.ComponentModel.DataAnnotations.Schema;

namespace InternetService.DAL.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public ICollection<Product_Category> ProductCategories { get; set; }
    }
}
