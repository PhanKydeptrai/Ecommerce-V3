using Domain.Models;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace CustomerUI.Components.Pages
{
    public partial class ProductDetail
    {
        [Parameter]
        public string ProductId { get; set; }
        public Product product { get; set; }

        protected override async Task OnInitializedAsync()
        {
            //goi api lấy thông tin product
            using (HttpClient client = new HttpClient())
            {
                using (var response = await client.GetAsync($"https://localhost:7206/api/Product/ProductId/{Guid.Parse(ProductId)}"))
                { 
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    product = JsonConvert.DeserializeObject<Product>(apiResponse);
                    Console.WriteLine(product.ProductName);
                }
            }
        }

    }
}
