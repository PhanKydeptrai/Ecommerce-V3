
using Domain.Models;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;

namespace CustomerUI.Components.Pages
{
    public partial class Home
    {
        IEnumerable<Product> products = new List<Product>();
        

        [Parameter]
        public string categoryId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            //gọi api lấy danh sách product
            using (HttpClient client = new HttpClient())
            {
                using (var response = await client.GetAsync($"https://localhost:7206/api/Product/Products"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    products = JsonConvert.DeserializeObject<IEnumerable<Product>>(apiResponse); 
                }
            }

            
        }

        public async Task LoadCategory()
        {
            products = products.Where(a => a.CategoryId == Guid.Parse(categoryId)).ToList();
        }

    }

    
}
