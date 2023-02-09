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
        [Route("{userId}/GetItems")]
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

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CartitemDto>> GetItem(int id)
        {
            try
            {
                var cartItem = await _shopingCartRepository.GetItem(id);
                if (cartItem == null)
                {
                    return NotFound();
                }
                var product = await _productRepository.GetItem(cartItem.ProductId);
               
                if (product == null)
                {
                    return NotFound();
                }
                var cartItemDto = cartItem.ConvertToDto(product);

                return Ok(cartItemDto);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<CartitemDto>> PostItem([FromBody]CartItemToAddDto cartItemToAddDto)
        {
            try
            {
                var newCartItem = await _shopingCartRepository.AddItem(cartItemToAddDto);
                if (newCartItem == null)
                {
                    return NoContent();
                }
                var product = await _productRepository.GetItem(newCartItem.ProductId);
                if (product == null)
                {
                    throw new Exception($"Something went wrong when attampting to retrive product (productId:({cartItemToAddDto.ProductId}))");
                }
                var newCartItemDto = newCartItem.ConvertToDto(product);

                return CreatedAtAction(nameof(GetItem), new { id = newCartItemDto.Id }, newCartItemDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<CartitemDto>> DeleteItem(int id)
        {
            try
            {
                var cartItem = await _shopingCartRepository.DeleteItem(id);
                if (cartItem == null)
                    return NotFound();

                var product = await _productRepository.GetItem(cartItem.ProductId);
                if (product == null)
                    return NotFound();

                var cartItemDto = cartItem.ConvertToDto(product);

                return Ok(cartItemDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<CartitemDto>> UpdateQty(int id, CartItemQtyUpdateDto cartItemQtyUpdateDto)
        {
            try
            {
                var cartItem = await _shopingCartRepository.UpdateQty(id, cartItemQtyUpdateDto);
                if (cartItem == null)
                    return NotFound();

                var product = await _productRepository.GetItem(cartItem.ProductId);
                var cartItemDto = cartItem.ConvertToDto(product);
                
                return Ok(cartItemDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
                throw;
            }
        }
    }
}
