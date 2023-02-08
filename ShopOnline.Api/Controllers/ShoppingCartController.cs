using Microsoft.AspNetCore.Mvc;
using ShopOnline.Api.Extensions;
using ShopOnline.Api.Repositories.Contracts;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShopingCartRepository _shopingCartRepository;
        private readonly IProductRepository _productRepository;

        public ShoppingCartController(IShopingCartRepository shopingCart,
                                        IProductRepository product)
        {
            _shopingCartRepository=shopingCart;
            _productRepository=product;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartitemDto>>> GetItems(int userId)
        {
            try
            {
                var cartItems = await _shopingCartRepository.GetItems(userId);

                if (cartItems == null)
                {
                    return NoContent();
                }
                var products = await _productRepository.GetItems();
                if (products == null) throw new Exception("No products exists in the system");

                var cartItemsDto = cartItems.ConvertToDto(products);

                return Ok(cartItemsDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
                throw;
            }
        }
    }
}
