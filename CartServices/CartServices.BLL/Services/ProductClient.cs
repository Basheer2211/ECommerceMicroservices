using CartServices.BLL.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CartServices.BLL.Services
{
    public class ProductClient : IProductClient
    {
        private readonly HttpClient _http;

        public ProductClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<ProductInfo?> GetProductAsync(long productId)
        {
            var resp = await _http.GetAsync($"/api/Products/{productId}");
            if (!resp.IsSuccessStatusCode)
                return null;

            var product = await resp.Content.ReadFromJsonAsync<ProductInfo>();
            return product;
        }
    }
}
