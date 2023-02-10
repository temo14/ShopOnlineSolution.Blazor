using ShopOnline.Models.Dtos;

namespace ShopOnlineSolution.Blazor.Services.Contracts
{
    public interface IManageCartItemsLocalStorageService
    {
        Task<List<CartitemDto>> GetCollection();
        Task SaveCollection(List<CartitemDto> cartitemDtos);
        Task RemoveCollection();
    }
}
