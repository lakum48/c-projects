using System.Net.Http.Json;
using ShopPKS.Client.Models;

namespace ShopPKS.Client.Services
{
    public class ProductService
    {
        private readonly HttpClient _httpClient;

        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Product>>("api/products") ?? new List<Product>();
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            var response = await _httpClient.PostAsJsonAsync("api/products", product);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Product>() ?? throw new Exception("Failed to create product");
        }

        public async Task UpdateProductAsync(int id, Product product)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/products/{id}", product);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteProductAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/products/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
} 