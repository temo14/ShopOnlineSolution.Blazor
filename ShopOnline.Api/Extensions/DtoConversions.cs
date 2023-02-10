using ShopOnline.Api.Entities;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Api.Extensions
{
    public static class DtoConversions
    {
        public static IEnumerable<ProductDto> ConvertToDto(this IEnumerable<Product> products)
        {
            return (from product in products
                    select new ProductDto
                    {
                        Id= product.Id,
                        Name= product.Name,
                        Description=product.Description,
                        ImageUrl=product.ImageURL,
                        Price=product.Price,
                        Qty=product.Qty,
                        CategoryId=product.ProductCategory.Id,
                        CategoryName=product.ProductCategory.Name
                    }).ToList();
        }

        public static ProductDto ConvertToDto(this Product product)
        {
            return new ProductDto
            {
                Id= product.Id,
                Name=product.Name,
                Description=product.Description,
                ImageUrl= product.ImageURL,
                Price=product.Price,
                Qty=product.Qty,
                CategoryId=product.ProductCategory.Id,
                CategoryName=product.ProductCategory.Name
            };
        }

        public static IEnumerable<CartitemDto> ConvertToDto(this IEnumerable<CartItem> cartItems, IEnumerable<Product> products)
        {
            return (from cartItem in cartItems
                    join product in products
                    on cartItem.ProductId equals product.Id
                    select new CartitemDto
                    {
                        Id= cartItem.Id,
                        ProductId= cartItem.ProductId,
                        ProductName=product.Name,
                        ProductDescription=product.Description,
                        ProductImgUrl=product.ImageURL,
                        Price=product.Price,
                        Qty=cartItem.Qty,
                        TotalPrice=product.Price * cartItem.Qty
                    }).ToList();
        }
        public static CartitemDto ConvertToDto(this CartItem item, Product product)
        {
            return new CartitemDto
            {
                Id= item.Id,
                ProductId= item.ProductId,
                ProductName=product.Name,
                ProductDescription=product.Description,
                ProductImgUrl=product.ImageURL,
                Price=product.Price,
                Qty=item.Qty,
                TotalPrice=product.Price * item.Qty
            };
        }
        public static IEnumerable<ProductCategoryDto> ConvertToDto(this IEnumerable<ProductCategory> productCategories)
        {
            return (from productCategory in productCategories
                    select new ProductCategoryDto
                    {
                        Id= productCategory.Id,
                        IconCss= productCategory.IconCss,
                        Name=productCategory.Name,
                    }).ToList();
        }
    }
}
