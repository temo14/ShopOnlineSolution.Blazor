using Newtonsoft.Json;
using ShopOnline.Models.Dtos;
using ShopOnlineSolution.Blazor.Services.Contracts;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;

namespace ShopOnlineSolution.Blazor.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly HttpClient _httpClient;

        public ShoppingCartService(HttpClient httpClient)
        {
            _httpClient=httpClient;
        }

        public event Action<int> OnShoppingCartChanged;

        public async Task<CartitemDto> AddItem(CartItemToAddDto cartItemToAddDto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync<CartItemToAddDto>("api/ShoppingCart", cartItemToAddDto);
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default(CartitemDto);
                    }

                    return await response.Content.ReadFromJsonAsync<CartitemDto>();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"HTTP status:{response.StatusCode} - Message - {message}");
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CartitemDto> DeleteItem(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/ShoppingCart/{id}");
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default(CartitemDto);
                    }

                    return await response.Content.ReadFromJsonAsync<CartitemDto>();
                }

                return default(CartitemDto);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<List<CartitemDto>> GetItems(int userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/ShoppingCart/{userId}/GetItems");
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<CartitemDto>().ToList();
                    }
                    return await response.Content.ReadFromJsonAsync<List<CartitemDto>>();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"HTTP status code: {response.StatusCode} Message: {message}");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void RaiseEventShoppingCartChanged(int totalQty)
        {
            if (OnShoppingCartChanged != null)
            {
                OnShoppingCartChanged.Invoke(totalQty);
            }
        }

        public async Task<CartitemDto> UpdateItem(CartItemQtyUpdateDto cartItemQtyUpdate)
        {
            try
            {
                var jsonRequest = JsonConvert.SerializeObject(cartItemQtyUpdate);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json-patch+json");

                var response = await _httpClient.PatchAsync($"api/Shopping/{cartItemQtyUpdate.CartItemId}", content);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<CartitemDto>();
                }

                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
