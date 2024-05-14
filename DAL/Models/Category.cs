namespace InternetService.DAL.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<Product_Category> ProductCategories { get; set; }
    }
}
