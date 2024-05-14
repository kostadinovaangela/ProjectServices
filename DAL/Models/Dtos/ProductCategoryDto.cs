namespace InternetService.DAL.Models.Dtos
{
    public class ProductCategoryDto
    {
        public ProductDto Product { get; set; }
        public CategoryDto Category { get; set; }
        public int StockQuantity { get; set; }
    }
}
