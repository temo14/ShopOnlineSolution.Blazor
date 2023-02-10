using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;
using ShopOnlineSolution.Blazor.Services.Contracts;

namespace ShopOnlineSolution.Blazor.Pages
{
    public class ProductBase : ComponentBase
    {
        [Inject]
        public IProductService ProductService { get; set; }

        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }
        [Inject]
        public IManageProductsLocalStorageService ManageProductsLocalStorageService { get; set; }
        [Inject]
        public IManageCartItemsLocalStorageService ManageCartItemsLocalStorageService { get; set; }

        public IEnumerable<ProductDto> Products { get; set;}

        protected override async Task OnInitializedAsync()
        {
            await ClearLocalStorage();

            Products = await ManageProductsLocalStorageService.GetCollection();

            var shoppingCartItems = await ManageCartItemsLocalStorageService.GetCollection();

            var totalQty = shoppingCartItems.Sum(x => x.Qty);

            ShoppingCartService.RaiseEventShoppingCartChanged(totalQty);
        }

        protected IOrderedEnumerable<IGrouping<int, ProductDto>> GetGroupedProductsByCategory()
        {
            return from product in Products
                   group product by product.CategoryId into prodByCatGroup
                   orderby prodByCatGroup.Key
                   select prodByCatGroup;
        }
        protected string GetCategoryName(IGrouping<int, ProductDto> groupedProductsDtos)
        {
            return groupedProductsDtos.FirstOrDefault(x => x.CategoryId == groupedProductsDtos.Key)!.CategoryName;
        }
        private async Task ClearLocalStorage()
        {
            await ManageProductsLocalStorageService.RemoveCollection();
            await ManageCartItemsLocalStorageService.RemoveCollection();
        }
    }
}
