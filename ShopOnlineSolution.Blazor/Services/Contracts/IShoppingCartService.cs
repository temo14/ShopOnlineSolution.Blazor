using ShopOnline.Models.Dtos;

namespace ShopOnlineSolution.Blazor.Services.Contracts
{
    public interface IShoppingCartService
    {
        Task<List<CartitemDto>> GetItems(int userId);
        Task<CartitemDto> AddItem(CartItemToAddDto cartItemToAddDto);
        Task<CartitemDto> DeleteItem(int id);
        Task<CartitemDto> UpdateItem(CartItemQtyUpdateDto cartItemQtyUpdate);
        
        event Action<int> OnShoppingCartChanged;
        void RaiseEventShoppingCartChanged(int totalQty);
    }
}
