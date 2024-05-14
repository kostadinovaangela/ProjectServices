
using InternetService.DAL.Models.Dtos;

namespace InternetService.Core.Services
{
    public interface IProductService
    {
        Task<List<DAL.Models.Product>> GetAllAsync();
        Task<DAL.Models.Product?> GetById(int id);
        Task<bool> Create(ProductDto categoryDto);
        Task<bool> Update(ProductDto categoryDto);
        Task<bool> Delete(int id);
        Task<bool> Import(List<ProductImportDataDto> json);
    }
}
