namespace InternetService.DAL.Models
{
    public class Product_Category
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int? StockQuantity { get; set; }
    }
}
