using ShopOnline.Models.Dtos;

namespace ShopOnlineSolution.Blazor.Services.Contracts
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetItems();
        Task<ProductDto> GetItem(int id);
    }
}
