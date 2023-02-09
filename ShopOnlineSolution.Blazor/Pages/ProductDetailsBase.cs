using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;
using ShopOnlineSolution.Blazor.Services.Contracts;

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

        public ProductDto Product { get; set; }

        public string ErrorMessage { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                Product = await ProductService.GetItem(Id);
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
                NavigationManager.NavigateTo("/ShoppingCart");
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
