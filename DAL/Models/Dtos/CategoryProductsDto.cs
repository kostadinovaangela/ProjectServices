namespace InternetService.DAL.Models.Dtos
{
    public class CategoryProductsDto
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<ProductDto> Products { get; set; }
    }
}
