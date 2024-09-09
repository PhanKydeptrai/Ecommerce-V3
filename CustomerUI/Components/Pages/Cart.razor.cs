using Domain.Models;
using Newtonsoft.Json;

namespace CustomerUI.Components.Pages
{
    public partial class Cart
    {
        //order có trạng thái waiting
        Order orders = new Order();
        //orderdetail của order
        IEnumerable<OrderDetail> orderDetails = new List<OrderDetail>();


        protected override async Task OnInitializedAsync()
        {
            //gọi api lấy order có trạng thái waiting và orderdetail của order
            using (HttpClient client = new HttpClient())
            {
                using (var apiResponse = await client.GetAsync($"https://localhost:7206/api/Order/GetOrderByStatus/waiting"))
                {
                    string response = await apiResponse.Content.ReadAsStringAsync();
                    orders = JsonConvert.DeserializeObject<Order>(response);
                    orderDetails = orders.OrderDetails;
                }
            }
        }
    }
}
