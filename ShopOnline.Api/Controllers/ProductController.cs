using Microsoft.AspNetCore.Mvc;
using ShopOnline.Api.Extensions;
using ShopOnline.Api.Repositories.Contracts;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository=productRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetItems()
        {
            try
            {
                var products = await _productRepository.GetItems();
                var productCategory = await _productRepository.GetCategories();

                if (products == null || productCategory == null)
                {
                    return NotFound();
                }
                else
                {
                    var productDtos = products.ConvertToDto(productCategory);
                    return Ok(productDtos);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Error retrieving data from the database");
                throw;
            }
        }      
        
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDto>> GetItem(int id)
        {
            try
            {
                var product = await _productRepository.GetItem(id);

                if (product == null)
                {
                    return BadRequest();
                }
                else
                {
                    var productcategory = await _productRepository.GetCategory(product.CategoryId);
                    var productDto = product.ConvertToDto(productcategory);

                    return Ok(productDto);
                }
            }
            catch (Exception)
            {
                return StatusCode(500, "Error retrieving data from the database");
            }
        }

    }
}
