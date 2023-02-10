﻿using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;
using ShopOnlineSolution.Blazor.Services.Contracts;

namespace ShopOnlineSolution.Blazor.Pages
{
    public class ProductsByCategoryBase : ComponentBase
    {
        [Parameter]
        public int CategoryId { get; set; }
        [Inject]
        public IManageProductsLocalStorageService ManageProductsLocalStorageService { get; set; }
        public IProductService ProductService { get; set; }
        public IEnumerable<ProductDto> Products { get; set; }
        public string CategoryName { get; set; }
        public string ErrorMessage { get; set; }
        protected override async Task OnParametersSetAsync()
        {
            try
            {
                Products = await GetProductCollectionByCategoryId(CategoryId);
              
                if (Products != null && Products.Count() > 0)
                {
                    var productDto = Products.FirstOrDefault(p => p.CategoryId == CategoryId);
                    if (productDto != null)
                    {
                        CategoryName= productDto.CategoryName;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                throw;
            }
        }
        private async Task<IEnumerable<ProductDto>> GetProductCollectionByCategoryId(int categoryId)
        {
            var productCollection = await ManageProductsLocalStorageService.GetCollection();

            if (productCollection != null)
            {
                return productCollection.Where(x => x.CategoryId ==categoryId);
            }
            else
            {
                return await ProductService.GetProductByCategory(categoryId);
            }
        }
    }
}
