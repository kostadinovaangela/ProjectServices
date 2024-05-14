namespace InternetService.DAL.Models.Dtos
{
    public class ProductImportDataDto
    {
        public string Name { get; set; }
        public List<string> Categories { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
