using Blazored.LocalStorage;
using ShopOnline.Models.Dtos;
using ShopOnlineSolution.Blazor.Services.Contracts;

namespace ShopOnlineSolution.Blazor.Services
{
    public class ManageCartItemsLocalStorageService : IManageCartItemsLocalStorageService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly IShoppingCartService _shoppingCartService;

        const string key = "CartItemCollection";

        public ManageCartItemsLocalStorageService(ILocalStorageService localStorage,
                                                  IShoppingCartService shoppingCartService)
        {
            _localStorage=localStorage;
            _shoppingCartService=shoppingCartService;
        }
        public async Task<List<CartitemDto>> GetCollection()
        {
            return await _localStorage.GetItemAsync<List<CartitemDto>>(key)
                        ?? await AddCollection();
        }

        public async Task RemoveCollection()
        {
            await _localStorage.RemoveItemAsync(key);
        }

        public async Task SaveCollection(List<CartitemDto> cartitemDtos)
        {
            await _localStorage.SetItemAsync(key, cartitemDtos);
        }

        private async Task<List<CartitemDto>> AddCollection()
        {
            var shoppingCartCollection = await _shoppingCartService.GetItems(HardCoded.UserId);
            if (shoppingCartCollection != null)
            {
                await _localStorage.SetItemAsync(key, shoppingCartCollection);
            }
            return shoppingCartCollection;
        }
    }
}
