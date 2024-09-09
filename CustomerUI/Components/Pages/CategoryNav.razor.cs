
using Domain.Models;
using Newtonsoft.Json;

namespace CustomerUI.Components.Pages
{
    public partial class CategoryNav
    {

        IEnumerable<Category> categories = new List<Category>();

        protected override async Task OnInitializedAsync()
        {
            //gọi api lấy danh sách category
            using (HttpClient client = new HttpClient())
            {
                using (var response = await client.GetAsync($"https://localhost:7206/api/Category/Categories"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    categories = JsonConvert.DeserializeObject<IEnumerable<Category>>(apiResponse);
                    categories = categories.Where(a => a.Status == "hd").ToList();
                }
            }
        }




    }
}
