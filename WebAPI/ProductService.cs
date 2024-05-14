using InternetService.DAL.Models;
using InternetService.DAL.Models.Dtos;
using Newtonsoft.Json;
namespace InternetService.WebAPI
{
    public class ProductService
    {
        public List<Product> ReadProductsFromJson(string jsonFilePath)
        {
            string jsonData = File.ReadAllText(jsonFilePath);
            List<Product> products = JsonConvert.DeserializeObject<List<Product>>(jsonData);
            return products;
        }
    }

}
