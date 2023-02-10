using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;
using ShopOnlineSolution.Blazor.Services.Contracts;
using System.Runtime.CompilerServices;

namespace ShopOnlineSolution.Blazor.Pages
{
    public class ProductDetailsBase : ComponentBase
    {
        [Parameter]
        public int Id { get; set; }

        [Inject]
        public IProductService ProductService { get; set; }

        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public IManageProductsLocalStorageService ManageProductsLocalStorageService { get; set; }
        [Inject]
        public IManageCartItemsLocalStorageService ManageCartItemsLocalStorageService { get; set; }

        public ProductDto Product { get; set; }

        public string ErrorMessage { get; set; }

        private List<CartitemDto> ShoppingCartItems { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                ShoppingCartItems = await ManageCartItemsLocalStorageService.GetCollection();
                Product = await GetProductById(Id);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                throw;
            }
        }
        protected async Task AddToCart_Click(CartItemToAddDto cartItemToAdd)
        {
            try
            {
                var carItemDto = await ShoppingCartService.AddItem(cartItemToAdd);

                if (carItemDto != null)
                {
                    ShoppingCartItems.Add(carItemDto);
                    await ManageCartItemsLocalStorageService.SaveCollection(ShoppingCartItems);
                }

                NavigationManager.NavigateTo("/ShoppingCart");
            }
            catch (Exception)
            {

                throw;
            }
        }
        private async Task<ProductDto> GetProductById(int id)
        {
            var productDto = await ManageProductsLocalStorageService.GetCollection();
            if (productDto != null)
            {
                return productDto.SingleOrDefault(x => x.Id==id);
            }
            return null;
        }
    }
}
