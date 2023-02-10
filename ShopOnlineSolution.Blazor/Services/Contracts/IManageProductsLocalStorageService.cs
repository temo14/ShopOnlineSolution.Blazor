using ShopOnline.Models.Dtos;

namespace ShopOnlineSolution.Blazor.Services.Contracts
{
    public interface IManageProductsLocalStorageService
    {
        Task<IEnumerable<ProductDto>> GetCollection();
        Task RemoveCollection();
    }
}
