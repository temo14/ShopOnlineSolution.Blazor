using Microsoft.EntityFrameworkCore;
using ShopOnline.Api.Data;
using ShopOnline.Api.Entities;
using ShopOnline.Api.Repositories.Contracts;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Api.Repositories
{
    public class ShopingCartRepository : IShopingCartRepository
    {
        private readonly ShopOnlineDbContext _context;

        public ShopingCartRepository(ShopOnlineDbContext context)
        {
            _context=context;
        }
        private async Task<bool> CartItemExists(int cartId, int productId)
        {
            return await _context.Cartitems.AnyAsync(x => x.CartId == cartId &&
                                                        x.ProductId == productId);
        }
        public async Task<CartItem> AddItem(CartItemToAddDto item)
        {
            if (await CartItemExists(item.CartId, item.ProductId) == false)
            {
                var product = await (from prod in _context.Products
                                     where prod.Id == item.ProductId
                                     select new CartItem
                                     {
                                         CartId = item.CartId,
                                         ProductId = prod.Id,
                                         Qty = item.Qty
                                     }).SingleOrDefaultAsync();
                if (product != null)
                {
                    var result = await _context.Cartitems.AddAsync(product);
                    await _context.SaveChangesAsync();
                    return result.Entity;
                }
            }

            return null;
        }

        public Task<CartItem> DeleteItem(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<CartItem> GetItem(int id)
        {
            return await (from cart in _context.Carts
                          join cartItem in _context.Cartitems
                          on cart.Id equals cartItem.CartId
                          where cartItem.Id == id
                          select new CartItem
                          {
                              Id= cartItem.Id,
                              CartId = cartItem.CartId,
                              ProductId = cartItem.ProductId,
                              Qty = cartItem.Qty
                          }).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<CartItem>> GetItems(int userId)
        {
            return await (from cart in _context.Carts
                          join cartItem in _context.Cartitems
                          on cart.Id equals cartItem.CartId
                          where cart.UserId == userId
                          select new CartItem
                          {
                              Id= cartItem.Id,
                              CartId = cartItem.CartId,
                              ProductId = cartItem.ProductId,
                              Qty = cartItem.Qty
                          }).ToListAsync();
        }

        public Task<CartItem> UpdateQty(int id, CartItemQtyUpdateDto item)
        {
            throw new NotImplementedException();
        }
    }
}
