using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ShopOnline.Models.Dtos;
using ShopOnlineSolution.Blazor.Services.Contracts;
using System.Security.Principal;

namespace ShopOnlineSolution.Blazor.Pages
{
    public class ShoppingCartBase: ComponentBase
    {
        [Inject]
        public IJSRuntime Js { get; set; }

        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }
        public List<CartitemDto> ShoppingCartItems { get; set; }
        public string ErrorMessage { get; set; }
        protected string TotalPrice { get; set; }
        protected int TotalQuantity { get; set; }


        protected override async Task OnInitializedAsync()
        {
            
            try
            {
                ShoppingCartItems = await ShoppingCartService.GetItems(HardCoded.UserId);
                CartChanged();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }
        protected async Task DeleteCartItem_Click(int id)
        {
            var cartItemDto = await ShoppingCartService.DeleteItem(id);
            if (cartItemDto != null)
            {
                RemoveCartItem(id);
                UpdateItemTotalPrice(cartItemDto);
                CartChanged();
            }
        }

        protected async Task UpdateQtyCartItem_Click(int id, int qty)
        {
            try
            {
                if (qty>0)
                {
                    var updateItemDto = new CartItemQtyUpdateDto
                    {
                        CartItemId= id,
                        Qty = qty
                    };

                    var returnedUpdateItemDto = await ShoppingCartService.UpdateItem(updateItemDto);

                    UpdateItemTotalPrice(returnedUpdateItemDto);

                    CartChanged();

                    await Js.InvokeVoidAsync("MakeUpdateQtyButtonVisible", id, false);
                }
                else
                {
                    var item = ShoppingCartItems.FirstOrDefault(i => i.Id == id);

                    if (item != null)
                    {
                        item.Qty=1;
                        item.TotalPrice=item.Price;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        protected async Task UpdateQty_Input(int id)
        {
            await Js.InvokeVoidAsync("MakeUpdateQtyButtonVisible", id, true);
        }

        private void UpdateItemTotalPrice(CartitemDto cartitemDto)
        {
            var cartItemDto = ShoppingCartItems.FirstOrDefault(x => x.Id == cartitemDto.Id);
            if (cartitemDto !=null)
            {
                cartItemDto.TotalPrice = cartitemDto.Price* cartItemDto.Qty;
            }
        }
        private void CalculateCartsummaryTotals()
        {
            SetTotalPrice();
            SetTotalQuantity();
        }

        private void SetTotalPrice()
        {
            TotalPrice = ShoppingCartItems.Sum(x => x.TotalPrice).ToString("C");
        }       
        private void SetTotalQuantity()
        {
            TotalQuantity = ShoppingCartItems.Sum(x => x.Qty);
        }

        private void RemoveCartItem(int id)
        {
            var cartItemDto = ShoppingCartItems.FirstOrDefault(x => x.Id == id);
            ShoppingCartItems.Remove(cartItemDto);
        }

        private void CartChanged()
        {
            CalculateCartsummaryTotals();
            ShoppingCartService.RaiseEventShoppingCartChanged(TotalQuantity);
        }
    }
}
