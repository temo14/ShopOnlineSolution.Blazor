using Blazored.LocalStorage;
using ShopOnline.Models.Dtos;
using ShopOnlineSolution.Blazor.Services.Contracts;

namespace ShopOnlineSolution.Blazor.Services
{
    public class ManageProductsLocalStorageService : IManageProductsLocalStorageService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly IProductService _productService;
        private const string key = "ProductCollection";
        public ManageProductsLocalStorageService(ILocalStorageService localStorage,
                                                    IProductService productService)
        {
            _localStorage=localStorage;
            _productService=productService;
        }
        public async Task<IEnumerable<ProductDto>> GetCollection()
        {
            return await _localStorage.GetItemAsync<IEnumerable<ProductDto>>(key)
                     ?? await AddCollection();               
        }

        public async Task RemoveCollection()
        {
            await _localStorage.RemoveItemAsync(key);
        }
        private async Task<IEnumerable<ProductDto>> AddCollection()
        {
            var productCollection = await _productService.GetItems();

            if (productCollection != null)
            {
                await _localStorage.SetItemAsync(key, productCollection);
            }

            return productCollection;
        }
    }
}
